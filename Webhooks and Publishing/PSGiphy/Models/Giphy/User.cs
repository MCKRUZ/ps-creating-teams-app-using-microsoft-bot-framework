using System;
using Newtonsoft.Json;

namespace PSGiphy.Models.Giphy
{
	public class User
	{
		[JsonProperty("avatar_url")]
		public Uri AvatarUrl { get; set; }
		[JsonProperty("banner_image")]
		public Uri BannerImage { get; set; }
		[JsonProperty("banner_url")]
		public Uri BannerUrl { get; set; }
		[JsonProperty("profile_url")]
		public Uri ProfileUrl { get; set; }
		[JsonProperty("username")]
		public string Username { get; set; }
		[JsonProperty("display_name")]
		public string DisplayName { get; set; }
		[JsonProperty("is_verified")]
		public bool IsVerified { get; set; }
	}
}
