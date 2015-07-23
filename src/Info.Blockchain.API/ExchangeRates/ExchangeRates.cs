using Newtonsoft.Json;
using System.Collections.Generic;

namespace Info.Blockchain.API.ExchangeRates
{
	/// <summary>
	/// This class reflects the functionality documented at https://blockchain.info/api/exchange_rates_api. 
	/// It allows users to fetch the latest ticker data and convert amounts between BTC and fiat currencies.
	/// </summary>
	public class ExchangeRates
	{
		/// <summary>
		/// Gets the price ticker from https://blockchain.info/ticker
		/// </summary>
		/// <param name="apiCode">Blockchain.info API code</param>
		/// <returns>A dictionary of currencies where the key is a 3-letter currency symbol
		/// and the value is the `Currency` class</returns>
		/// <exception cref="APIException">If the server returns an error</exception>
		public static Dictionary<string, Currency> GetTicker(string apiCode = null)
		{
			var req = new NameValueCollection();
			if (apiCode != null)
				req["api_code"] = apiCode;

			string response = HttpClientUtil.Get("ticker", req);
			Dictionary<string, Currency> currencyDictionary = JsonConvert.DeserializeObject<Dictionary<string, Currency>>(response);

			return currencyDictionary;
		}

		/// <summary>
		/// Converts x value in the provided currency to BTC.
		/// </summary>
		/// <param name="currency">Currency code</param>
		/// <param name="value">Value to convert</param>
		/// <param name="apiCode">Blockchain.info API code</param>
		/// <returns>Converted value in BTC</returns>
		/// <exception cref="APIException">If the server returns an error</exception>
		public static double ToBTC(string currency, double value, string apiCode = null)
		{
			var req = new NameValueCollection();
			req["currency"] = currency;
			req["value"] = value.ToString();
			if (apiCode != null)
				req["api_code"] = apiCode;

			string response = HttpClientUtil.Get("tobtc", req);
			return double.Parse(response);
		}
	}
}
