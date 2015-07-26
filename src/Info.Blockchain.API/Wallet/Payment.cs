using System.Collections.Generic;
using System.Linq;
using Info.Blockchain.API.BlockExplorer;
using Newtonsoft.Json;
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Info.Blockchain.API.Wallet
{
	/// <summary>
	/// Used as a request object to the `sendMany` methods in the `Wallet` class.
	/// </summary>
	internal class ManyPaymentRequest
	{
		[JsonProperty("fee")]
		public BitcoinValue? Fee { get; }
		[JsonProperty("from")]
		public string FromAddress { get; }
		[JsonProperty("note")]
		public string Note { get; }
		[JsonProperty("main_password")]
		public string Password { get; }
		[JsonProperty("second_password")]
		public string SecondPassword { get; }
		[JsonProperty("recipients")]
		public Dictionary<string, long> Recipients { get; }

		public ManyPaymentRequest(string password, string secondPassword, Dictionary<string, BitcoinValue> recipients, string fromAddress = null, BitcoinValue? fee = null, string note = null)
		{
			this.Recipients = recipients.ToDictionary(kv => kv.Key, kv => kv.Value.Satoshis);
			this.FromAddress = fromAddress;
			this.Fee = fee;
			this.Note = note;
			this.Password = password;
			this.SecondPassword = secondPassword;
		}
	}

	/// <summary>
	/// Used as a request object to the `send` methods in the `Wallet` class.
	/// </summary>
	internal class SinglePaymentRequest
	{
		[JsonProperty("fee")]
		public BitcoinValue? Fee { get; }
		[JsonProperty("from")]
		public string FromAddress { get; }
		[JsonProperty("note")]
		public string Note { get; }
		[JsonProperty("main_password")]
		public string Password { get; }
		[JsonProperty("second_password")]
		public string SecondPassword { get; }
		[JsonProperty("to")]
		public string ToAddress { get; }
		[JsonProperty("amount")]
		public long SendAmount { get; }

		public SinglePaymentRequest(string password, string secondPassword, string toAddress, long sendAmount, string fromAddress = null, BitcoinValue? fee = null, string note = null)
		{
			this.ToAddress = toAddress;
			this.SendAmount = sendAmount;
			this.FromAddress = fromAddress;
			this.Fee = fee;
			this.Note = note;
			this.Password = password;
			this.SecondPassword = secondPassword;
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
