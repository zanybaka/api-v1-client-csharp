using System.Collections.Generic;
using Info.Blockchain.API.BlockExplorer;
using Newtonsoft.Json;
using System.Linq;

namespace Info.Blockchain.API.Wallet
{
	/// <summary>
	/// Used as a request object to the `sendMany` methods in the `Wallet` class.
	/// </summary>
	public class ManyPaymentRequest
	{
		[JsonProperty("fee")]
		public BitcoinValue? fee { get; }
		[JsonProperty("from")]
		public string fromAddress { get; }
		[JsonProperty("note")]
		public string note { get; }
		[JsonProperty("main_password")]
		public string password { get; }
		[JsonProperty("second_password")]
		public string secondPassword { get; }
		[JsonProperty("recipients")]
		public Dictionary<string, long> recipients { get; }

		public ManyPaymentRequest(string password, string secondPassword, Dictionary<string, BitcoinValue> recipients, string fromAddress = null, BitcoinValue? fee = null, string note = null)
		{
			this.recipients = recipients.ToDictionary(kv => kv.Key, kv => kv.Value.Satoshis);
			this.fromAddress = fromAddress;
			this.fee = fee;
			this.note = note;
			this.password = password;
			this.secondPassword = secondPassword;
		}
	}

	/// <summary>
	/// Used as a request object to the `send` methods in the `Wallet` class.
	/// </summary>
	public class SinglePaymentRequest
	{
		[JsonProperty("fee")]
		public BitcoinValue? fee { get; }
		[JsonProperty("from")]
		public string fromAddress { get; }
		[JsonProperty("note")]
		public string note { get; }
		[JsonProperty("main_password")]
		public string password { get; }
		[JsonProperty("second_password")]
		public string secondPassword { get; }
		[JsonProperty("to")]
		public string toAddress { get; }
		[JsonProperty("amount")]
		public long sendAmount { get; }

		public SinglePaymentRequest(string password, string secondPassword, string toAddress, long sendAmount, string fromAddress = null, BitcoinValue? fee = null, string note = null)
		{
			this.toAddress = toAddress;
			this.sendAmount = sendAmount;
			this.fromAddress = fromAddress;
			this.fee = fee;
			this.note = note;
			this.password = password;
			this.secondPassword = secondPassword;
		}
	}


	/// <summary>
	/// Used as a response object to the `send` and `sendMany` methods in the `Wallet` class.
	/// </summary>
	public class PaymentResponse
	{
		[JsonConstructor]
		private PaymentResponse()
		{
		}

		/// <summary>
		/// Response message from the server
		/// </summary>
		[JsonProperty("message")]
		public string Message { get; private set; }

		/// <summary>
		/// Transaction hash
		/// </summary>
		[JsonProperty("tx_hash")]
		public string TxHash { get; private set; }

		/// <summary>
		/// Additional response message from the server
		/// </summary>
		[JsonProperty("notice")]
		public string Notice { get; private set; }
	}
}
