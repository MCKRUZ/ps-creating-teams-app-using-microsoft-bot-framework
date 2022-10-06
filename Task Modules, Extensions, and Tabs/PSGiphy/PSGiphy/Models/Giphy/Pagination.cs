using Newtonsoft.Json;

namespace PSGiphy.Models.Giphy
{
	public class Pagination
	{
		[JsonProperty("total_count")]
		public long TotalCount { get; set; }
		[JsonProperty("count")]
		public long Count { get; set; }
		[JsonProperty("offset")]
		public long Offset { get; set; }
	}
}
