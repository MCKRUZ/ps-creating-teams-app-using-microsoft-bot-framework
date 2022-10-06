using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Extensions.Configuration;
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

		#region Advanced
		protected override async Task OnTeamsChannelCreatedAsync(ChannelInfo channelInfo, TeamInfo teamInfo, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
		{
			var heroCard = new HeroCard(text: $"{channelInfo.Name} is the Channel created");
			await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);
		}

		protected override async Task OnTeamsChannelRenamedAsync(ChannelInfo channelInfo, TeamInfo teamInfo, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
		{
			var heroCard = new HeroCard(text: $"{channelInfo.Name} is the new Channel name");
			await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);
		}

		protected override async Task OnTeamsChannelDeletedAsync(ChannelInfo channelInfo, TeamInfo teamInfo, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
		{
			var heroCard = new HeroCard(text: $"{channelInfo.Name} is the Channel deleted");
			await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);
		}

		protected override async Task OnTeamsMembersAddedAsync(IList<TeamsChannelAccount> teamsMembersAdded, TeamInfo teamInfo, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
		{
			foreach (TeamsChannelAccount member in teamsMembersAdded)
			{
				if (member.Id == turnContext.Activity.Recipient.Id)
				{
					// Send a message to introduce the bot to the team
					var heroCard = new HeroCard(text: $"The {member.Name} bot has joined {teamInfo.Name}");
					await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);
				}
				else
				{
					var heroCard = new HeroCard(text: $"{member.Name} joined {teamInfo.Name}");
					await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);
				}
			}
		}

		protected override async Task OnTeamsMembersRemovedAsync(IList<TeamsChannelAccount> teamsMembersRemoved, TeamInfo teamInfo, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
		{
			foreach (TeamsChannelAccount member in teamsMembersRemoved)
			{
				if (member.Id == turnContext.Activity.Recipient.Id)
				{
					// The bot was removed
					// You should clear any cached data you have for this team
				}
				else
				{
					var heroCard = new HeroCard(text: $"{member.Name} was removed from {teamInfo.Name}");
					await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);
				}
			}
		}

		protected override async Task OnTeamsTeamRenamedAsync(TeamInfo teamInfo, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
		{
			var heroCard = new HeroCard(text: $"{teamInfo.Name} is the new Team name");
			await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);
		}

		protected override async Task OnReactionsAddedAsync(IList<MessageReaction> messageReactions, ITurnContext<IMessageReactionActivity> turnContext, CancellationToken cancellationToken)
		{
			foreach (var reaction in messageReactions)
			{
				var newReaction = $"You reacted with '{reaction.Type}' to the following message: '{turnContext.Activity.ReplyToId}'";
				var replyActivity = MessageFactory.Text(newReaction);
				var resourceResponse = await turnContext.SendActivityAsync(replyActivity, cancellationToken);
			}
		}

		protected override async Task OnReactionsRemovedAsync(IList<MessageReaction> messageReactions, ITurnContext<IMessageReactionActivity> turnContext, CancellationToken cancellationToken)
		{
			foreach (var reaction in messageReactions)
			{
				var newReaction = $"You removed the reaction '{reaction.Type}' from the following message: '{turnContext.Activity.ReplyToId}'";
				var replyActivity = MessageFactory.Text(newReaction);
				var resourceResponse = await turnContext.SendActivityAsync(replyActivity, cancellationToken);
			}
		}
		#endregion
	}
}
