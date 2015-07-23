using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Info.Blockchain.API.Json
{
	internal class UnixDateTimeJsonConverter : DateTimeConverterBase
	{
		private static DateTime epoch { get; } = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.Value is long)
			{
				double millisecondsAfterEpoch = (long)reader.Value / 1000d;
				return epoch.AddMilliseconds(millisecondsAfterEpoch);
			}
			return null;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value is DateTime)
			{
				string unixTimestamp = ((DateTime)value - epoch).TotalMilliseconds + "000";
				writer.WriteRawValue(unixTimestamp);
			}
		}
	}
}
