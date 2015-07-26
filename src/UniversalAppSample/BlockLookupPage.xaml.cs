using Info.Blockchain.API;
using Info.Blockchain.API.BlockExplorer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalAppSample
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class BlockLookupPage : Page
	{
		public BlockLookupPage()
		{
			this.InitializeComponent();
		}

		private async void LookupButtonClick(object sender, RoutedEventArgs e)
		{
			using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
			{
				string blockHash = this.AddressTextBox.Text;
				Block block = await apiHelper.BlockExpolorer.GetBlockAsync(blockHash);

				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Bits: " + block.Bits);
				stringBuilder.AppendLine("Fees: " + block.Fees);
				stringBuilder.AppendLine("Hash: " + block.Hash);
				stringBuilder.AppendLine("Height: " + block.Height);
				stringBuilder.AppendLine("Index: " + block.Index);
				stringBuilder.AppendLine("Main Chain: " + block.MainChain);
				stringBuilder.AppendLine("MerkleRoot: " + block.MerkleRoot);
				stringBuilder.AppendLine("Nonce: " + block.Nonce);
				stringBuilder.AppendLine("Previous Block Hash: " + block.PreviousBlockHash);
				stringBuilder.AppendLine("Received Time: " + block.ReceivedTime.ToString("R"));
				stringBuilder.AppendLine("Relayed By: " + block.RelayedBy);
				stringBuilder.AppendLine("Size: " + block.Size);
				stringBuilder.AppendLine("Time: " + block.Time.ToString("R"));
				stringBuilder.AppendLine("Version: " + block.Version);
				stringBuilder.AppendLine("Transaction Count: " + block.Transactions.Count);

				this.BlockInfoTextBlock.Text = stringBuilder.ToString();
			}
        }
	}
}
