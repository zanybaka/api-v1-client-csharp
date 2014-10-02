using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Info.Blockchain.API.BlockExplorer
{
    /// <summary>
    /// Represents a transaction.
    /// </summary>
    public class Transaction
    {
        public Transaction(JObject t, long? blockHeight = null, bool? doubleSpend = null)
        {
            DoubleSpend = doubleSpend != null ? doubleSpend.Value : (bool)t["double_spend"];
            BlockHeight = blockHeight != null ? blockHeight.Value : (long)t["block_height"];
            Time = (long)t["time"];
            RelayedBy = (string)t["relayed_by"];
            Hash = (string)t["hash"];
            Index = (long)t["tx_index"];
            Version = (int)t["ver"];
            Size = (long)t["size"];

            var ins = t["inputs"].AsJEnumerable().Select(x => new Input((JObject)x)).ToList();
            Inputs = new ReadOnlyCollection<Input>(ins);

            var outs = t["out"].AsJEnumerable().Select(x => new Output((JObject)x)).ToList();
            Outputs = new ReadOnlyCollection<Output>(outs);
        }

        /// <summary>
        /// Whether the transaction is a double spend
        /// </summary>
        public bool DoubleSpend { get; private set; }

        /// <summary>
        /// Block height of the parent block. -1 for unconfirmed transactions.
        /// </summary>
        public long BlockHeight { get; private set; }

        /// <summary>
        /// Timestamp of the transaction (unix time in seconds)
        /// </summary>
        public long Time { get; private set; }

        /// <summary>
        /// IP address that relayed the transaction
        /// </summary>
        public string RelayedBy { get; private set; }

        /// <summary>
        /// Transaction hash
        /// </summary>
        public string Hash { get; private set; }

        /// <summary>
        /// Transaction index
        /// </summary>
        public long Index { get; private set; }

        /// <summary>
        /// Transaction format version
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        /// Serialized size of the transaction
        /// </summary>
        public long Size { get; private set; }

        /// <summary>
        /// List of inputs
        /// </summary>
        public ReadOnlyCollection<Input> Inputs { get; private set; }

        /// <summary>
        /// List of outputs
        /// </summary>
        public ReadOnlyCollection<Output> Outputs { get; private set; }
    }
}
