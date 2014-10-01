##`Info.Blockchain.Api.Receive` namespace

The `Createwallet` namespace contains the `CreateWallet` class that reflects the functionality documented at https://blockchain.info/api/create_walleti. It allows users to create new wallets as long as their API code has the 'generate wallet' permission. 

Example usage:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Info.Blockchain.API;
using Info.Blockchain.API.CreateWallet;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var newWallet = CreateWallet.Create("someComplicated123Password", "8fd2335e-720c-442b-9694-83bdd2983cc9");

                Console.WriteLine("The new wallet identifier is: {0}", newWallet.Identifier);
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
