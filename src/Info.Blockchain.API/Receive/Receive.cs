using Newtonsoft.Json;

namespace Info.Blockchain.API.Receive
{
	/// <summary>
	/// This class reflects the functionality documented at at https://blockchain.info/api/api_receive. 
	/// It allows merchants to create forwarding addresses and be notified upon payment.
	/// </summary>
	public class Receive
	{
		/// <summary>
		/// Calls the 'api/receive' endpoint and creates a forwarding address.
		/// </summary>
		/// <param name="receivingAddress">Destination address where the payment should be sent</param>
		/// <param name="callbackUrl">Callback URI that will be called upon payment</param>
		/// <param name="apiCode">Blockchain.info API code</param>
		/// <returns>An instance of the ReceiveResponse class</returns>
		/// <exception cref="APIException">If the server returns an error</exception>
		public static ReceiveResponse ReceiveFunds(string receivingAddress, string callbackUrl, string apiCode = null)
		{
			var req = new NameValueCollection();
			req["address"] = receivingAddress;
			req["callback"] = callbackUrl;
			req["method"] = "create";

			if (apiCode != null)
				req["api_code"] = apiCode;

			string response = HttpClientUtil.Post("api/receive", req);
			ReceiveResponse receiveResponse = JsonConvert.DeserializeObject<ReceiveResponse>(response);
			return receiveResponse;
		}
	}
}
