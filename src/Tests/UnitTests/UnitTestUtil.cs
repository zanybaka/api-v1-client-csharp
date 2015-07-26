namespace Info.Blockchain.API.Tests.UnitTests
{
	internal static class UnitTestUtil
	{
		internal static BlockchainApiHelper GetFakeHelper()
		{
			return new BlockchainApiHelper(httpClient: new FakeHttpClient());
		}
	}
}