using Info.Blockchain.API.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Info.Blockchain.API.BlockExplorer
{
	/// <summary>
	/// The BlockExplorer class reflects the functionality documented at 
	/// https://blockchain.info/api/blockchain_api. It can be used to query the block chain, 
	/// fetch block, transaction and address data, get unspent outputs for an address etc.
	/// </summary>
	public class BlockExplorer
	{
		private IHttpClient httpClient { get; }

		internal BlockExplorer(IHttpClient httpClient)
		{
			this.httpClient = httpClient;
		}
		
		/// <summary>
		///  Gets a single transaction based on a transaction index.
		/// </summary>
		/// <param name="txIndex">Transaction index</param>
		/// <returns>An instance of the Transaction class</returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<Transaction> GetTransaction(long index)
		{
			return await this.GetTransactionAsync(index.ToString());
		}

		/// <summary>
		///  Gets a single transaction based on a transaction hash.
		/// </summary>
		/// <param name="txHash">Transaction hash</param>
		/// <returns>An instance of the Transaction class</returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<Transaction> GetTransactionAsync(string hashOrIndex)
		{
			Transaction transaction = await this.httpClient.GetAsync<Transaction>("rawtx/" + hashOrIndex);
			return transaction;
		}

		/// <summary>
		/// Gets a single block based on a block index.
		/// </summary>
		/// <param name="blockIndex">Block index</param>
		/// <returns>An instance of the Block class</returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<Block> GetBlockAsync(long index)
		{
			return await this.GetBlockAsync(index.ToString());
		}

		/// <summary>
		/// Gets a single block based on a block hash.
		/// </summary>
		/// <param name="blockHash">Block hash</param>
		/// <returns>An instance of the Block class</returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<Block> GetBlockAsync(string hashOrIndex)
		{
			JObject blockJson = await this.httpClient.GetAsync<JObject>("rawblock/" + hashOrIndex);

			//Hack to add the missing block_height value into transactions
			foreach (JObject transaction in blockJson["tx"].AsJEnumerable())
			{
				transaction["block_height"] = blockJson["height"];
			}
			Block block = blockJson.ToObject<Block>();
			return block;
		}

		/// <summary>
		/// Gets data for a single address asynchronously.
		/// </summary>
		/// <param name="address">Base58check or hash160 address string</param>
		/// <param name="maxTransactionCount">Max amount of transactions to retrieve</param>
		/// <returns>An instance of the Address class</returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<Address> GetAddressAsync(string address, int? maxTransactionCount = null)
		{
			Address addressObj = await this.GetAddressWithOffsetAsync(address);
			List<Transaction> transactionList = await this.GetTransactions(addressObj, maxTransactionCount);
			return new Address(addressObj, transactionList);
		}

		private async Task<List<Transaction>> GetTransactions(Address address, long? maxTransactionCount = null)
		{
			const int maxTransactionLimit = 50;
			int transactionsPerRequest = maxTransactionCount == null || maxTransactionCount >= maxTransactionLimit ? maxTransactionLimit : (int)maxTransactionCount.Value;


			maxTransactionCount = maxTransactionCount ?? address.TransactionCount;
			List<Task<Address>> tasks = new List<Task<Address>>();

			for (int offset = 50; offset <= maxTransactionCount.Value; offset += 50)
			{
				Task<Address> task = this.GetAddressWithOffsetAsync(address.AddressStr, transactionsPerRequest, offset);
				tasks.Add(task);
			}

			await Task.WhenAll(tasks.ToArray());

			List<Transaction> transactions = address.Transactions.ToList();

			foreach (Task<Address> task in tasks)
			{
				transactions.AddRange(task.Result.Transactions);
			}

			return transactions;
		}

		private async Task<Address> GetAddressWithOffsetAsync(string address, int transactionLimit = 50, int offset = 0)
		{
			if (transactionLimit > 50 || transactionLimit < 1)
			{
				throw new ArgumentOutOfRangeException("Transaction limit can't be greater than 50 or less than 1.", nameof(transactionLimit));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("Offset can't be less than 0.", nameof(offset));
			}

			QueryString queryString = new QueryString();
			queryString.Add("offset", offset.ToString());
			queryString.Add("limit", transactionLimit.ToString());

			Address addressObj = await this.httpClient.GetAsync<Address>("rawaddr/" + address, queryString);
			return addressObj;
		}

		/// <summary>
		/// Gets a list of blocks at the specified height. Normally, only one block will be returned, 
		/// but in case of a chain fork, multiple blocks may be present.
		/// </summary>
		/// <param name="height">Block height</param>
		/// <returns>A list of blocks at the specified height</returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<ReadOnlyCollection<Block>> GetBlocksAtHeightAsync(long height)
		{
			QueryString queryString = new QueryString();
			queryString.Add("format", "json");

			List<Block> blocks = await this.httpClient.GetAsync<List<Block>>("block-height/" + height, queryString);
			return new ReadOnlyCollection<Block>(blocks);
		}

		/// <summary>
		/// Gets unspent outputs for a single address.
		/// </summary>
		/// <param name="address">Base58check or hash160 address string</param>
		/// <returns>A list of unspent outputs for the specified address </returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<ReadOnlyCollection<UnspentOutput>> GetUnspentOutputsAsync(string address)
		{
			QueryString queryString = new QueryString();
			queryString.Add("active", address);
			try
			{
				List<UnspentOutput> unspentOuputs = await this.httpClient.GetAsync<List<UnspentOutput>>("unspent", queryString);
				return new ReadOnlyCollection<UnspentOutput>(unspentOuputs);
			}
			catch (ServerApiException ex)
			{
				// the API isn't supposed to return an error code here. No free outputs is
				// a legitimate situation. We are circumventing that by returning an empty list
				if (ex.Message == "No free outputs to spend")
				{
					return new ReadOnlyCollection<UnspentOutput>(new List<UnspentOutput>());
				}
				throw;
			}
		}

		/// <summary>
		/// Gets the latest block on the main chain (simplified representation).
		/// </summary>
		/// <returns>An instance of the LatestBlock class</returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<LatestBlock> GetLatestBlockAsync()
		{
			LatestBlock latestBlock = await this.httpClient.GetAsync<LatestBlock>("latestblock");
			return latestBlock;
		}

		/// <summary>
		/// Gets a list of currently unconfirmed transactions.
		/// </summary>
		/// <returns>A list of unconfirmed Transaction objects</returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<ReadOnlyCollection<Transaction>> GetUnconfirmedTransactionsAsync()
		{
			QueryString queryString = new QueryString();
			queryString.Add("format"," json");

			List<Transaction> transactions = await this.httpClient.GetAsync<List<Transaction>>("unconfirmed-transactions", queryString);
			return new ReadOnlyCollection<Transaction>(transactions);
		}

		/// <summary>
		/// Gets a list of blocks mined today by all pools since 00:00 UTC.
		/// </summary>
		/// <returns>A list of SimpleBlock objects</returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<ReadOnlyCollection<SimpleBlock>> GetBlocksAsync()
		{
			return await this.GetBlocksAsync(null);
		}

		/// <summary>
		/// Gets a list of blocks mined on a specific day.
		/// </summary>
		/// <param name="timestamp">Unix timestamp (without milliseconds) that falls 
		/// between 00:00 UTC and 23:59 UTC of the desired day.</param>
		/// <returns>A list of SimpleBlock objects</returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<ReadOnlyCollection<SimpleBlock>> GetBlocksAsync(long timestamp)
		{
			string timestampString = (timestamp * 1000).ToString();
            return await this.GetBlocksAsync(timestampString);
		}

		/// <summary>
		/// Gets a list of recent blocks by a specific mining pool.
		/// </summary>
		/// <param name="poolName">Name of the mining pool</param>
		/// <returns>A list of SimpleBlock objects</returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<ReadOnlyCollection<SimpleBlock>> GetBlocksAsync(string poolNameOrTimestamp)
		{
			QueryString queryString = new QueryString();
			queryString.Add("format", "json");

			List<SimpleBlock> simpleBlocks = await this.httpClient.GetAsync<List<SimpleBlock>>("blocks/" + poolNameOrTimestamp, queryString);
			
			return new ReadOnlyCollection<SimpleBlock>(simpleBlocks);
		}

		/// <summary>
		/// Gets inventory data for an object.
		/// </summary>
		/// <param name="hash">Object hash</param>
		/// <returns>An instance of the InventoryData class</returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<InventoryData> GetInventoryDataAsync(string hash)
		{
			QueryString queryString = new QueryString();
			queryString.Add("format", "json");

			InventoryData invertoryData = await this.httpClient.GetAsync<InventoryData>("inv/" + hash, queryString);
			return invertoryData;
		}
	}
}
