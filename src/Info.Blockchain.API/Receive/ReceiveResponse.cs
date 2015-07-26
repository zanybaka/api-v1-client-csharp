using Newtonsoft.Json;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Info.Blockchain.API.Receive
{
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
		[JsonProperty("fee_percent", Required = Required.Always)]
		public int FeePercent { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("destination", Required = Required.Always)]
		public string DestinationAddress { get; private set; }

		/// <summary>
		/// Input address where the funds should be sent
		/// </summary>
		[JsonProperty("input_address", Required = Required.Always)]
		public string InputAddress { get; private set; }

		/// <summary>
		/// Callback URL that will be called upon payment
		/// </summary>
		[JsonProperty("callback_url", Required = Required.Always)]
		public string CallbackUrl { get; private set; }
	}
}
