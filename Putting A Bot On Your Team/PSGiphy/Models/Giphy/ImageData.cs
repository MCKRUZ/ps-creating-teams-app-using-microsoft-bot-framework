using System;
using Newtonsoft.Json;
using PSGiphy.Helpers.Converters;

namespace PSGiphy.Models.Giphy
{
	public class ImageData
	{
		[JsonProperty("url")]
		public Uri Url { get; set; }
		[JsonProperty("width")]
		[JsonConverter(typeof(ParseStringConverter))]
		public long Width { get; set; }
		[JsonProperty("height")]
		[JsonConverter(typeof(ParseStringConverter))]
		public long Height { get; set; }
		[JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
		[JsonConverter(typeof(ParseStringConverter))]
		public long? Size { get; set; }
		[JsonProperty("mp4", NullValueHandling = NullValueHandling.Ignore)]
		public Uri Mp4 { get; set; }
		[JsonProperty("mp4_size", NullValueHandling = NullValueHandling.Ignore)]
		[JsonConverter(typeof(ParseStringConverter))]
		public long? Mp4Size { get; set; }
		[JsonProperty("webp")]
		public Uri Webp { get; set; }
		[JsonProperty("webp_size")]
		[JsonConverter(typeof(ParseStringConverter))]
		public long WebpSize { get; set; }
		[JsonProperty("frames", NullValueHandling = NullValueHandling.Ignore)]
		[JsonConverter(typeof(ParseStringConverter))]
		public long? Frames { get; set; }
		[JsonProperty("hash", NullValueHandling = NullValueHandling.Ignore)]
		public string Hash { get; set; }
	}
}