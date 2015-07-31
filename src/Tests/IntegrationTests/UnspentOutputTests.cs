using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Info.Blockchain.API.BlockExplorer;
using Xunit;

namespace Info.Blockchain.API.Tests.IntegrationTests
{
	public class UnspentOutputTests
	{
		[Fact]
		public async void GetUnspent_ByAdress_Valid()
		{
			using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
			{
				const string address = "13k5KUK2vswXRdjgjxgCorGoY2EFGMFTnu";
				ReadOnlyCollection<UnspentOutput> outputs = await apiHelper.BlockExpolorer.GetUnspentOutputsAsync(address);
				Assert.NotNull(outputs);
			}
		}
	}
}
