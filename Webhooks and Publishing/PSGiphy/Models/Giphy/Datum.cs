using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PSGiphy.Helpers.Converters;
using static PSGiphy.Models.Common.Enums;

namespace PSGiphy.Models.Giphy
{
	public class Datum
	{
		[JsonProperty("type")]
		public string Type { get; set; }
		[JsonProperty("id")]
		public string Id { get; set; }
		[JsonProperty("url")]
		public Uri Url { get; set; }
		[JsonProperty("slug")]
		public string Slug { get; set; }
		[JsonProperty("bitly_gif_url")]
		public Uri BitlyGifUrl { get; set; }
		[JsonProperty("bitly_url")]
		public Uri BitlyUrl { get; set; }
		[JsonProperty("embed_url")]
		public Uri EmbedUrl { get; set; }
		[JsonProperty("username")]
		public string Username { get; set; }
		[JsonProperty("source")]
		public string Source { get; set; }
		[JsonProperty("title")]
		public string Title { get; set; }
		[JsonProperty("rating")]
		[JsonConverter(typeof(RatingConverter))]
		public Rating Rating { get; set; }
		[JsonProperty("content_url")]
		public string ContentUrl { get; set; }
		[JsonProperty("source_tld")]
		public string SourceTld { get; set; }
		[JsonProperty("source_post_url")]
		public string SourcePostUrl { get; set; }
		[JsonProperty("is_sticker")]
		public long IsSticker { get; set; }
		[JsonProperty("import_datetime")]
		public DateTimeOffset ImportDatetime { get; set; }
		[JsonProperty("trending_datetime")]
		public string TrendingDatetime { get; set; }
		[JsonProperty("images")]
		public Images Images { get; set; }
		[JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
		public User User { get; set; }
		[JsonProperty("analytics_response_payload")]
		public string AnalyticsResponsePayload { get; set; }
		[JsonProperty("analytics")]
		public Analytics Analytics { get; set; }
	}

}
