using Info.Blockchain.API.BlockExplorer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Info.Blockchain.API.Wallet
{
	/// <summary>
	/// This class reflects the functionality documented
	/// at https://blockchain.info/api/blockchain_wallet_api. It allows users to interact
	/// with their Blockchain.info wallet.
	/// </summary>
	public class Wallet
	{
		private string identifier;
		private string password;
		private string secondPassword;
		private string apiCode;

		/// <summary>
		/// </summary>
		/// <param name="identifier">Wallet identifier (GUID)</param>
		/// <param name="password">Decryption password</param>
		/// <param name="secondPassword">Second password</param>
		/// <param name="apiCode">Blockchain.info API code</param>
		public Wallet(string identifier, string password,
			string secondPassword = null, string apiCode = null)
		{
			this.identifier = identifier;
			this.password = password;
			this.secondPassword = secondPassword;
			this.apiCode = apiCode;
		}

		/// <summary>
		/// Sends bitcoin from your wallet to a single address.
		/// </summary>
		/// <param name="toAddress">Recipient bitcoin address</param>
		/// <param name="amount">Amount to send</param>
		/// <param name="fromAddress">Specific address to send from</param>
		/// <param name="fee">Transaction fee. Must be greater than the default fee</param>
		/// <param name="note">Public note to include with the transaction</param>
		/// <returns>An instance of the PaymentResponse class</returns>
		/// <exception cref="APIException">If the server returns an error</exception>
		public PaymentResponse Send(string toAddress, BitcoinValue amount,
			string fromAddress = null, BitcoinValue? fee = null, string note = null)
		{
			var recipients = new Dictionary<string, BitcoinValue>();
			recipients[toAddress] = amount;

			return SendMany(recipients, fromAddress, fee, note);
		}

		/// <summary>
		/// Sends bitcoin from your wallet to multiple addresses.
		/// </summary>
		/// <param name="recipients">Dictionary with the structure of 'address':amount 
		/// (string:BitcoinValue)</param>
		/// <param name="fromAddress">Specific address to send from</param>
		/// <param name="fee">Transaction fee. Must be greater than the default fee</param>
		/// <param name="note">Public note to include with the transaction</param>
		/// <returns>An instance of the PaymentResponse class</returns>
		/// <exception cref="APIException">If the server returns an error</exception>
		public PaymentResponse SendMany(Dictionary<string, BitcoinValue> recipients,
			string fromAddress = null, BitcoinValue? fee = null, string note = null)
		{
			var req = BuildBasicRequest();
			string method = null;

			if (recipients.Count == 1)
			{
				method = "payment";
				req["to"] = recipients.First().Key;
				req["amount"] = recipients.First().Value.ToString();
			}
			else
			{
				method = "sendmany";
				req["recipients"] = Newtonsoft.Json.JsonConvert.SerializeObject(recipients);
			}

			if (fromAddress != null)
				req["from"] = fromAddress;
			if (fee != null)
				req["fee"] = fee.ToString();
			if (note != null)
				req["note"] = note;

			string response = HttpClientUtil.Get(string.Format("merchant/{0}/{1}",
				identifier, method), req);
			PaymentResponse paymentResponse = JsonConvert.DeserializeObject<PaymentResponse>(response);

			return paymentResponse;
		}

		/// <summary>
		/// Fetches the wallet balance. Includes unconfirmed transactions 
		/// and possibly double spends.
		/// </summary>
		/// <returns>Wallet balance</returns>
		/// <exception cref="Info.Blockchain.API.APIException">If the server returns an error</exception>
		public BitcoinValue GetBalance()
		{
			string response = HttpClientUtil.Get(string.Format("merchant/{0}/balance", identifier),
				BuildBasicRequest());
			JObject topElem = ParseResponse(response);

			return BitcoinValue.FromSatoshis(Convert.ToInt64(topElem.Property("balance").Value));
		}

		/// <summary>
		/// Lists all active addresses in the wallet.
		/// </summary>
		/// <param name="confirmations">Minimum number of confirmations transactions 
		/// must have before being included in the balance of addresses (can be 0)</param>
		/// <returns>A list of Address objects</returns>
		/// <exception cref="Info.Blockchain.API.APIException">If the server returns an error</exception>
		public List<Address> ListAddresses(int confirmations = 0)
		{
			var req = BuildBasicRequest();
			req["confirmations"] = confirmations.ToString();

			string response = HttpClientUtil.Get(string.Format("merchant/{0}/list",
				identifier), req);
			List<Address> addressList = JsonConvert.DeserializeObject<List<Address>>(response);
			
			return addressList;
		}

		/// <summary>
		/// Retrieves an address from the wallet.
		/// </summary>
		/// <param name="address"> Address in the wallet to look up</param>
		/// <param name="confirmations">Minimum number of confirmations transactions 
		/// must have before being included in the balance of addresses (can be 0)</param>
		/// <returns>An instance of the Address class</returns>
		/// <exception cref="Info.Blockchain.API.APIException">If the server returns an error</exception>
		public Address GetAddress(string address, int confirmations = 0)
		{
			var req = BuildBasicRequest();
			req["confirmations"] = confirmations.ToString();
			req["address"] = address;

			string response = HttpClientUtil.Get(string.Format("merchant/{0}/address_balance",
				identifier), req);
			Address addressObj = JsonConvert.DeserializeObject<Address>(response);

			return addressObj;
		}

		/// <summary>
		/// Generates a new address and adds it to the wallet.
		/// </summary>
		/// <param name="label">Label to attach to this address</param>
		/// <returns>An instance of the Address class</returns>
		/// <exception cref="Info.Blockchain.API.APIException">If the server returns an error</exception>
		public Address NewAddress(string label = null)
		{
			var req = BuildBasicRequest();
			if (label != null)
				req["label"] = label;

			string response = HttpClientUtil.Get(string.Format("merchant/{0}/new_address",
				identifier), req);
			Address addressObj = JsonConvert.DeserializeObject<Address>(response);

			return addressObj;
		}

		/// <summary>
		/// Archives an address.
		/// </summary>
		/// <param name="address">Address to archive</param>
		/// <returns>String representation of the archived address</returns>
		/// <exception cref="Info.Blockchain.API.APIException">If the server returns an error</exception>
		public string ArchiveAddress(string address)
		{
			var req = BuildBasicRequest();
			req["address"] = address;

			string response = HttpClientUtil.Get(string.Format("merchant/{0}/archive_address",
			   identifier), req);
			JObject topElem = ParseResponse(response);

			return (string)topElem["archived"];
		}

		/// <summary>
		/// Unarchives an address.
		/// </summary>
		/// <param name="address">Address to unarchive</param>
		/// <returns>String representation of the unarchived address</returns>
		/// <exception cref="Info.Blockchain.API.APIException">If the server returns an error</exception>
		public string UnarchiveAddress(string address)
		{
			var req = BuildBasicRequest();
			req["address"] = address;

			string response = HttpClientUtil.Get(string.Format("merchant/{0}/unarchive_address",
			   identifier), req);
			JObject topElem = ParseResponse(response);

			return (string)topElem["active"];
		}

		/// <summary>
		/// Consolidates the wallet addresses.
		/// </summary>
		/// <param name="days">Addresses which have not received any
		/// transactions in at least this many days will be consolidated.</param>
		/// <returns>A list of consolidated addresses in the string format</returns>
		/// <exception cref="Info.Blockchain.API.APIException">If the server returns an error</exception>
		public List<string> Consolidate(int days = 0)
		{
			var req = BuildBasicRequest();
			req["days"] = days.ToString();

			string response = HttpClientUtil.Get(string.Format("merchant/{0}/auto_consolidate",
				identifier), req);
			JObject topElem = ParseResponse(response);

			return topElem["consolidated"].Select(x => (string)x).ToList();
		}

		private JObject ParseResponse(string response)
		{
			JObject topElem = JObject.Parse(response);
			var error = topElem["error"];
			if (error != null)
				throw new APIException((string)error);

			return topElem;
		}

		private NameValueCollection BuildBasicRequest()
		{
			var req = new NameValueCollection();

			req["password"] = password;
			if (apiCode != null)
				req["api_code"] = apiCode;
			if (secondPassword != null)
				req["second_password"] = secondPassword;

			return req;
		}
	}
}
