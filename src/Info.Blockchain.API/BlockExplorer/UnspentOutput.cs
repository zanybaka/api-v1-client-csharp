using Info.Blockchain.API.Json;
using Newtonsoft.Json;

namespace Info.Blockchain.API.BlockExplorer
{
	/// <summary>
	/// Represents an unspent transaction output.
	/// </summary>
	public class UnspentOutput
	{
		[JsonConstructor]
		private UnspentOutput()
		{
		}

		/// <summary>
		/// Index of the output in a transaction
		/// </summary>
		[JsonProperty("tx_output_n")]
		public int N { get; private set; }

		/// <summary>
		/// Transaction hash
		/// </summary>
		[JsonProperty("tx_hash")]
		public string TransactionHash { get; private set; }

		/// <summary>
		/// Transaction index
		/// </summary>
		[JsonProperty("tx_index")]
		public long TransactionIndex { get; private set; }

		/// <summary>
		/// Output script
		/// </summary>
		[JsonProperty("script")]
		public string Script { get; private set; }

		/// <summary>
		/// Value of the output
		/// </summary>
		[JsonProperty("value")]
		[JsonConverter(typeof(BitcoinValueJsonConverter))]
		public BitcoinValue Value { get; private set; }

		/// <summary>
		/// Number of confirmations
		/// </summary>
		[JsonProperty("confirmations")]
		public long Confirmations { get; private set; }
	}
}
