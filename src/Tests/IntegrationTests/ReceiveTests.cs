using Xunit;

namespace Info.Blockchain.API.Tests.IntegrationTests
{
	public class ReceiveTests
	{
		[Fact]
		public async void ReceiveFunds_BadAddress_ServerApiException()
		{
			//Dont want to spam. Checking to see if server responds
			ServerApiException serverApiException = await Assert.ThrowsAsync<ServerApiException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
				{
					await apiHelper.FundReceiver.ReceiveFundsAsync("Test", "Test");
				}
			});
			Assert.Contains("Invalid Destination", serverApiException.Message);
		} 
	}
}