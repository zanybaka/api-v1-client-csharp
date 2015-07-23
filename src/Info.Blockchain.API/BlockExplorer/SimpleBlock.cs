using Info.Blockchain.API.Json;
using Newtonsoft.Json;
using System;

namespace Info.Blockchain.API.BlockExplorer
{
	/// <summary>
	///  Simple representation of a block
	/// </summary>
	public class SimpleBlock
	{
		[JsonConstructor]
		private SimpleBlock()
		{

		}


		protected SimpleBlock(bool mainChain = false)
		{
			this.MainChain = mainChain;
		}

		/// <summary>
		/// Block height
		/// </summary>
		[JsonProperty("height")]
		public long Height { get; private set; }

		/// <summary>
		/// Block hash
		/// </summary>
		[JsonProperty("hash")]
		public string Hash { get; private set; }

		/// <summary>
		/// Block timestamp set by the miner
		/// </summary>
		[JsonProperty("time")]
		[JsonConverter(typeof(UnixDateTimeJsonConverter))]
		public DateTime Time { get; private set; }

		/// <summary>
		/// Whether the block is on the main chain
		/// </summary>
		[JsonProperty("main_chain")]
		[JsonConverter(typeof(TrueTrumpsAllJsonConverter))]
		public bool MainChain { get; private set; }
	}
}
