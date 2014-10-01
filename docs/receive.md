##`Info.Blockchain.Api.Receive` namespace

The `Receive` namespace contains the `Receive` class that reflects the functionality documented at at https://blockchain.info/api/api_receive. It allows merchants to create forwarding addresses and be notified upon payment.

Example usage:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Info.Blockchain.API;
using Info.Blockchain.API.Receive;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var rec = Receive.ReceiveFunds("1F9cWF6j4vZbvdPiG7gXd8aJ1tNNmXCtxE", "http://www.yourwebsite.com");
                Console.WriteLine("The receiving address is {0}. The payment will be forwarded to {1}",
                    rec.InputAddress, rec.DestinationAddress);
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