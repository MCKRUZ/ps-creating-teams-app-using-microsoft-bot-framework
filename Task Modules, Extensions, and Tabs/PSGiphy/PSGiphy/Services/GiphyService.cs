using Newtonsoft.Json;
using PSGiphy.Models.Common;
using PSGiphy.Models.Giphy;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using PSGiphy.Helpers;
using Serilog;
using static PSGiphy.Models.Common.Enums;

namespace PSGiphy.Services
{
	public static class GiphyService
	{
		public static async Task<Tuple<string, List<Datum>>> GetGifsAsync(string searchQuery, Rating rating, string offset, string gifUrl, string apiKey, ILogger log)
		{
			var client = new HttpClient();
			var gifs = new List<Datum>();

			try
			{
				// Create the call
				var url = GifSearch(gifUrl, apiKey, searchQuery, rating, offset);

				// Log the Url
				log.Information("Pulling data from " + url);

				// Issue the call
				var responseBody = await client.GetAsync(url);

				// Interpret the response
				var response = JsonConvert.DeserializeObject<GiphySearchResponse>(responseBody.Content.ReadAsStringAsync().Result);

				// Add the new images
				gifs.AddRange(response.Data);

				// Setup for next call
				if ((response.Pagination == null) || (response.Pagination.TotalCount == response.Pagination.Offset + response.Pagination.Count))
					offset = "0";
				else
					offset = (response.Pagination.Offset + response.Pagination.Count).ToString();

				log.Information(string.Format("Successfully retrieved all images. Total: {0}", gifs.Count.ToString()));

				return Tuple.Create(offset, gifs);
			}
			catch (Exception e)
			{
				log.Error(e, e.Message);
			}
			return null;
		}

		public static string GifSearch(string gifURL, string apiKey, string searchQuery, Enums.Rating rating, string offset)
		{
			var returnURL = string.Format("{0}/gifs/search?api_key={1}", gifURL, apiKey);
			returnURL = string.Format("{0}&limit={1}", returnURL, "100");
			returnURL = string.Format("{0}&lang={1}", returnURL, "en");
			returnURL = string.Format("{0}&rating={1}", returnURL, Common.ConvertRatingToString(rating));
			returnURL = string.Format("{0}&offset={1}", returnURL, offset);
			returnURL = string.Format("{0}&q={1}", returnURL, searchQuery);
			return returnURL;
		}
	}
}
