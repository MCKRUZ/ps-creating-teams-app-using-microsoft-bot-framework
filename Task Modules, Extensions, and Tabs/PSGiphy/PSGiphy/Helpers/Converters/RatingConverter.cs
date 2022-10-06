using Newtonsoft.Json;
using System;
using static PSGiphy.Models.Common.Enums;

namespace PSGiphy.Helpers.Converters
{
	internal class RatingConverter : JsonConverter
	{
		public override bool CanConvert(Type t) => t == typeof(Rating) || t == typeof(Rating?);
		public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null) return null;
			var value = serializer.Deserialize<string>(reader);
			switch (value)
			{
				case "g":
					return Rating.G;
				case "pg":
					return Rating.PG;
				case "pg-13":
					return Rating.PG13;
				default:
					return Rating.G;
			}
		}
		public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
		{
			if (untypedValue == null)
			{
				serializer.Serialize(writer, null);
				return;
			}
			var value = (Rating)untypedValue;
			switch (value)
			{
				case Rating.G:
					serializer.Serialize(writer, "g");
					return;
				case Rating.PG:
					serializer.Serialize(writer, "pg");
					return;
				case Rating.PG13:
					serializer.Serialize(writer, "pg-13");
					return;
				case Rating.R:
					serializer.Serialize(writer, "r");
					return;
				default:
					serializer.Serialize(writer, "g");
					return;
			}
		}
		public static readonly RatingConverter Singleton = new RatingConverter();
	}
}