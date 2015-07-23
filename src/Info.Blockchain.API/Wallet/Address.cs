using Info.Blockchain.API.BlockExplorer;
using Info.Blockchain.API.Json;
using Newtonsoft.Json;

namespace Info.Blockchain.API.Wallet
{
	/// <summary>
	/// Used in combination with the `Wallet` class
	/// </summary>
	public class Address
	{
		[JsonConstructor]
		private Address()
		{
		}


		/// <summary>
		/// Balance in bitcoins
		/// </summary>
		[JsonProperty("balance")]
		[JsonConverter(typeof(BitcoinValueJsonConverter))]
		public BitcoinValue Balance { get; private set; }

		/// <summary>
		/// String representation of the address
		/// </summary>
		[JsonProperty("address")]
		public string AddressStr { get; private set; }

		/// <summary>
		/// Label attached to the address
		/// </summary>
		[JsonProperty("label")]
		public string Label { get; private set; }

		/// <summary>
		/// Total received amount
		/// </summary>
		[JsonProperty("total_received")]
		[JsonConverter(typeof(BitcoinValueJsonConverter))]
		public BitcoinValue TotalReceived { get; private set; }
	}
}
