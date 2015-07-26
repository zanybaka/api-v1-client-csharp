using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Info.Blockchain.API.Json
{
	internal class UnixDateTimeJsonConverter : DateTimeConverterBase
	{
		private static DateTime epoch { get; } = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		public static DateTime GenesisBlockDate { get; } = UnixDateTimeJsonConverter.UnixSecondsToDateTime(1231006505);
		public const long GenesisBlockUnixMillis = 1231006505000;

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			object value = reader.Value;
			if (reader.Value is long)
			{
				value = (double)(long) value;
			}
			if (value is double)
			{
				return UnixDateTimeJsonConverter.UnixSecondsToDateTime((double) value);
			}
			return null;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value is DateTime)
			{
				double untixTimestamp = UnixDateTimeJsonConverter.DateTimeToUnixSeconds((DateTime) value);
				writer.WriteRawValue(untixTimestamp.ToString());
			}
		}

		internal static double DateTimeToUnixSeconds(DateTime dateTime)
		{
			return (dateTime - UnixDateTimeJsonConverter.epoch).TotalSeconds;
        }

		private static DateTime UnixSecondsToDateTime(double unixSeconds)
		{
			return UnixDateTimeJsonConverter.UnixMillisToDateTime(unixSeconds * 1000d);
		}
		private static DateTime UnixMillisToDateTime(double unixMillis)
		{
			DateTime dateTime = UnixDateTimeJsonConverter.epoch.AddMilliseconds(unixMillis);
			if (dateTime < UnixDateTimeJsonConverter.GenesisBlockDate)
			{
				throw new ArgumentOutOfRangeException(nameof(unixMillis), "No date can be before the genesis block (2009-01-03T18:15:05+00:00)");
			}
			if (dateTime > DateTime.UtcNow)
			{
				throw new ArgumentOutOfRangeException(nameof(unixMillis), "No future date should exist as a value"); //TODO remove?
			}
			return dateTime;
		}
	}
}
