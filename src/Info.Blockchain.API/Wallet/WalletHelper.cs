using Info.Blockchain.API.Abstractions;
using Info.Blockchain.API.BlockExplorer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Info.Blockchain.API.Wallet
{
	/// <summary>
	/// This class reflects the functionality documented
	/// at https://blockchain.info/api/blockchain_wallet_api. It allows users to interact
	/// with their Blockchain.info wallet.
	/// </summary>
	public class WalletHelper
	{
		private IHttpClient httpClient { get; }

		private string identifier { get; }
		private string password { get; }
		private string secondPassword { get; }

		/// <summary>
		/// </summary>
		/// <param name="identifier">Wallet identifier (GUID)</param>
		/// <param name="password">Decryption password</param>
		/// <param name="secondPassword">Second password</param>
		internal WalletHelper(IHttpClient httpClient, string identifier, string password, string secondPassword = null)
		{
			this.httpClient = httpClient;
			this.identifier = identifier;
			this.password = password;
			this.secondPassword = secondPassword;
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
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<PaymentResponse> SendAsync(string toAddress, BitcoinValue amount,
			string fromAddress = null, BitcoinValue? fee = null, string note = null)
		{
			if (string.IsNullOrWhiteSpace(toAddress))
			{
				throw new ArgumentNullException(nameof(toAddress));
			}
			if (amount.Btc <= 0)
			{
				throw new ArgumentException("Amount sent must be greater than 0", nameof(amount));
			}

			SinglePaymentRequest paymentRequest = new SinglePaymentRequest(this.password, this.secondPassword, toAddress, amount.Satoshis, fromAddress, fee, note);

			string route = string.Format("merchant/{0}/payment", this.identifier);

			PaymentResponse paymentResponse = await this.httpClient.PostAsync<SinglePaymentRequest, PaymentResponse>(route, paymentRequest);
			return paymentResponse;
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
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<PaymentResponse> SendManyAsync(Dictionary<string, BitcoinValue> recipients,
			string fromAddress = null, BitcoinValue? fee = null, string note = null)
		{
			if (recipients == null || !recipients.Any())
			{
				throw new ArgumentException("Sending bitcoin from your wallet requires at least one receipient.", nameof(recipients));
			}

			ManyPaymentRequest paymentRequest = new ManyPaymentRequest(this.password, this.secondPassword, recipients, fromAddress, fee, note);


			string route = string.Format("merchant/{0}/sendmany", this.identifier);

			PaymentResponse paymentResponse = await this.httpClient.PostAsync<ManyPaymentRequest, PaymentResponse>(route, paymentRequest);

			return paymentResponse;
		}

		/// <summary>
		/// Fetches the wallet balance. Includes unconfirmed transactions 
		/// and possibly double spends.
		/// </summary>
		/// <returns>Wallet balance</returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<BitcoinValue> GetBalanceAsync()
		{
			QueryString queryString = this.BuildBasicQueryString();
			string route = $"merchant/{this.identifier}/balance";
            BitcoinValue bitcoinValue = await this.httpClient.GetAsync<BitcoinValue>(route, queryString);
			return bitcoinValue;
		}

		/// <summary>
		/// Lists all active addresses in the wallet.
		/// </summary>
		/// <param name="confirmations">Minimum number of confirmations transactions 
		/// must have before being included in the balance of addresses (can be 0)</param>
		/// <returns>A list of Address objects</returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<List<Address>> ListAddressesAsync(int confirmations = 0)
		{
			QueryString queryString = this.BuildBasicQueryString();
			queryString.Add("confirmations", confirmations.ToString());

			string route = $"merchant/{this.identifier}/list";

			List<Address> addressList = await this.httpClient.GetAsync<List<Address>>(route, queryString);
			return addressList;
		}

		/// <summary>
		/// Retrieves an address from the wallet.
		/// </summary>
		/// <param name="address"> Address in the wallet to look up</param>
		/// <param name="confirmations">Minimum number of confirmations transactions 
		/// must have before being included in the balance of addresses (can be 0)</param>
		/// <returns>An instance of the Address class</returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<Address> GetAddressAsync(string address, int confirmations = 0)
		{
			QueryString queryString = this.BuildBasicQueryString();
			queryString.Add("confirmations", confirmations.ToString());
			queryString.Add("address", address);

			string route = $"merchant/{this.identifier}/address_balance";
			Address addressObj = await this.httpClient.GetAsync<Address>(route, queryString);
			return addressObj;
		}

		/// <summary>
		/// Generates a new address and adds it to the wallet.
		/// </summary>
		/// <param name="label">Label to attach to this address</param>
		/// <returns>An instance of the Address class</returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<Address> NewAddress(string label = null)
		{
			QueryString queryString = this.BuildBasicQueryString();
			if (label != null)
			{
				queryString.Add("label", label);
			}
			string route = $"merchant/{this.identifier}/new_address";
			Address addressObj = await this.httpClient.GetAsync<Address>(route, queryString);
			return addressObj;
		}

		/// <summary>
		/// Archives an address.
		/// </summary>
		/// <param name="address">Address to archive</param>
		/// <returns>String representation of the archived address</returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<string> ArchiveAddress(string address)
		{
			QueryString queryString = this.BuildBasicQueryString();
			queryString.Add("address", address);

			string route = $"merchant/{this.identifier}/archive_address";
            string archiveAddress = await this.httpClient.GetAsync<string>(route, queryString);

			return archiveAddress;
		}

		/// <summary>
		/// Unarchives an address.
		/// </summary>
		/// <param name="address">Address to unarchive</param>
		/// <returns>String representation of the unarchived address</returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<string> UnarchiveAddress(string address)
		{
			QueryString queryString = this.BuildBasicQueryString();
			queryString.Add("address", address);

			string route = $"merchant/{this.identifier}/unarchive_address";
            string activeAddress = await this.httpClient.GetAsync<string>(route, queryString);
			return activeAddress;
		}

		/// <summary>
		/// Consolidates the wallet addresses.
		/// </summary>
		/// <param name="days">Addresses which have not received any
		/// transactions in at least this many days will be consolidated.</param>
		/// <returns>A list of consolidated addresses in the string format</returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<List<string>> Consolidate(int days = 0)
		{
			QueryString queryString = this.BuildBasicQueryString();
			queryString.Add("days", days.ToString());

			string route = $"merchant/{this.identifier}/auto_consolidate";
            List<string> addresses = await this.httpClient.GetAsync<List<string>>(route, queryString);
			return addresses;
		}

		private QueryString BuildBasicQueryString()
		{
			QueryString queryString = new QueryString();

			queryString.Add("password", this.password);
			if (this.secondPassword != null)
			{
				queryString.Add("second_password", this.secondPassword);
			}

			return queryString;
		}
	}
}
