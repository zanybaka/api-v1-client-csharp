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
		private string apiCode;

		public BlockExplorer(string apiCode = null)
		{
			this.apiCode = apiCode;
		}

		/// <summary>
		///  Gets a single transaction based on a transaction index.
		/// </summary>
		/// <param name="txIndex">Transaction index</param>
		/// <returns>An instance of the Transaction class</returns>
		/// <exception cref="APIException">If the server returns an error</exception>
		public Transaction GetTransaction(long txIndex)
		{
			return GetTransaction(txIndex.ToString());
		}

		/// <summary>
		///  Gets a single transaction based on a transaction hash.
		/// </summary>
		/// <param name="txHash">Transaction hash</param>
		/// <returns>An instance of the Transaction class</returns>
		/// <exception cref="APIException">If the server returns an error</exception>
		public Transaction GetTransaction(string txHash)
		{
			var req = new NameValueCollection();
			if (apiCode != null)
			{
				req["api_code"] = apiCode;
			}
			string response = HttpClientUtil.Get("rawtx/" + txHash, req);
			Transaction transaction = JsonConvert.DeserializeObject<Transaction>(response);
			return transaction;
		}

		/// <summary>
		/// Gets a single block based on a block index.
		/// </summary>
		/// <param name="blockIndex">Block index</param>
		/// <returns>An instance of the Block class</returns>
		/// <exception cref="APIException">If the server returns an error</exception>
		public async Task<Block> GetBlockAsync(long blockIndex)
		{
			return await GetBlockAsync(blockIndex.ToString());
		}

		/// <summary>
		/// Gets a single block based on a block hash.
		/// </summary>
		/// <param name="blockHash">Block hash</param>
		/// <returns>An instance of the Block class</returns>
		/// <exception cref="APIException">If the server returns an error</exception>
		public async Task<Block> GetBlockAsync(string blockHash)
		{
			var req = new NameValueCollection();
			if (apiCode != null)
			{
				req["api_code"] = apiCode;
			}

			string response = await HttpClientUtil.GetAsync("rawblock/" + blockHash, req);

			//Hack to add the missing block_height value into transactions
			JObject blockJson = JObject.Parse(response);
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
		/// <exception cref="APIException">If the server returns an error</exception>
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

			var req = new NameValueCollection();
			if (apiCode != null)
			{
				req["api_code"] = apiCode;
			}
			req["offset"] = offset.ToString();
			req["limit"] = transactionLimit.ToString();

			string response = await HttpClientUtil.GetAsync("rawaddr/" + address, req);
			Address addressObj = JsonConvert.DeserializeObject<Address>(response);
			return addressObj;
		}

		/// <summary>
		/// Gets a list of blocks at the specified height. Normally, only one block will be returned, 
		/// but in case of a chain fork, multiple blocks may be present.
		/// </summary>
		/// <param name="height">Block height</param>
		/// <returns>A list of blocks at the specified height</returns>
		/// <exception cref="APIException">If the server returns an error</exception>
		public ReadOnlyCollection<Block> GetBlocksAtHeight(long height)
		{
			var req = new NameValueCollection();
			req["format"] = "json";
			if (apiCode != null)
			{
				req["api_code"] = apiCode;
			}

			string response = HttpClientUtil.Get("block-height/" + height, req);
			List<Block> blocks = JsonConvert.DeserializeObject<List<Block>>(response);
			return new ReadOnlyCollection<Block>(blocks);
		}

		/// <summary>
		/// Gets unspent outputs for a single address.
		/// </summary>
		/// <param name="address">Base58check or hash160 address string</param>
		/// <returns>A list of unspent outputs for the specified address </returns>
		/// <exception cref="APIException">If the server returns an error</exception>
		public ReadOnlyCollection<UnspentOutput> GetUnspentOutputs(string address)
		{
			var req = new NameValueCollection();
			req["active"] = address;
			if (apiCode != null)
			{
				req["api_code"] = apiCode;
			}

			string response = null;

			try
			{
				response = HttpClientUtil.Get("unspent", req);
			}
			catch (APIException e)
			{
				// the API isn't supposed to return an error code here. No free outputs is
				// a legitimate situation. We are circumventing that by returning an empty list
				if (e.Message == "No free outputs to spend")
				{
					return new ReadOnlyCollection<UnspentOutput>(new List<UnspentOutput>());
				}
				else
				{
					throw e;
				}
			}

			List<UnspentOutput> outputs = JsonConvert.DeserializeObject<List<UnspentOutput>>(response);
			return new ReadOnlyCollection<UnspentOutput>(outputs);
		}

		/// <summary>
		/// Gets the latest block on the main chain (simplified representation).
		/// </summary>
		/// <returns>An instance of the LatestBlock class</returns>
		/// <exception cref="APIException">If the server returns an error</exception>
		public LatestBlock GetLatestBlock()
		{
			var req = new NameValueCollection();
			if (apiCode != null)
			{
				req["api_code"] = apiCode;
			}

			string response = HttpClientUtil.Get("latestblock", req);
			LatestBlock latestBlock = JsonConvert.DeserializeObject<LatestBlock>(response);
			return latestBlock;
		}

		/// <summary>
		/// Gets a list of currently unconfirmed transactions.
		/// </summary>
		/// <returns>A list of unconfirmed Transaction objects</returns>
		/// <exception cref="APIException">If the server returns an error</exception>
		public ReadOnlyCollection<Transaction> GetUnconfirmedTransactions()
		{
			var req = new NameValueCollection();
			req["format"] = "json";
			if (apiCode != null)
			{
				req["api_code"] = apiCode;
			}

			string response = HttpClientUtil.Get("unconfirmed-transactions", req);

			List<Transaction> transactions = JsonConvert.DeserializeObject<List<Transaction>>(response);

			if (transactions.Any(x => x.BlockHeight != -1))
			{
				throw new NotImplementedException(); //TODO default to -1
			}
			return new ReadOnlyCollection<Transaction>(transactions);
		}

		/// <summary>
		/// Gets a list of blocks mined today by all pools since 00:00 UTC.
		/// </summary>
		/// <returns>A list of SimpleBlock objects</returns>
		/// <exception cref="APIException">If the server returns an error</exception>
		public ReadOnlyCollection<SimpleBlock> GetBlocks()
		{
			return GetBlocks(null);
		}

		/// <summary>
		/// Gets a list of blocks mined on a specific day.
		/// </summary>
		/// <param name="timestamp">Unix timestamp (without milliseconds) that falls 
		/// between 00:00 UTC and 23:59 UTC of the desired day.</param>
		/// <returns>A list of SimpleBlock objects</returns>
		/// <exception cref="APIException">If the server returns an error</exception>
		public ReadOnlyCollection<SimpleBlock> GetBlocks(long timestamp)
		{
			return GetBlocks((timestamp * 1000).ToString());
		}

		/// <summary>
		/// Gets a list of recent blocks by a specific mining pool.
		/// </summary>
		/// <param name="poolName">Name of the mining pool</param>
		/// <returns>A list of SimpleBlock objects</returns>
		/// <exception cref="APIException">If the server returns an error</exception>
		public ReadOnlyCollection<SimpleBlock> GetBlocks(string poolName)
		{
			var req = new NameValueCollection();
			req["format"] = "json";
			if (apiCode != null)
			{
				req["api_code"] = apiCode;
			}

			poolName = poolName == null ? null : poolName;

			string response = HttpClientUtil.Get("blocks/" + poolName, req);

			List<SimpleBlock> simpleBlocks = JsonConvert.DeserializeObject<List<SimpleBlock>>(response);
			return new ReadOnlyCollection<SimpleBlock>(simpleBlocks);
		}

		/// <summary>
		/// Gets inventory data for an object.
		/// </summary>
		/// <param name="hash">Object hash</param>
		/// <returns>An instance of the InventoryData class</returns>
		/// <exception cref="APIException">If the server returns an error</exception>
		public InventoryData GetInventoryData(string hash)
		{
			var req = new NameValueCollection();
			req["format"] = "json";
			if (apiCode != null)
			{
				req["api_code"] = apiCode;
			}

			string response = HttpClientUtil.Get("inv/" + hash, req);
			InventoryData invertoryData = JsonConvert.DeserializeObject<InventoryData>(response);
			return invertoryData;
		}
	}
}
