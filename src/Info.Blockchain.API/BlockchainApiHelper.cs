using System;
using Info.Blockchain.API.Abstractions;
using Info.Blockchain.API.CreateWallet;
using Info.Blockchain.API.ExchangeRates;
using Info.Blockchain.API.PushTx;
using Info.Blockchain.API.Receive;
using Info.Blockchain.API.Statistics;
using Info.Blockchain.API.Wallet;

namespace Info.Blockchain.API
{
	public class BlockchainApiHelper : IDisposable
	{
        private IHttpClient baseHttpClient { get; }
        private IHttpClient serviceHttpClient { get; }

        public BlockExplorer.BlockExplorer BlockExpolorer { get; }
		public WalletCreator WalletCreator { get; }
		public ExchangeRateExplorer ExchangeRateExplorer { get; }
		public TransactionPusher TransactionPusher { get; }
		public FundReceiver FundReceiver { get; }
		public StatisticsExplorer StatisticsExplorer { get; }


		public BlockchainApiHelper(string serviceUrl, string apiCode = null, IHttpClient baseHttpClient = null, IHttpClient serviceHttpClient = null)
		{
			if (baseHttpClient == null)
			{
				this.baseHttpClient = new BlockchainHttpClient(apiCode);
			} else {
				this.baseHttpClient = baseHttpClient;
				if (apiCode != null)
				{
					this.baseHttpClient.ApiCode = apiCode;
				}
			}

            if (serviceHttpClient == null)
            {
                this.serviceHttpClient = new BlockchainHttpClient(apiCode, serviceUrl);
            } else {
                this.serviceHttpClient = serviceHttpClient;
                if (apiCode != null)
                {
                    this.serviceHttpClient.ApiCode = apiCode;
                }
            }



            this.BlockExpolorer = new BlockExplorer.BlockExplorer(this.baseHttpClient);
			this.WalletCreator = new WalletCreator(this.serviceHttpClient);
			this.ExchangeRateExplorer = new ExchangeRateExplorer(this.baseHttpClient);
			this.TransactionPusher = new TransactionPusher(this.baseHttpClient);
			this.FundReceiver = new FundReceiver(this.baseHttpClient);
			this.StatisticsExplorer = new StatisticsExplorer(this.baseHttpClient);
		}

		/// <summary>
		/// Creates an instance of 'WalletHelper' based on the identifier allowing the use
		/// of that wallet
		/// </summary>
		/// <param name="identifier">Wallet identifier (GUID)</param>
		/// <param name="password">Decryption password</param>
		/// <param name="secondPassword">Second password</param>
		public WalletHelper CreateWalletHelper(string identifier, string password, string secondPassword = null)
		{
			return new WalletHelper(this.serviceHttpClient, identifier, password, secondPassword);
		}

		public void Dispose()
		{
            this.baseHttpClient.Dispose();
			this.serviceHttpClient.Dispose();
		}
	}
}
