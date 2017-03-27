## `Info.Blockchain.API.CreateWallet` namespace

The CreateWallet namespace contains the CreateWallet class that reflects the functionality documented at https://blockchain.info/api/create_wallet. It allows users to create new wallets as long as their API code has the 'generate wallet' permission.

Example usage:

```csharp
using System;
using System.Collections.Generic;
using Info.Blockchain.API.Client;
using Info.Blockchain.API.CreateWallet;

namespace TestApp
{
    class Program
    {
        private static WalletCreator walletCreator;

        static void main(string[] args)
        {
            using (ApiHelper apiHelper = new ApiHelper(apiCode: "1770d5d9-bcea-4d28-ad21-6cbd5be018a8", serviceUrl: "http://127.0.0.1:3000/"))
            {
                try
                {
                    walletCreator = apiHelper.CreateWalletCreator();

                    // create a new wallet
                    var newWallet = _walletCreator.Create("someComplicated123Password", label: "some-optional-label").Result;
                    Console.WriteLine("The new wallet identifier is: {0}", newWallet.Identifier);
                }
                catch (ClientApiException e)
                {
                    Console.WriteLine("Blockchain exception: " + e.Message);
                }
            }
        }
    }
}
```