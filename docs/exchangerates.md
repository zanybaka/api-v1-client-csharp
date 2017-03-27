## `Info.Blockchain.API.ExchangeRates` namespace

The `ExchangeRates` namespace contains the `ExchangeRateExplorer` class that reflects the functionality documented at https://blockchain.info/api/exchange_rates_api. It allows users to get price tickers for most major currencies and directly convert fiat amounts to BTC.

Example usage:

```csharp
using System;
using Info.Blockchain.API.Client;
using Info.Blockchain.API.ExchangeRates;

namespace TestApp
{
    class Program
    {
        private static ExchangeRateExplorer explorer;

        static void Main(string[] args)
        {
            // create a new Exchange Rate Explorer
            explorer = new ExchangeRateExplorer();

            try
            {
                var ticker = explorer.GetTickerAsync().Result;
                foreach (var key in ticker.Keys)
                {
                    Console.WriteLine("The last price of BTC in {0} is {1}", key, ticker[key].Last);
                }

                double btcAmount = explorer.ToBtcAsync("USD", 1500).Result;
                Console.WriteLine("1500 USD equals {0} BTC", btcAmount);
            }
            catch (ClientApiException e)
            {
                Console.WriteLine("Blockchain exception: " + e.Message);
            }
        }
    }
}
```
