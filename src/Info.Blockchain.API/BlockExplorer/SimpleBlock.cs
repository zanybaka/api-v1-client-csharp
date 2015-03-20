using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Info.Blockchain.API.BlockExplorer
{
    /// <summary>
    ///  Simple representation of a block
    /// </summary>
    public class SimpleBlock
    {

        public SimpleBlock(JObject b, bool? mainChain = false)
        {
            Height = (long)b["height"];
            Hash = (string)b["hash"];
            Time = (long)b["time"];
            MainChain = mainChain != null ? mainChain.Value : (bool)b["main_chain"];
        }

        /// <summary>
        /// Block height
        /// </summary>
        public long Height { get; private set; }

        /// <summary>
        /// Block hash
        /// </summary>
        public string Hash { get; private set; }

        /// <summary>
        /// Block timestamp set by the miner (unix time in seconds)
        /// </summary>
        public long Time { get; private set; }

        /// <summary>
        /// Whether the block is on the main chain
        /// </summary>
        public bool MainChain { get; private set; }
    }
}
