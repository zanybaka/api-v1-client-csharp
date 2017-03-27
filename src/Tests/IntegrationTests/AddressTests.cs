using Info.Blockchain.API.BlockExplorer;
using Info.Blockchain.API.Client;
using Xunit;

namespace Info.Blockchain.API.Tests.IntegrationTests
{
    public class AddressTests
	{
		[Fact]
		public async void GetAddress_ByAddress_Valid()
		{
			using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
			{
				const string addressString = "13k5KUK2vswXRdjgjxgCorGoY2EFGMFTnu";
				Address address = await apiHelper.blockExplorer.GetAddressAsync(addressString);
				Assert.NotNull(address);
				Assert.Equal(address.AddressStr, addressString);
			}
		}

		[Fact]
		public async void GetAddress_ByHash_Valid()
		{
			using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
			{
				const string hash = "1e15be27e4763513af36364674eebdba5a047323";
				Address address = await apiHelper.blockExplorer.GetAddressAsync(hash);
				Assert.NotNull(address);
				Assert.Equal(address.Hash160, hash);
			}
		}

		[Theory]
		[InlineData(100)]
		[InlineData(101)]
		[InlineData(76)]
		[InlineData(0)]
		[InlineData(3)]
		public async void GetAddress_LimitTransactions_Valid(int transactionCount)
		{
			using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
			{
				const string hash = "1e15be27e4763513af36364674eebdba5a047323";
				Address address = await apiHelper.blockExplorer.GetAddressAsync(hash, transactionCount);
				Assert.NotNull(address);
				Assert.Equal(address.Transactions.Count, transactionCount);
			}
		}
	}
}
