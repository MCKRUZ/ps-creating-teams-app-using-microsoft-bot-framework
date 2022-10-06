using Newtonsoft.Json;

namespace PSGiphy.Models.Giphy
{
	public class Analytics
	{
		[JsonProperty("onload")]
		public Onclick Onload { get; set; }

		[JsonProperty("onclick")]
		public Onclick Onclick { get; set; }

		[JsonProperty("onsent")]
		public Onclick Onsent { get; set; }
	}
}
