using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Info.Blockchain.API.Statistics
{
    /// <summary>
    /// This class is used as a response object to the 'get' method in the 'Statistics' class
    /// </summary>
    public class StatisticsResponse
    {
        public StatisticsResponse(JObject s)
        {
            TradeVolumeBTC = (double)s["trade_volume_btc"];
            TradeVolumeUSD = (double)s["trade_volume_usd"];
            MinersRevenueBTC = (double)s["miners_revenue_btc"];
            MinersRevenueUSD = (double)s["miners_revenue_usd"];
            MarketPriceUSD = (double)s["market_price_usd"];
            EstimatedTransactionVolumeUSD = (double)s["estimated_transaction_volume_usd"];
            TotalFeesBTC = (long)s["total_fees_btc"];
            TotalBTCSent = (long)s["total_btc_sent"];
            EstimatedBTCSent = (long)s["estimated_btc_sent"];
            BTCMined = (long)s["n_btc_mined"];
            Difficulty = (double)s["difficulty"];
            MinutesBetweenBlocks = (double)s["minutes_between_blocks"];
            DaysDestroyed = (double)s["days_destroyed"];
            NumberOfTransactions = (long)s["n_tx"];
            HashRate = (double)s["hash_rate"];
            Timestamp = (long)s["timestamp"];
            BlocksSize = (long)s["blocks_size"];
            TotalBTC = (long)s["totalbc"];
            TotalBlocks = (long)s["n_blocks_total"];
            NextRetarget = (long)s["nextretarget"];
        }

        /// <summary>
        /// Trade volume in the past 24 hours (in BTC)
        /// </summary>
        public double TradeVolumeBTC { get; private set; }

        /// <summary>
        /// Trade volume in the past 24 hours (in USD)
        /// </summary>
        public double TradeVolumeUSD { get; private set; }

        /// <summary>
        /// Miners' revenue in BTC
        /// </summary>
        public double MinersRevenueBTC { get; private set; }

        /// <summary>
        /// Miners' revenue in USD
        /// </summary>
        public double MinersRevenueUSD { get; private set; }

        /// <summary>
        /// Current market price in USD
        /// </summary>
        public double MarketPriceUSD { get; private set; }

        /// <summary>
        /// Estimated transaction volume in the past 24 hours
        /// </summary>
        public double EstimatedTransactionVolumeUSD { get; private set; }

        /// <summary>
        /// Total fees in the past 24 hours (in satoshi)
        /// </summary>
        public long TotalFeesBTC { get; private set; }

        /// <summary>
        /// Total BTC sent in the past 24 hours (in satoshi)
        /// </summary>
        public long TotalBTCSent { get; private set; }

        /// <summary>
        /// Estimated BTC sent in the past 24 hours (in satoshi)
        /// </summary>
        public long EstimatedBTCSent { get; private set; }

        /// <summary>
        /// Number of BTC mined in the past 24 hours (in satoshi)
        /// </summary>
        public long BTCMined { get; private set; }

        /// <summary>
        /// Current difficulty
        /// </summary>
        public double Difficulty { get; private set; }

        /// <summary>
        /// Minutes between blocks
        /// </summary>
        public double MinutesBetweenBlocks { get; private set; }

        /// <summary>
        /// Days destroyed in the past 24 hours
        /// </summary>
        public double DaysDestroyed { get; private set; }

        /// <summary>
        /// Number of transactions in the past 24 hours
        /// </summary>
        public long NumberOfTransactions { get; private set; }

        /// <summary>
        /// Current hashrate in GH/s
        /// </summary>
        public double HashRate { get; set; }

        /// <summary>
        /// Timestamp of when this report was compiled (in ms)
        /// </summary>
        public long Timestamp { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public long BlocksSize { get; private set; }

        /// <summary>
        /// Total BTC in existence (in satoshi)
        /// </summary>
        public long TotalBTC { get; private set; }

        /// <summary>
        /// Total number of blocks in existence
        /// </summary>
        public long TotalBlocks { get; private set; }

        /// <summary>
        /// The next block height where the difficulty retarget will occur
        /// </summary>
        public long NextRetarget { get; private set; }
    }
}
