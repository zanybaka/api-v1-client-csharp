using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Info.Blockchain.API.BlockExplorer;
using KellermanSoftware.CompareNetObjects;
using Xunit;

namespace Info.Blockchain.API.Tests.UnitTests
{
	public class TransactionTests
	{
		private BlockchainApiHelper GetFakeHelper()
		{
			return new BlockchainApiHelper(httpClient: new FakeHttpClient());
		}

		[Fact]
		public async void GetTransaction_BadIds_ArgumentExecption()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = this.GetFakeHelper())
				{
					await apiHelper.BlockExpolorer.GetTransactionAsync(null);
				}
			});

			await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = this.GetFakeHelper())
				{
					await apiHelper.BlockExpolorer.GetTransactionByIndexAsync(-1);
				}
			});
		}
	}
}
