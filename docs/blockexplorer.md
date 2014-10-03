##`Info.Blockchain.Api.BlockExplorer` namespace

The `BlockExplorer` namespace contains the `BlockExplorer` class that reflects the functionality documented at  https://blockchain.info/api/blockchain_api. It can be used to query the block chain, fetch block, transaction and address data, get unspent outputs for an address etc.

Example usage:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Info.Blockchain.API;
using Info.Blockchain.API.BlockExplorer;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // instantiate a block explorer
                var blockExplorer = new BlockExplorer();

                // get a transaction by hash and list the value of all its inputs
                var tx = blockExplorer.GetTransaction("df67414652722d38b43dcbcac6927c97626a65bd4e76a2e2787e22948a7c5c47");
    	        foreach (Input i in tx.Inputs)
    	        {
                    Console.WriteLine(i.PreviousOutput.Value);
    	        }

                // get a block by hash and read the number of transactions in the block
                var block = blockExplorer.GetBlock("0000000000000000050fe18c9b961fc7c275f02630309226b15625276c714bf1");
                int numberOfTxsInBlock = block.Transactions.Count;

                // get an address and read its final balance
                var address = blockExplorer.GetAddress("1EjmmDULiZT2GCbJSeXRbjbJVvAPYkSDBw");
                long finalBalance = address.FinalBalance;

                // get a list of currently unconfirmed transactions and print the relay IP address for each
    	        var unconfirmedTxs = blockExplorer.GetUnconfirmedTransactions();
    	        foreach (Transaction unconfTx in unconfirmedTxs)
                {
                    Console.WriteLine(unconfTx.RelayedBy);
                }

                // calculate the balanace of an address by fetching a list of all its unspent outputs
    	        var outs = blockExplorer.GetUnspentOutputs("1EjmmDULiZT2GCbJSeXRbjbJVvAPYkSDBw");
                long totalUnspentValue = outs.Sum(x => x.Value);

                // get inventory data for a recent transaction (valid up to one hour)
                //var inv = blockExplorer.GetInventoryData("f23ee3525f78df032b47c3c9be6cd0d930f38c32674e861c1e41c6558b32ee97");

                // get the latest block on the main chain and read its height
                var latestBlock = blockExplorer.GetLatestBlock();
                long latestBlockHeight = latestBlock.Height;

                // use the previous block height to get a list of blocks at that height
		        // and detect a potential chain fork
    	        var blocksAtHeight = blockExplorer.GetBlocksAtHeight(latestBlockHeight);
    	        if (blocksAtHeight.Count > 1)
    		        Console.WriteLine("The chain has forked!");
    	        else
    		        Console.WriteLine("The chain is still in one piece :)");

                // get a list of all blocks that were mined today since 00:00 UTC
                var todaysBlocks = blockExplorer.GetBlocks();
                Console.WriteLine(todaysBlocks.Count);
                
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
