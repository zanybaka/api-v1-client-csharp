using Info.Blockchain.API;
using Info.Blockchain.API.BlockExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalAppSample
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class AddressLookupPage : Page
	{
		public AddressLookupPage()
		{
			this.InitializeComponent();
		}

		private async void LookupButtonClick(object sender, RoutedEventArgs e)
		{
			string addressString = this.AddressTextBox.Text;
			bool validAddress = this.AddressIsValid(addressString);
			if (!validAddress)
			{
				//TODO
			}

			using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
			{
				Address address = await apiHelper.BlockExpolorer.GetAddressAsync(addressString);
				
				this.TotalSentValue.Text = this.ToBtcString(address.TotalSent);
				this.TotalReceivedValue.Text = this.ToBtcString(address.TotalReceived);
				this.FinalBalanceValue.Text = this.ToBtcString(address.FinalBalance);
				this.Hash160Value.Text = address.Hash160;
				this.TransactionList.Items.Clear();
				foreach (Transaction transaction in address.Transactions)
				{
					this.AddTransaction(transaction, address);
				}
			}
		}

		private void AddTransaction(Transaction transaction, Address address)
		{
			TextBlock textBox = new TextBlock();
			BitcoinValue totalValue = this.GetTransactionValue(transaction, address);
			string btcValue = this.ToBtcString(totalValue);
			textBox.Text = string.Format("{0} on {1}", btcValue, transaction.Time);
			this.TransactionList.Items.Add(textBox);
		}

		private BitcoinValue GetTransactionValue(Transaction transaction, Address address)
		{
			List<Output> outputs = transaction.Outputs
				.Where(o => o.Address == address.AddressStr)
				.ToList();

			decimal totalValue = 0;
			if (outputs.Any())
			{
				totalValue += outputs
				.Sum(o => o.Value.Btc);
			}

			List<Input> inputs = transaction.Inputs
				.Where(o => o.PreviousOutput.Address == address.AddressStr)
				.ToList();

			if (inputs.Any())
			{
				totalValue -= inputs
				.Sum(o => o.PreviousOutput.Value.Btc);
			}

			return new BitcoinValue(totalValue);			
		}

		private bool AddressIsValid(string addressString)
		{
			return true; //TODO
		}

		private string ToBtcString(BitcoinValue value)
		{
			return value.ToString() + " BTC";
		}

	}
}
