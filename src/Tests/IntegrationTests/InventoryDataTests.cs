using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Info.Blockchain.API.BlockExplorer;
using Xunit;

namespace Info.Blockchain.API.Tests.IntegrationTests
{
	public class InventoryDataTests
	{
		[Fact]
		public async void GetInventoryData_ByHash_Valid()
		{
			using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
			{
				//have to get the latest block, hashes only are temporary
				LatestBlock latestBlock = await apiHelper.BlockExpolorer.GetLatestBlockAsync();
				InventoryData data = await apiHelper.BlockExpolorer.GetInventoryDataAsync(latestBlock.Hash);
				Assert.NotNull(data);

				Assert.Equal(latestBlock.Hash, data.Hash);
			}
		}
	}
}
