using Info.Blockchain.API.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Info.Blockchain.API.BlockExplorer
{
    /// <summary>
    /// This class is a full representation of a block. For simpler representations, see SimpleBlock and LatestBlock.
    /// </summary>
    public class Block : SimpleBlock
    {
        public Block(JObject b) : base(b)
        {
            Version = (int)b["ver"];
            PreviousBlockHash = (string)b["prev_block"];
            MerkleRoot = (string)b["mrkl_root"];
            Bits = (long)b["bits"];
            Fees = BitcoinValue.FromSatoshis((long)b["fee"]);
            Nonce = (long)b["nonce"];
            Size = (long)b["size"];
            Index = (long)b["block_index"];
			ReceivedTime = b["received_time"] != null ? DateTimeUtil.UnixTimeStampToDateTime((long)b["received_time"]) : this.Time;
            RelayedBy = b["relayed_by"] != null ? (string)b["relayed_by"] : null;

            var txs = b["tx"].Select(x => new Transaction((JObject)x, Height, false)).ToList();
            Transactions = new ReadOnlyCollection<Transaction>(txs);
        }

        /// <summary>
        /// Block version as specified by the protocol
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        /// Hash of the previous block
        /// </summary>
        public string PreviousBlockHash { get; private set; }

        /// <summary>
        /// Merkle root of the block
        /// </summary>
        public string MerkleRoot { get; private set; }

        /// <summary>
        /// Representation of the difficulty target for this block
        /// </summary>
        public long Bits { get; private set; }

        /// <summary>
        /// Total transaction fees from this block
        /// </summary>
        public BitcoinValue Fees { get; private set; }

        /// <summary>
        /// Block nonce
        /// </summary>
        public long Nonce { get; private set; }

        /// <summary>
        /// Serialized size of this block
        /// </summary>
        public long Size { get; private set; }

        /// <summary>
        /// Index of this block
        /// </summary>
        public long Index { get; private set; }

        /// <summary>
        /// The time this block was received by Blockchain.info
        /// </summary>
        public DateTime ReceivedTime { get; private set; }

        /// <summary>
        /// IP address that relayed the block
        /// </summary>
        public string RelayedBy { get; private set; }

        /// <summary>
        /// Transactions in the block
        /// </summary>
        public ReadOnlyCollection<Transaction> Transactions { get; private set; }
    }
}
