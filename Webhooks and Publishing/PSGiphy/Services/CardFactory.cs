using System;
using System.Collections.Generic;
using System.Linq;
using AdaptiveCards;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using PSGiphy.Models.Bot;
using PSGiphy.Models.Common;
using PSGiphy.Models.Giphy;

namespace PSGiphy.Services
{
	public static class CardFactory
	{
		private const string adaptiveCardSchemaVersion = "1.2";

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

		public static AdaptiveCard CreateSearchCard()
		{
			return new AdaptiveCard(new AdaptiveSchemaVersion(adaptiveCardSchemaVersion))
			{
				Id = "searchCard",
				Body = new List<AdaptiveElement>
				{
					new AdaptiveTextInput()
					{
						Id = "search",
						Placeholder = "Search"

					},
					new AdaptiveTextBlock()
					{
						Text = "Content Rating"
					},
					new AdaptiveChoiceSetInput()
					{
						Id = "rating",
						IsMultiSelect = false,
						Value = "r",
						Choices = new List<AdaptiveChoice>()
						{
							new AdaptiveChoice()
							{
								Title = "G",
								Value = "g",
							},
							new AdaptiveChoice()
							{
								Title = "PG",
								Value = "pg"
							},
							new AdaptiveChoice()
							{
								Title = "PG-13",
								Value = "pg13"
							},
							new AdaptiveChoice()
							{
								Title = "R",
								Value = "r"
							}
						}
					}
				},
				Actions = new List<AdaptiveAction>
				{
					new AdaptiveSubmitAction()
					{
						Id = "cancel",
						Type = AdaptiveSubmitAction.TypeName,
						Title = "Cancel",
						Data = new JObject { { "action", "Cancel" }, { "card", "Search" } }
					},
					new AdaptiveSubmitAction()
					{
						Id = "search",
						Type = AdaptiveSubmitAction.TypeName,
						Title = "Search",
						Data = new JObject { { "action", "Ok" }, { "card", "Search" } }
					}
				}
			};

		}

		public static AdaptiveCard CreateResultsCard(AdaptiveCardModel model, List<Datum> images)
		{
			// Create all the columns
			var columns = new List<AdaptiveColumn>();
			foreach (var item in SplitList<Datum>(images, 25).ToList())
			{
				columns.Add(AdaptiveColumnBuilder(AdaptiveImageBuilder(item)));
			}

			// Create return objects
			var cancelObject = new AdaptiveCardModel();
			cancelObject.Action = Enums.ActionType.Cancel;
			cancelObject.Card = Enums.CardType.Selection;

			// Create return objects
			var moreObject = new AdaptiveCardModel();
			moreObject.Offset = model.Offset;
			moreObject.Search = model.Search;
			moreObject.Rating = model.Rating;
			moreObject.Action = Enums.ActionType.Ok;
			moreObject.Card = Enums.CardType.Search;


			return new AdaptiveCard(new AdaptiveSchemaVersion(adaptiveCardSchemaVersion))
			{
				Id = "resultsCard",
				Height = AdaptiveHeight.Stretch,
				VerticalContentAlignment = AdaptiveVerticalContentAlignment.Top,
				Body = new List<AdaptiveElement>()
				{
					new AdaptiveActionSet()
					{
						Actions = new List<AdaptiveAction>
						{
							new AdaptiveSubmitAction()
							{
								Title = "Cancel",
								Data = cancelObject
							},
							new AdaptiveSubmitAction()
							{
								Title = "Get More",
								Data = moreObject
							}
						}
					},
					new AdaptiveColumnSet()
					{
						Height = AdaptiveHeight.Stretch,
						Columns = columns
					},

				}
			};
		}

		public static IEnumerable<List<T>> SplitList<T>(List<T> items, int nSize = 30)
		{
			for (int i = 0; i < items.Count; i += nSize)
			{
				yield return items.GetRange(i, Math.Min(nSize, items.Count - i));
			}
		}

		private static AdaptiveColumn AdaptiveColumnBuilder(List<AdaptiveElement> images)
		{
			return new AdaptiveColumn()
			{
				Width = "10000",
				Items = images
			};
		}

		public static List<AdaptiveElement> AdaptiveImageBuilder(List<Datum> images)
		{
			var returnList = new List<AdaptiveElement>();
			foreach (var item in images)
			{
				returnList.Add(
					new AdaptiveImage()
					{
						Url = new Uri(item.Images.DownsizedLarge.Url.AbsoluteUri, UriKind.Absolute),
						HorizontalAlignment = AdaptiveHorizontalAlignment.Center,
						Size = AdaptiveImageSize.Stretch,
						Spacing = AdaptiveSpacing.Small,
						SelectAction = new AdaptiveSubmitAction()
						{
							Data = new JObject { { "action", "Ok" }, { "card", "Selection" }, { "url", item.Images.DownsizedLarge.Url.AbsoluteUri } }
						}
					});
			}
			return returnList;
		}
	}
}