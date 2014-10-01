##`Info.Blockchain.Api.Exchangerates` namespace

The `Exchangerates` namespace contains the `ExchangeRates` class that reflects the functionality documented at https://blockchain.info/api/exchange_rates_api. It allows users to get price tickers for most major currencies and directly convert fiat amounts to BTC.

Example usage:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Info.Blockchain.API;
using Info.Blockchain.API.ExchangeRates;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var ticker = ExchangeRates.GetTicker();
                foreach (var key in ticker.Keys)
                {
                    Console.WriteLine("The last price of BTC in {0} is {1}", key, ticker[key].Last);
                }

                double btcAmount = ExchangeRates.ToBTC("USD", 1500);
                Console.WriteLine("1500 USD equals {0} BTC", btcAmount);
            }
            catch (APIException e)
            {
                Console.WriteLine("Blockchain exception: " + e.Message);
            }

            Console.ReadLine();
        }
    }
}

```
