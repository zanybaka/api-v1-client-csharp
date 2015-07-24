using Newtonsoft.Json;

namespace Info.Blockchain.API.CreateWallet
{
	public class CreateWalletRequest
	{
		[JsonProperty("email")]
		private string Email { get; }
		[JsonProperty("label")]
		private string Label { get; }
		[JsonProperty("password")]
		private string Password { get; }
		[JsonProperty("priv")]
		private string PrivateKey { get; }

		public CreateWalletRequest(string password, string privateKey = null, string label = null, string email = null)
		{
			this.Password = password;
			this.PrivateKey = privateKey;
			this.Label = label;
			this.Email = email;
		}
	}

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
