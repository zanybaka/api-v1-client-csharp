##`Info.Blockchain.Api.Wallet` namespace

The `Wallet` namespace contains the `Wallet` class that reflects the functionality documented at https://github.com/blockchain/service-my-wallet-v3. It allows users to directly interact with their existing Blockchain.info wallet, send funds, manage addresses etc.

Example usage:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Info.Blockchain.API;
using Info.Blockchain.API.BlockExplorer;
using Info.Blockchain.API.Wallet;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (BlockchainApiHelper apiHelper = new BlockchainApiHelper(apiCode: "your-api-code", serviceUrl: "url-to-service-my-wallet-v3"))
            {
                try
                {
                    WalletHelper helper = apiHelper.CreateWalletHelper("your-wallet-guid", "your-wallet-password");

                    // get an address from your wallet and include only transactions with up to 3
                    // confirmations in the balance
                    Info.Blockchain.API.Wallet.Address addr = await helper.GetAddressAsync("15urYnyeJe3gwbGJ74wcX89Tz7ZtsFDVew", 3);
                    Console.WriteLine("The balance is {0}", addr.Balance);

                    // create a new address and attach a label to it
                    Info.Blockchain.API.Wallet.Address newAddr = await helper.NewAddress("test label 123");
                    Console.WriteLine("The new address is {0}", newAddr.AddressStr);

                    // list the wallet balance
                    BitcoinValue totalBalance = await helper.GetBalanceAsync();
                    Console.WriteLine("The total wallet balance is {0} BTC", totalBalance.Btc);

                    // send 0.2 bitcoins with a custom fee of 100,000 satoshis and a note
                    // public notes require a minimum transaction value of 0.005 BTC
                    BitcoinValue fee = BitcoinValue.FromSatoshis(10000);
                    BitcoinValue amount = BitcoinValue.FromSatoshis(20000000);
                    PaymentResponse payment = await helper.SendAsync("1dice6YgEVBf88erBFra9BHf6ZMoyvG88", amount, fee: fee, note: "Amazon payment");
                    Console.WriteLine("The payment TX hash is {0}", payment.TxHash);

                    // list all addresses and their balances (with 0 confirmations)
                    List<Info.Blockchain.API.Wallet.Address> addresses = await helper.ListAddressesAsync(0);
                    foreach (var a in addresses)
                    {
                        Console.WriteLine("The address {0} has a balance of {1}", a.AddressStr, a.Balance);
                    }

                    // archive an old address
                    await helper.ArchiveAddress("1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa");
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
