using Info.Blockchain.API.BlockExplorer;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Info.Blockchain.API.Wallet
{
    /// <summary>
    /// Used in combination with the `Wallet` class
    /// </summary>
    public class Address
    {
		private string v1;
		private string v2;
		private int v3;

		public Address(JObject a)
        {
            Balance = BitcoinValue.FromSatoshis((long)a["balance"]);
            AddressStr = (string)a["address"];
            Label = (string)a["label"];
            TotalReceived = BitcoinValue.FromSatoshis((long)a["total_received"]);
        }

        public Address(BitcoinValue balance, string address,
            string label, BitcoinValue totalReceived)
        {
            Balance = balance;
            AddressStr = address;
            Label = label;
            TotalReceived = totalReceived;
        }

		public Address(JObject a, string v1, string v2, int v3) : this(a)
		{
			this.v1 = v1;
			this.v2 = v2;
			this.v3 = v3;
		}


		/// <summary>
		/// Balance in bitcoins
		/// </summary>
		public BitcoinValue Balance { get; private set; }

        /// <summary>
        /// String representation of the address
        /// </summary>
        public string AddressStr { get; private set; }

        /// <summary>
        /// Label attached to the address
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// Total received amount
        /// </summary>
        public BitcoinValue TotalReceived { get; private set; }
    }
}
