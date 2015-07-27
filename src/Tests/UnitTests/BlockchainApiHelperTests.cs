using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Info.Blockchain.API.Tests.UnitTests
{
	public class BlockchainApiHelperTests
	{
		[Fact]
		public void CreateHelper_Valid()
		{
			const string apiCode = "5";
			using (BlockchainApiHelper apiHelper = new BlockchainApiHelper(apiCode, new FakeHttpClient()))
			{
				Assert.NotNull(apiHelper);
				Assert.NotNull(apiHelper.StatisticsExplorer);
				Assert.NotNull(apiHelper.BlockExpolorer);
				Assert.NotNull(apiHelper.ExchangeRateExplorer);
				Assert.NotNull(apiHelper.FundReceiver);
				Assert.NotNull(apiHelper.TransactionPusher);
				Assert.NotNull(apiHelper.WalletCreator);
			}
		}
	}
}
