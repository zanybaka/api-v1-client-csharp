using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Info.Blockchain.API.Abstractions;
using Info.Blockchain.API.Receive;
using Xunit;

namespace Info.Blockchain.API.Tests.UnitTests
{
	public class ReceiveTests
	{
		[Fact]
		public async void ReceiveFunds_NullParameters_ArgumentNullExceptions()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
				{
					await apiHelper.FundReceiver.ReceiveFundsAsync(null, "callback");
				}
			});

			await Assert.ThrowsAsync<ArgumentNullException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
				{
					await apiHelper.FundReceiver.ReceiveFundsAsync("address", null);
				}
			});
		}

		[Fact]
		public async void ReceiveFunds_MockRequest_Valid()
		{
			using (BlockchainApiHelper apiHelper = new BlockchainApiHelper(baseHttpClient: new MockReceiveHttpClient()))
			{
				ReceiveResponse receiveResponse = await apiHelper.FundReceiver.ReceiveFundsAsync("MockAddress", "CallbackUrl");
				Assert.NotNull(receiveResponse);

				Assert.Equal(receiveResponse.CallbackUrl, "http://yoururl.com");
				Assert.Equal(receiveResponse.DestinationAddress, "1A8JiWcwvpY7tAopUkSnGuEYHmzGYfZPiq");
				Assert.Equal(receiveResponse.FeePercent, 0);
				Assert.Equal(receiveResponse.InputAddress, "1KZoUuPWFAeyVySHAGqvTUDoX6P3ntuLNF");
			}
		}

		public class MockReceiveHttpClient : IHttpClient
		{
			public void Dispose()
			{
			}

			public string ApiCode { get; set; }
			public Task<T> GetAsync<T>(string route, QueryString queryString = null, Func<string, T> customDeserialization = null)
			{
				ReceiveResponse receiveResponse = ReflectionUtil.DeserializeFile<ReceiveResponse>("receive_response_mock");
				if (receiveResponse is T)
				{
					return Task.FromResult((T)(object)receiveResponse);
				}
				return Task.FromResult(default(T));
			}

			public Task<TResponse> PostAsync<TPost, TResponse>(string route, TPost postObject, Func<string, TResponse> customDeserialization = null,
				bool multiPartContent = false)
			{
				throw new NotImplementedException();
			}
		}
	}
}
