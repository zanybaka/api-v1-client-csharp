using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Info.Blockchain.API.BlockExplorer
{
    /// <summary>
    /// Used as a response to the `GetLatestBlock` method in the `BlockExplorer` class.
    /// </summary>
    public class LatestBlock : SimpleBlock
    {
        public LatestBlock(JObject b) : base(b, true)
        {
            Index = (long)b["block_index"];

            var txs = b["txIndexes"].AsJEnumerable().Select(x => (long)x).ToList();
            TransactionIndexes = txs.AsReadOnly();
        }

        /// <summary>
        /// Block index
        /// </summary>
        public long Index { get; private set; }

        /// <summary>
        /// Transaction indexes included in this block
        /// </summary>
        public ReadOnlyCollection<long> TransactionIndexes { get; private set; }
    }
}
