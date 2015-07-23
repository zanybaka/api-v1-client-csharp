using Info.Blockchain.API.BlockExplorer;
using Info.Blockchain.API.Json;
using Newtonsoft.Json;

namespace Info.Blockchain.API.Statistics
{
	/// <summary>
	/// This class is used as a response object to the 'get' method in the 'Statistics' class
	/// </summary>
	public class StatisticsResponse
	{
		[JsonConstructor]
		private StatisticsResponse()
		{
		}

		/// <summary>
		/// Trade volume in the past 24 hours (in BTC)
		/// </summary>
		[JsonProperty("trade_volume_btc")]
		public double TradeVolumeBTC { get; private set; }

		/// <summary>
		/// Trade volume in the past 24 hours (in USD)
		/// </summary>
		[JsonProperty("trade_volume_usd")]
		public double TradeVolumeUSD { get; private set; }

		/// <summary>
		/// Miners' revenue in BTC
		/// </summary>
		[JsonProperty("miners_revenue_btc")]
		public double MinersRevenueBTC { get; private set; }

		/// <summary>
		/// Miners' revenue in USD
		/// </summary>
		[JsonProperty("miners_revenue_usd")]
		public double MinersRevenueUSD { get; private set; }

		/// <summary>
		/// Current market price in USD
		/// </summary>
		[JsonProperty("market_price_usd")]
		public double MarketPriceUSD { get; private set; }

		/// <summary>
		/// Estimated transaction volume in the past 24 hours
		/// </summary>
		[JsonProperty("estimated_transaction_volume_usd")]
		public double EstimatedTransactionVolumeUSD { get; private set; }

		/// <summary>
		/// Total fees in the past 24 hours
		/// </summary>
		[JsonProperty("total_fees_btc")]
		[JsonConverter(typeof(BitcoinValueJsonConverter))]
		public BitcoinValue TotalFeesBTC { get; private set; }

		/// <summary>
		/// Total BTC sent in the past 24 hours
		/// </summary>
		[JsonProperty("total_btc_sent")]
		[JsonConverter(typeof(BitcoinValueJsonConverter))]
		public BitcoinValue TotalBTCSent { get; private set; }

		/// <summary>
		/// Estimated BTC sent in the past 24 hours
		/// </summary>
		[JsonProperty("estimated_btc_sent")]
		[JsonConverter(typeof(BitcoinValueJsonConverter))]
		public BitcoinValue EstimatedBTCSent { get; private set; }

		/// <summary>
		/// Number of BTC mined in the past 24 hours
		/// </summary>
		[JsonProperty("n_btc_mined")]
		[JsonConverter(typeof(BitcoinValueJsonConverter))]
		public BitcoinValue BTCMined { get; private set; }

		/// <summary>
		/// Current difficulty
		/// </summary>
		[JsonProperty("difficulty")]
		public double Difficulty { get; private set; }

		/// <summary>
		/// Minutes between blocks
		/// </summary>
		[JsonProperty("minutes_between_blocks")]
		public double MinutesBetweenBlocks { get; private set; }

		/// <summary>
		/// Number of transactions in the past 24 hours
		/// </summary>
		[JsonProperty("n_tx")]
		public long NumberOfTransactions { get; private set; }

		/// <summary>
		/// Current hashrate in GH/s
		/// </summary>
		[JsonProperty("hash_rate")]
		public double HashRate { get; set; }

		/// <summary>
		/// Timestamp of when this report was compiled (in ms)
		/// </summary>
		[JsonProperty("timestamp")]
		public long Timestamp { get; private set; }

		/// <summary>
		/// Number of blocks mined in the past 24 hours
		/// </summary>
		[JsonProperty("n_blocks_mined")]
		public long MinedBlocks { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("blocks_size")]
		public long BlocksSize { get; private set; }

		/// <summary>
		/// Total BTC in existence
		/// </summary>
		[JsonProperty("totalbc")]
		[JsonConverter(typeof(BitcoinValueJsonConverter))]
		public BitcoinValue TotalBTC { get; private set; }

		/// <summary>
		/// Total number of blocks in existence
		/// </summary>
		[JsonProperty("n_blocks_total")]
		public long TotalBlocks { get; private set; }

		/// <summary>
		/// The next block height where the difficulty retarget will occur
		/// </summary>
		[JsonProperty("nextretarget")]
		public long NextRetarget { get; private set; }
	}
}
