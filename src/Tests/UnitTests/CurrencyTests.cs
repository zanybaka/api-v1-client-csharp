using System;
using Info.Blockchain.API.Client;
using Xunit;

namespace Info.Blockchain.API.Tests.UnitTests
{
	public class CurrencyTests
	{
		[Fact]
		public async void ToBtc_NullCurrency_ArgumentNullException()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
				{
					await apiHelper.exchangeRateExplorer.ToBtcAsync(null, 1);
				}
			});
		}

		[Fact]
		public async void ToBtc_NegativeValue_ArgumentOutOfRangeException()
		{
			await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
				{
					await apiHelper.exchangeRateExplorer.ToBtcAsync("USD", -1);
				}
			});
		}
	}
}
