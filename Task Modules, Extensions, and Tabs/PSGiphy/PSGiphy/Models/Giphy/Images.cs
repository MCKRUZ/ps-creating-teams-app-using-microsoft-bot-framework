
using Newtonsoft.Json;

namespace PSGiphy.Models.Giphy
{
    public class Images
    {
        [JsonProperty("downsized_large")]
        public ImageData DownsizedLarge { get; set; }

        [JsonProperty("fixed_height_small_still")]
        public ImageData FixedHeightSmallStill { get; set; }

        [JsonProperty("original")]
        public ImageData Original { get; set; }

        [JsonProperty("fixed_height_downsampled")]
        public ImageData FixedHeightDownsampled { get; set; }

        [JsonProperty("downsized_still")]
        public ImageData DownsizedStill { get; set; }

        [JsonProperty("fixed_height_still")]
        public ImageData FixedHeightStill { get; set; }

        [JsonProperty("downsized_medium")]
        public ImageData DownsizedMedium { get; set; }

        [JsonProperty("downsized")]
        public ImageData Downsized { get; set; }

        [JsonProperty("preview_webp")]
        public ImageData PreviewWebp { get; set; }

        [JsonProperty("original_mp4")]
        public ImageData OriginalMp4 { get; set; }

        [JsonProperty("fixed_height_small")]
        public ImageData FixedHeightSmall { get; set; }

        [JsonProperty("fixed_height")]
        public ImageData FixedHeight { get; set; }

        [JsonProperty("downsized_small")]
        public ImageData DownsizedSmall { get; set; }

        [JsonProperty("preview")]
        public ImageData Preview { get; set; }

        [JsonProperty("fixed_width_downsampled")]
        public ImageData FixedWidthDownsampled { get; set; }

        [JsonProperty("fixed_width_small_still")]
        public ImageData FixedWidthSmallStill { get; set; }

        [JsonProperty("fixed_width_small")]
        public ImageData FixedWidthSmall { get; set; }

        [JsonProperty("original_still")]
        public ImageData OriginalStill { get; set; }

        [JsonProperty("fixed_width_still")]
        public ImageData FixedWidthStill { get; set; }

        [JsonProperty("looping")]
        public ImageData Looping { get; set; }

        [JsonProperty("fixed_width")]
        public ImageData FixedWidth { get; set; }

        [JsonProperty("preview_gif")]
        public ImageData PreviewGif { get; set; }

        [JsonProperty("480w_still")]
        public ImageData The480WStill { get; set; }

        [JsonProperty("hd", NullValueHandling = NullValueHandling.Ignore)]
        public ImageData Hd { get; set; }
    }
}
