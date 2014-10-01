##`Info.Blockchain.Api.Statistics` namespace

The `Statistics` namespace contains the `Statistics` class that reflects the functionality documented at at https://blockchain.info/api/charts_api. It makes various network statistics available, such as the total number of blocks in existence, next difficulty retarget block, total BTC mined in the past 24 hours etc.

Example usage:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Info.Blockchain.API;
using Info.Blockchain.API.Statistics;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var stats = Statistics.Get();

                Console.WriteLine("The current difficulty is {0}. The next retarget will happen in {1} hours",
                    stats.Difficulty,
                    (int)((stats.NextRetarget - stats.TotalBlocks) * stats.MinutesBetweenBlocks / 60));
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
