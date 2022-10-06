using System;
using Newtonsoft.Json;

namespace PSGiphy.Models.Giphy
{
	public class Onclick
	{
		[JsonProperty("url")]
		public Uri Url { get; set; }
	}
}
