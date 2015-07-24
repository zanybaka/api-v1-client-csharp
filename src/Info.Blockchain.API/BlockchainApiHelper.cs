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
		private IHttpClient httpClient { get; }
		public BlockExplorer.BlockExplorer BlockExpolorer { get; }
		public WalletCreator WalletCreator { get; }
		public ExchangeRateExplorer ExchangeRateExplorer { get; }
		public TransactionPusher TransactionPusher { get; }
		public FundReceiver FundReceiver { get; }
		public StatisticsExplorer StatisticsExplorer { get; }


		public BlockchainApiHelper(string apiCode = null)
		{
			this.httpClient = new DotNetHttpClient(apiCode);

			this.BlockExpolorer = new BlockExplorer.BlockExplorer(this.httpClient);
			this.WalletCreator = new WalletCreator(this.httpClient);
			this.ExchangeRateExplorer = new ExchangeRateExplorer(this.httpClient);
			this.TransactionPusher = new TransactionPusher(this.httpClient);
			this.FundReceiver = new FundReceiver(this.httpClient);
			this.StatisticsExplorer = new StatisticsExplorer(this.httpClient);
		}

		/// <summary>
		/// Creates an instance of 'WalletHelper' based on the identifier allowing the use
		/// of that wallet
		/// </summary>
		/// <param name="identifier">Wallet identifier (GUID)</param>
		/// <param name="password">Decryption password</param>
		/// <param name="secondPassword">Second password</param>
		public WalletHelper CreateWalletHelper(string identifier, string password, string secondPassword)
		{
			return new WalletHelper(this.httpClient, identifier, password, secondPassword);
		}

		public void Dispose()
		{
			this.httpClient.Dispose();
		}
	}
}
