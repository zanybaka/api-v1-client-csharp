using Newtonsoft.Json;

namespace Info.Blockchain.API.Receive
{

	public class ReceiveRequest
	{
		[JsonProperty("address")]
		public string ReceivingAddress { get; }
		[JsonProperty("callback")]
		public string CallbackUrl { get; }
		[JsonProperty("method")]
		public string Method { get; }

		public ReceiveRequest(string receivingAddress, string callbackUrl)
		{
			this.ReceivingAddress = receivingAddress;
			this.CallbackUrl = callbackUrl;
			this.Method = "create";
		}
	}


	/// <summary>
	/// This class is used as a response object to the `Receive.receive` method. 
	/// </summary>
	public class ReceiveResponse
	{
		[JsonConstructor]
		private ReceiveResponse()
		{
		}

		/// <summary>
		/// Forwarding fee
		/// </summary>
		[JsonProperty("fee_percent")]
		public int FeePercent { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("destination")]
		public string DestinationAddress { get; private set; }

		/// <summary>
		/// Input address where the funds should be sent
		/// </summary>
		[JsonProperty("input_address")]
		public string InputAddress { get; private set; }

		/// <summary>
		/// Callback URL that will be called upon payment
		/// </summary>
		[JsonProperty("callback_url")]
		public string CallbackUrl { get; private set; }
	}
}
