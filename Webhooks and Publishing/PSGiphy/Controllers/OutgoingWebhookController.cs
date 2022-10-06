using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Schema;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PSGiphy.Helpers;
using PSGiphy.Models.Bot;
using PSGiphy.Models.Common;
using PSGiphy.Services;
using Serilog;

namespace PSGiphy.Controllers
{
	[ApiController]
	public class OutgoingWebhookController : ControllerBase
	{
		private readonly ILogger _logger;
		private readonly string _giphyKey;
		private readonly string _giphyUrl;
		private readonly string _webhookKey;

		public OutgoingWebhookController(ILogger logger, IConfiguration configuration)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_giphyKey = configuration["GiphyKey"] ?? throw new ArgumentNullException(nameof(configuration));
			_giphyUrl = configuration["GiphyUrl"] ?? throw new ArgumentNullException(nameof(configuration));
			_webhookKey = configuration["WebhookKey"] ?? throw new ArgumentNullException(nameof(configuration));
		}

		/// <summary>
		/// Gets the data.
		/// </summary>
		/// <returns>The response activity.</returns>
		[HttpPost]
		[Route("api/OutgoingWebhook")]
		public async Task<Activity> GetData()
		{
			var content = await this.Request.GetRawBodyStringAsync().ConfigureAwait(false);
			var authHeader = AuthenticationHeaderValue.Parse(Request.Headers[HeaderNames.Authorization]);
			var authResponse = AuthHelper.Validate(authHeader, content, _webhookKey);
			var response = new Activity();

			if (!authResponse.Item1)
			{
				response.Text = "You are not authorized to call into this end point.";
			}
			else
			{
				var activity = JsonConvert.DeserializeObject<Activity>(content);
				response.Text = "Here are your top 3 matching gifs ";
				response.Attachments = new List<Attachment>();

				var actionData = new AdaptiveCardModel()
				{
					Offset = "0",
					Rating = Enums.Rating.G,
					Search = activity.Text.Substring(activity.Text.IndexOf("</at>", StringComparison.InvariantCultureIgnoreCase) + 5).RemoveSpecialCharacters()
				};

				var images = await GiphyService.GetGifsAsync(actionData.Search, actionData.Rating, actionData.Offset, _giphyUrl, _giphyKey, _logger);
				foreach (var img in images.Item2.Take(3))
				{
					var heroCard = CardFactory.CreateHeroCard("", "", "", new string[] { img.Images.DownsizedLarge.Url.AbsoluteUri }, new string[] { img.Url.AbsoluteUri });
					response.Attachments.Add(heroCard);
				}
			}

			return response;
		}
	}
}

