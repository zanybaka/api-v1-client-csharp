using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Info.Blockchain.API.CreateWallet
{
    /// <summary>
    /// This class is used in response to the `Create` method in the `CreateWallet` class.
    /// </summary>
    public class CreateWalletResponse
    {
        public CreateWalletResponse(string identifier, string address, string link)
        {
            Identifier = identifier;
            Address = address;
            Link = link;
        }

        /// <summary>
        /// Wallet identifier (GUID)
        /// </summary>
        public string Identifier { get; private set; }

        /// <summary>
        /// First address in the wallet
        /// </summary>
        public string Address { get; private set; }

        /// <summary>
        /// Link to the wallet
        /// </summary>
        public string Link { get; private set; }
    }
}
