using Xunit;

namespace Info.Blockchain.API.Tests.IntegrationTests
{
	public class CreateWalletTests
	{
		[Fact]
		public async void CreateWallet_BadCredentials_ServerApiException()
		{
			//Dont want to spam to create wallets. Check to see if serialization works and get a message from the server
			ServerApiException exception = await Assert.ThrowsAsync<ServerApiException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
				{
					await apiHelper.WalletCreator.Create("badpassword");
				}
			});
			Assert.Contains("Password", exception.Message);
		}
	}
}
