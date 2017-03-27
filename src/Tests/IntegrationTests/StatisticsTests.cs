using Info.Blockchain.API.Client;
using Info.Blockchain.API.Statistics;
using Xunit;

namespace Info.Blockchain.API.Tests.IntegrationTests
{
    public class StatisticsTests
	{
		[Fact]
		public async void GetStatistics_Valid()
		{
			using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
			{
				StatisticsResponse statisticsResponse = await apiHelper.statisticsExplorer.GetAsync();
				Assert.NotNull(statisticsResponse);
			}
		}
	}
}
