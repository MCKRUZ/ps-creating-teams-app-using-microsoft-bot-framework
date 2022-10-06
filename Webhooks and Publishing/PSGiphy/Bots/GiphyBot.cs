using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using AdaptiveCards;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PSGiphy.Models.Bot;
using PSGiphy.Models.Common;
using PSGiphy.Services;
using Serilog;

namespace PSGiphy.Bots
{
	public class GiphyBot : TeamsActivityHandler
	{
		private readonly ILogger _logger;
		private readonly string _giphyKey;
		private readonly string _giphyUrl;

		public GiphyBot(ILogger logger, IConfiguration configuration)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_giphyKey = configuration["GiphyKey"] ?? throw new ArgumentNullException(nameof(configuration));
			_giphyUrl = configuration["GiphyUrl"] ?? throw new ArgumentNullException(nameof(configuration));
		}

		protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
		{
			// Remove the @ entry for the bot
			turnContext.Activity.RemoveRecipientMention();

			// Retrieve the text from the message
			var text = turnContext.Activity.Text.Trim().ToLower();

			// Create a mention
			var mention = new Mention
			{
				Mentioned = turnContext.Activity.From,
				Text = $"<at>{XmlConvert.EncodeName(turnContext.Activity.From.Name)}</at>",
			};

			// Create a response
			var response = MessageFactory.Text($"Hello {mention.Text}.  Here are your top 3 matching gifs ");
			response.Entities = new List<Entity> { mention };

			// Retrieve all images
			var images = await GiphyService.GetGifsAsync(text, Enums.Rating.G, "0", _giphyUrl, _giphyKey, _logger);

			// Create cards for each item.
			foreach (var img in images.Item2.Take(3))
			{
				var heroCard = CardFactory.CreateHeroCard("", "", "", new string[] { img.Images.DownsizedLarge.Url.AbsoluteUri }, new string[] { img.Url.AbsoluteUri });
				response.Attachments.Add(heroCard);
			}

			await turnContext.SendActivityAsync(response, cancellationToken);

			// Execute Base Code
			await base.OnMessageActivityAsync(turnContext, cancellationToken);
		}

		protected override async Task<MessagingExtensionResponse> OnTeamsMessagingExtensionQueryAsync(ITurnContext<IInvokeActivity> turnContext, MessagingExtensionQuery query, CancellationToken cancellationToken)
		{
			var text = query?.Parameters?[0]?.Value as string ?? string.Empty;
			var images = await GiphyService.GetGifsAsync(text, Enums.Rating.G, "0", _giphyUrl, _giphyKey, _logger);

			var attachments = new List<MessagingExtensionAttachment>();
			foreach (var item in images.Item2)
			{
				var attachment = new MessagingExtensionAttachment
				{
					ContentType = HeroCard.ContentType,
					Content = new HeroCard { Images = new List<CardImage>() { new CardImage() { Url = item.Images.DownsizedLarge.Url.AbsoluteUri } } },
				};
				attachments.Add(attachment);
			}

			return new MessagingExtensionResponse
			{
				ComposeExtension = new MessagingExtensionResult
				{
					Type = "result",
					AttachmentLayout = "grid",
					Attachments = attachments
				}
			};
		}

		protected override async Task<MessagingExtensionActionResponse> OnTeamsMessagingExtensionFetchTaskAsync(ITurnContext<IInvokeActivity> turnContext, MessagingExtensionAction action, CancellationToken cancellationToken)
		{
			try
			{
				var advancedSearch = CardFactory.CreateSearchCard();

				return new MessagingExtensionActionResponse
				{
					Task = new TaskModuleContinueResponse
					{
						Value = new TaskModuleTaskInfo
						{
							Card = new Attachment
							{
								Content = advancedSearch,
								ContentType = AdaptiveCard.ContentType,
							},
							Height = 200,
							Width = 500,
							Title = "Search Giphy Library",
						},
					},
				};

			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message, ex);
			}
			return null;
		}

		protected override async Task<MessagingExtensionActionResponse> OnTeamsMessagingExtensionSubmitActionAsync(ITurnContext<IInvokeActivity> turnContext, MessagingExtensionAction action, CancellationToken cancellationToken)
		{
			try
			{
				var actionData = JsonConvert.DeserializeObject<AdaptiveCardModel>(action.Data.ToString());
				switch (actionData.Card)
				{
					case Enums.CardType.Search:
						if (actionData.Action == Enums.ActionType.Ok)
						{
							var images = await GiphyService.GetGifsAsync(actionData.Search, actionData.Rating, actionData.Offset, _giphyUrl, _giphyKey, _logger);
							actionData.Offset = images.Item1;
							var adaptiveCard = CardFactory.CreateResultsCard(actionData, images.Item2);
							return new MessagingExtensionActionResponse
							{
								Task = new TaskModuleContinueResponse
								{
									Value = new TaskModuleTaskInfo
									{
										Card = new Attachment
										{
											Content = adaptiveCard,
											ContentType = AdaptiveCard.ContentType,
										},
										Height = 500,
										Width = 500,
										Title = "Giphy Results",
									},
								},
							};
						}
						break;
					case Enums.CardType.Selection:
						if (actionData.Action == Enums.ActionType.Ok)
						{
							var reply = MessageFactory.Text(@"<div><img src='" + actionData.Url + "'></div></div>");
							await turnContext.SendActivityAsync(reply, cancellationToken);
						}
						break;
					default:
						break;
				}

			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message, ex);
			}
			return null;
		}

	}
}
