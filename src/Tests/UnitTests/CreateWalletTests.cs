using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Info.Blockchain.API.Tests.UnitTests
{
	public class CreateWalletTests
	{
		[Fact]
		public async void CreateWallet_NullPassword_ArgumentNullException()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper("APICODE"))
				{
					await apiHelper.WalletCreator.Create(null);
				}
			});
		}

		[Fact]
		public async void CreateWallet_NullApiCode_ArgumentNullException()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
				{
					await apiHelper.WalletCreator.Create("password");
				}
			});
		}
	}
}
