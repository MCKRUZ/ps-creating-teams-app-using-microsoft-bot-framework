using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PSGiphy.Helpers
{
	public static class HttpRequestExtensions
	{
		/// <summary>
		/// Retrieve the raw body as a string from the Request.Body stream
		/// </summary>
		/// <param name="request">Request instance to apply to</param>
		/// <param name="encoding">Optional - Encoding, defaults to UTF8</param>
		/// <returns></returns>
		public static async Task<string> GetRawBodyStringAsync(this HttpRequest request, Encoding encoding = null)
		{
			if (encoding == null)
				encoding = Encoding.UTF8;

			using (var memoryStream = new MemoryStream())
			{
				await request.Body.CopyToAsync(memoryStream).ConfigureAwait(false);
				memoryStream.Seek(0, SeekOrigin.Begin);
				using (var reader = new StreamReader(memoryStream, encoding))
				{
					return await reader.ReadToEndAsync();
				}
			}
		}
	}

}
