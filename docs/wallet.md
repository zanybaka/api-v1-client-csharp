## `Info.Blockchain.API.Wallet` namespace

The `Wallet` namespace contains the `Wallet` class that reflects the functionality documented at https://github.com/blockchain/service-my-wallet-v3. It allows users to directly interact with their existing Blockchain.info wallet, send funds, manage addresses etc.

Example usage:

```csharp
using System;
using System.Collections.Generic;
using Info.Blockchain.API.Client;
using Info.Blockchain.API.Wallet;
using Info.Blockchain.API.BlockExplorer;

namespace TestApp
{
    class Program
    {
        private static Wallet wallet;

        static void Main(string[] args)
        {
            using (ApiHelper apiHelper = new ApiHelper(apiCode: "1770d5d9-bcea-4d28-ad21-6cbd5be018a8", serviceUrl: "http://127.0.0.1:3000/"))
            {
                try
                {
                    // create an instance of an existing wallet
                    wallet = apiHelper.CreateWallet("wallet-identifier", "someComplicated123Password");

                    // create a new address and attach a label to it
                    WalletAddress newAddr = wallet.NewAddress("test label 123").Result;
                    Console.WriteLine("The new address is {0}", newAddr.AddressStr);

                    // get an address from your wallet and include only transactions with up to 3
                    // confirmations in the balance (in this example we use the address just created)
                    WalletAddress addr = wallet.GetAddressAsync(newAddr.AddressStr, 3).Result;
                    Console.WriteLine("The balance is {0}", addr.Balance);

                    // list the wallet balance
                    BitcoinValue totalBalance = wallet.GetBalanceAsync().Result;
                    Console.WriteLine("The total wallet balance is {0} BTC", totalBalance.GetBtc());

                    // send 0.2 bitcoins with a custom fee of 100,000 satoshis and a note
                    // public notes require a minimum transaction value of 0.005 BTC
                    // this will throw an error if insufficient wallet funds
                    BitcoinValue fee = BitcoinValue.FromSatoshis(10000);
                    BitcoinValue amount = BitcoinValue.FromSatoshis(20000000);
                    PaymentResponse payment = wallet.SendAsync("1dice6YgEVBf88erBFra9BHf6ZMoyvG88", amount, fee: fee, note: "Amazon payment").Result;
                    Console.WriteLine("The payment TX hash is {0}", payment.TxHash);

                    // list all addresses and their balances (with 0 confirmations)
                    List<WalletAddress> addresses = wallet.ListAddressesAsync(0).Result;
                    foreach (var a in addresses)
                    {
                        Console.WriteLine("The address {0} has a balance of {1}", a.AddressStr, a.Balance);
                    }

                    // archive an old address
                    wallet.ArchiveAddress(addr.AddressStr).Wait();
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
