using Newtonsoft.Json;

namespace Info.Blockchain.API.CreateWallet
{
	/// <summary>
	/// This class is used in response to the `Create` method in the `CreateWallet` class.
	/// </summary>
	public class CreateWalletResponse
	{
		[JsonConstructor]
		private CreateWalletResponse()
		{
		}

		/// <summary>
		/// Wallet identifier (GUID)
		/// </summary>
		[JsonProperty("guid")]
		public string Identifier { get; private set; }

		/// <summary>
		/// First address in the wallet
		/// </summary>
		[JsonProperty("address")]
		public string Address { get; private set; }

		/// <summary>
		/// Link to the wallet
		/// </summary>
		[JsonProperty("link")]
		public string Link { get; private set; }
	}
}
