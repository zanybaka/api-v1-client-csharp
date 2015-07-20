using Info.Blockchain.API.BlockExplorer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

			BlockExplorer blockExplorer = new BlockExplorer();
			Address address = await blockExplorer.GetAddressAsync(addressString);

			this.TotalSentValue.Text = this.ToBtcString(address.TotalSent);
			this.TotalReceivedValue.Text = this.ToBtcString(address.TotalReceived);
			this.FinalBalanceValue.Text = this.ToBtcString(address.FinalBalance);
			this.Hash160Value.Text = address.Hash160;
			this.TransactionList.Items.Clear();
			foreach(Transaction transaction in address.Transactions)
			{
				this.AddTransaction(transaction, address);
			}
		}

		private void AddTransaction(Transaction transaction, Address address)
		{
			TextBlock textBox = new TextBlock();
			BitcoinValue totalValue = this.GetTransactionValue(transaction, address);
			string btcValue = this.ToBtcString(totalValue);
			DateTime dateTime = AddressLookupPage.UnixTimeStampToDateTime(transaction.Time);
			textBox.Text = string.Format("{0} on {1}", btcValue, dateTime);
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
			return value.Btc.ToString() + " BTC";
		}

		public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
		{
			// Unix timestamp is seconds past epoch
			System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
			return dtDateTime;
		}

	}
}
