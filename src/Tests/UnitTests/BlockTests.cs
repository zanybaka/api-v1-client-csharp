using System;
using Xunit;

namespace Info.Blockchain.API.Tests.UnitTests
{
	public class BlockTests
	{
		private BlockchainApiHelper GetFakeHelper()
		{
			return new BlockchainApiHelper(httpClient: new FakeHttpClient());
		}

		[Fact]
		public async void GetBlock_BadIds_ArgumentException()
		{
			await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = this.GetFakeHelper())
				{
					await apiHelper.BlockExpolorer.GetBlockAsync(-1);
				}
			});


			await Assert.ThrowsAsync<ArgumentNullException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = this.GetFakeHelper())
				{
					await apiHelper.BlockExpolorer.GetBlockAsync(null);
				}
			});
		}

		[Fact]
		public async void GetBlocks_BadParameters_ArgumentException()
		{
			await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = this.GetFakeHelper())
				{
					await apiHelper.BlockExpolorer.GetBlocksAsync(-1);
				}
			});


			await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = this.GetFakeHelper())
				{
					await apiHelper.BlockExpolorer.GetBlocksAsync(1000);
				}
			});

			await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = this.GetFakeHelper())
				{
					await apiHelper.BlockExpolorer.GetBlocksAsync(int.MaxValue);
				}
			});


			await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = this.GetFakeHelper())
				{
					await apiHelper.BlockExpolorer.GetBlocksAsync(DateTime.MinValue);
				}
			});

			await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = this.GetFakeHelper())
				{
					await apiHelper.BlockExpolorer.GetBlocksAsync(DateTime.MaxValue);
				}
			});
		}

		[Fact]
		public async void GetBlocksByHeight_BadParameters_ArgumentException()
		{
			await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = this.GetFakeHelper())
				{
					await apiHelper.BlockExpolorer.GetBlocksAtHeightAsync(-1);
				}
			});
		}
	}
}
