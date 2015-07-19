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
        public Address(JObject a)
        {
            Balance = (long)a["balance"];
            AddressStr = (string)a["address"];
            Label = (string)a["label"];
            TotalReceived = (long)a["total_received"];
        }

        public Address(long balance, string address,
            string label, long totalReceived)
        {
            Balance = balance;
            AddressStr = address;
            Label = label;
            TotalReceived = totalReceived;
        }

        /// <summary>
        /// Balance in satoshi
        /// </summary>
        public long Balance { get; private set; }

        /// <summary>
        /// String representation of the address
        /// </summary>
        public string AddressStr { get; private set; }

        /// <summary>
        /// Label attached to the address
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// Total received amount in satoshi
        /// </summary>
        public long TotalReceived { get; private set; }
    }
}
