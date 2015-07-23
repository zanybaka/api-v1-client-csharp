namespace Info.Blockchain.API.PushTx
{
	/// <summary>
	/// This class reflects the functionality provided at https://blockchain.info/pushtx. 
	/// It allows users to broadcast hex encoded transactions to the bitcoin network.
	/// </summary>
	public class PushTx
	{
		/// <summary>
		/// Pushes a hex encoded transaction to the network.
		/// </summary>
		/// <param name="tx">Hex encoded transaction</param>
		/// <param name="apiCode">Blockchain.info API code</param>
		/// <exception cref="APIException">If the server returns an error</exception>
		public static void PushTransaction(string tx, string apiCode = null)
		{
			var req = new NameValueCollection();
			req["tx"] = tx;

			if (apiCode != null)
				req["api_code"] = apiCode;

			HttpClientUtil.Post("pushtx", req);
		}
	}
}
