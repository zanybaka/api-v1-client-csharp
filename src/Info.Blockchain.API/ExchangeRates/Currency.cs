using Newtonsoft.Json;
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Info.Blockchain.API.ExchangeRates
{
	/// <summary>
	/// This class is used in the response of the `GetTicker` method in the `ExchangeRates` class.
	/// </summary>
	public class Currency
	{
		[JsonConstructor]
		private Currency()
		{
		}

		/// <summary>
		/// Current buy price
		/// </summary>
		[JsonProperty("buy")]
		public double Buy { get; private set; }

		/// <summary>
		/// Current sell price
		/// </summary>
		[JsonProperty("sell")]
		public double Sell { get; private set; }

		/// <summary>
		/// Most recent market price
		/// </summary>
		[JsonProperty("last")]
		public double Last { get; private set; }

		/// <summary>
		/// 15 minutes delayed market price
		/// </summary>
		[JsonProperty("15m")]
		public double Price15M { get; private set; }

		/// <summary>
		/// Currency symbol
		/// </summary>
		[JsonProperty("symbol")]
		public string Symbol { get; private set; }
	}
}
