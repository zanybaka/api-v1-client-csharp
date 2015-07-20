using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Info.Blockchain.API.BlockExplorer
{
    /// <summary>
    /// Represents a transaction output.
    /// </summary>
    public class Output
    {
        public Output(JObject o, bool? spent = null)
        {
            N = (int)o["n"];
            Value = BitcoinValue.FromSatoshis((long)o["value"]);
            Address = (string)o["addr"];
            TxIndex = (long)o["tx_index"];
            Script = (string)o["script"];
            Spent = spent != null ? spent.Value : (bool)o["spent"];
        }

        /// <summary>
        /// Index of the output in a transaction
        /// </summary>
        public int N { get; private set; }

        /// <summary>
        /// Value of the output
        /// </summary>
        public BitcoinValue Value { get; private set; }

        /// <summary>
        /// Address that the output belongs to
        /// </summary>
        public string Address { get; private set; }

        /// <summary>
        /// Transaction index
        /// </summary>
        public long TxIndex { get; private set; }

        /// <summary>
        /// Output script
        /// </summary>
        public string Script { get; private set; }

        /// <summary>
        /// Whether the output is spent
        /// </summary>
        public bool Spent { get; private set; }
    }
}
