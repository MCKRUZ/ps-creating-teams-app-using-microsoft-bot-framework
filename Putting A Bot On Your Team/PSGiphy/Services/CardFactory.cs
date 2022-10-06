using System.Collections.Generic;
using Microsoft.Bot.Schema;

namespace PSGiphy.Services
{
	public static class CardFactory
	{
		public static Attachment CreateHeroCard(string title, string subTitle, string text, string[] images, string[] buttons)
		{
			var heroCard = new HeroCard()
			{
				Title = title,
				Subtitle = subTitle,
				Text = text,
				Images = new List<CardImage>(),
				Buttons = new List<CardAction>(),
			};

			// Set images
			if (images != null)
			{
				foreach (var img in images)
				{
					heroCard.Images.Add(new CardImage()
					{
						Url = img,
						Alt = img,
					});
				}
			}

			// Set buttons
			if (buttons != null)
			{
				foreach (var btn in buttons)
				{
					heroCard.Buttons.Add(new CardAction()
					{
						Title = "Open",
						Type = ActionTypes.OpenUrl,
						Value = btn,
					});
				}
			}
			return new Attachment()
			{
				ContentType = HeroCard.ContentType,
				Content = heroCard,
			};
		}
	}
}