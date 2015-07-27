using System;
using System.Threading.Tasks;
using Info.Blockchain.API.Abstractions;

namespace Info.Blockchain.API.Receive
{
	/// <summary>
	/// This class reflects the functionality documented at at https://blockchain.info/api/api_receive. 
	/// It allows merchants to create forwarding addresses and be notified upon payment.
	/// </summary>
	public class FundReceiver
	{
		private IHttpClient httpClient { get; }
		internal FundReceiver(IHttpClient httpClient)
		{
			this.httpClient = httpClient;
		}

		/// <summary>
		/// Calls the 'api/receive' endpoint and creates a forwarding address.
		/// </summary>
		/// <param name="receivingAddress">Destination address where the payment should be sent</param>
		/// <param name="callbackUrl">Callback URI that will be called upon payment</param>
		/// <returns>An instance of the ReceiveResponse class</returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<ReceiveResponse> ReceiveFundsAsync(string receivingAddress, string callbackUrl)
		{
			if (string.IsNullOrWhiteSpace(receivingAddress))
			{
				throw new ArgumentNullException(nameof(receivingAddress));
			}
			if (string.IsNullOrWhiteSpace(callbackUrl))
			{
				throw new ArgumentNullException(nameof(callbackUrl));
			}
			QueryString queryString = new QueryString();
			queryString.Add("method", "create");
			queryString.Add("address", receivingAddress);
			queryString.Add("callback", callbackUrl);
			ReceiveResponse receiveResponse = await this.httpClient.GetAsync<ReceiveResponse>("api/receive", queryString);
			return receiveResponse;
		}
	}
}
