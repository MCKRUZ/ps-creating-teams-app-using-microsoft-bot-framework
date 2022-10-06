using Newtonsoft.Json;
using PSGiphy.Models.Common;
using static PSGiphy.Models.Common.Enums;

namespace PSGiphy.Models.Bot
{
	public class AdaptiveCardModel
	{

		public AdaptiveCardModel()
		{
			Offset = "0";
		}

		[JsonProperty("search")]
		public string Search { get; set; }
		[JsonProperty("url")]
		public string Url { get; set; }
		[JsonProperty("offset")]
		public string Offset { get; set; }
		[JsonProperty("rating")]
		public Rating Rating { get; set; }
		[JsonProperty("action")]
		public ActionType Action { get; set; }
		[JsonProperty("card")]
		public CardType Card { get; set; }
	}
}