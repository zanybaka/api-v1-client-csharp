using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Info.Blockchain.API.Tests.UnitTests
{
	public class UnspentOutputTests
	{
		[Fact]
		public async void GetUnspentOutputs_NullAddress_ArgumentNullException()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
				{
					await apiHelper.BlockExpolorer.GetUnspentOutputsAsync(null);
				}
			});
		}
	}
}
