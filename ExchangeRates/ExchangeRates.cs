using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

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

            string response = HttpClient.Get("ticker", req);
            JObject tickerObj = JObject.Parse(response);

            var resultDict = new Dictionary<string, Currency>();
            foreach (var t in tickerObj.Properties())
            {
                var ccy = new Currency(
                    (double)t.Value["buy"],
                    (double)t.Value["sell"],
                    (double)t.Value["last"],
                    (double)t.Value["15m"],
                    (string)t.Value["symbol"]);

                resultDict[t.Name] = ccy;
            }

            return resultDict;
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

            string response = HttpClient.Get("tobtc", req);
            return double.Parse(response);
        }
    }
}
