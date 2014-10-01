##`Info.Blockchain.Api.Wallet` namespace

The `Wallet` namespace contains the `Wallet` class that reflects the functionality documented at at https://blockchain.info/api/blockchain_wallet_api. It allows users to directly interact with their existing Blockchain.info wallet, send funds, manage addresses etc.

Example usage:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Info.Blockchain.API;
using Info.Blockchain.API.Wallet;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Wallet wallet = new Wallet("18d4cf81-fcec-4a94-aa93-202a25085c0e", "yourPassword123");

                // get an address from your wallet and include only transactions with up to 3
    	        // confirmations in the balance
    	        Address addr = wallet.GetAddress("1JzSZFs2DQke2B3S4pBxaNaMzzVZaG4Cqh", 3);
                Console.WriteLine("The balance is {0}", addr.Balance);

                // send 0.2 bitcoins with a custom fee of 0.01 BTC and a note
                PaymentResponse payment = wallet.Send("1dice6YgEVBf88erBFra9BHf6ZMoyvG88",
                    20000000, fee: 1000000L, note: "Amazon payment");
                Console.WriteLine("The payment TX hash is {0}", payment.TxHash);

                // list the wallet balance
                long totalBalance = wallet.GetBalance();
                Console.WriteLine("The total wallet balance is {0}", totalBalance);

                // list all addresses and their balances (with 0 confirmations)
                List<Address> addresses = wallet.ListAddresses(0);
                foreach (var a in addresses)
                {
                    Console.WriteLine("The address {0} has a balance of {1}", a.AddressStr, a.Balance);
                }

                // archive an old address
                wallet.ArchiveAddress("1JzSZFs2DQke2B3S4pBxaNaMzzVZaG4Cqh");

                // consolidate addresses that have been inactive more than 25 days
    	        List<String> consolidated = wallet.Consolidate(25);
    	        foreach (string c in consolidated)
    	        {
                    Console.WriteLine("Address {0} has been consolidated", c);
    	        }

                // create a new address and attach a label to it
    	        Address newAddr = wallet.NewAddress("test label 123");
    	        Console.WriteLine("The new address is {0}", newAddr.AddressStr);
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
