using System;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace PSGiphy.Helpers
{
	public static class AuthHelper
	{
		public static Tuple<bool, string> Validate(AuthenticationHeaderValue authenticationHeaderValue, string messageContent, string signingKey)
		{
			// Perform Error Checks
			if (authenticationHeaderValue == null)
				return new Tuple<bool, string>(false, "Authentication header not present on request.");

			if (!string.Equals("HMAC", authenticationHeaderValue.Scheme))
				return new Tuple<bool, string>(false, "Incorrect authorization header scheme.");

			var providedHmacValue = authenticationHeaderValue.Parameter;

			try
			{
				var serializedPayloadBytes = Encoding.UTF8.GetBytes(messageContent);
				string calculatedHmacValue = null;
				var keyBytes = Convert.FromBase64String(signingKey);
				using (HMACSHA256 hmacSHA256 = new HMACSHA256(keyBytes))
				{
					var hashBytes = hmacSHA256.ComputeHash(serializedPayloadBytes);
					calculatedHmacValue = Convert.ToBase64String(hashBytes);
				}

				if (string.Equals(providedHmacValue, calculatedHmacValue))
				{
					return new Tuple<bool, string>(true, null);
				}
				else
				{
					var errorMessage = string.Format(
						"AuthHeaderValueMismatch. Expected:'{0}' Provided:'{1}'",
						calculatedHmacValue,
						providedHmacValue);
					return new Tuple<bool, string>(false, errorMessage);
				}
			}
			catch (Exception ex)
			{
				return new Tuple<bool, string>(false, "Exception thrown while verifying MAC on incoming request.");
			}
		}
	}
}