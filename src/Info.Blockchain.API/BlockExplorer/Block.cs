using Info.Blockchain.API.Json;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;

namespace Info.Blockchain.API.BlockExplorer
{
	/// <summary>
	/// This class is a full representation of a block. For simpler representations, see SimpleBlock and LatestBlock.
	/// </summary>
	public class Block : SimpleBlock
	{
		private DateTime receivedTime = DateTime.MinValue;

		[JsonConstructor]
		private Block() : base()
		{
		}

		/// <summary>
		/// Block version as specified by the protocol
		/// </summary>
		[JsonProperty("ver")]
		public int Version { get; private set; }

		/// <summary>
		/// Hash of the previous block
		/// </summary>
		[JsonProperty("prev_block")]
		public string PreviousBlockHash { get; private set; }

		/// <summary>
		/// Merkle root of the block
		/// </summary>
		[JsonProperty("mrkl_root")]
		public string MerkleRoot { get; private set; }

		/// <summary>
		/// Representation of the difficulty target for this block
		/// </summary>
		[JsonProperty("bits")]
		public long Bits { get; private set; }

		/// <summary>
		/// Total transaction fees from this block
		/// </summary>
		[JsonProperty("fee")]
		[JsonConverter(typeof(BitcoinValueJsonConverter))]
		public BitcoinValue Fees { get; private set; }

		/// <summary>
		/// Block nonce
		/// </summary>
		[JsonProperty("nonce")]
		public long Nonce { get; private set; }

		/// <summary>
		/// Serialized size of this block
		/// </summary>
		[JsonProperty("size")]
		public long Size { get; private set; }

		/// <summary>
		/// Index of this block
		/// </summary>
		[JsonProperty("block_index")]
		public long Index { get; private set; }

		/// <summary>
		/// The time this block was received by Blockchain.info
		/// </summary>
		[JsonProperty("received_time")]
		[JsonConverter(typeof(UnixDateTimeJsonConverter))]
		public DateTime ReceivedTime
		{
			get
			{
				if (this.receivedTime == DateTime.MinValue)
				{
					return this.Time;
				}
				return this.receivedTime;
			}
			private set
			{
				this.receivedTime = value;
			}
		}

		/// <summary>
		/// IP address that relayed the block
		/// </summary>
		[JsonProperty("relayed_by")]
		public string RelayedBy { get; private set; }

		/// <summary>
		/// Transactions in the block
		/// </summary>
		[JsonProperty("tx")] 
		public ReadOnlyCollection<Transaction> Transactions { get; private set; }
	}
}
