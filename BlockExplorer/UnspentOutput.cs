using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Info.Blockchain.API.BlockExplorer
{
    /// <summary>
    /// Represents an unspent transaction output.
    /// </summary>
    public class UnspentOutput
    {
        public UnspentOutput(JObject o)
        {
            N = (int)o["tx_output_n"];
            TransactionHash = (string)o["tx_hash"];
            TransactionIndex = (long)o["tx_index"];
            Script = (string)o["script"];
            Value = (long)o["value"];
            Confirmations = (long)o["confirmations"];
        }

        /// <summary>
        /// Index of the output in a transaction
        /// </summary>
        public int N { get; private set; }

        /// <summary>
        /// Transaction hash
        /// </summary>
        public string TransactionHash { get; private set; }

        /// <summary>
        /// Transaction index
        /// </summary>
        public long TransactionIndex { get; private set; }

        /// <summary>
        /// Output script
        /// </summary>
        public string Script { get; private set; }

        /// <summary>
        /// Value of the output (in satoshi)
        /// </summary>
        public long Value { get; private set; }

        /// <summary>
        /// Number of confirmations
        /// </summary>
        public long Confirmations { get; private set; }
    }
}
