using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Info.Blockchain.API.Wallet
{  
    /// <summary>
    /// Used as a response object to the `send` and `sendMany` methods in the `Wallet` class.
    /// </summary>
    public class PaymentResponse
    {
        public PaymentResponse(String message, String txHash, String notice)
        {
            Message = message;
            TxHash = txHash;
            Notice = notice;
        }

        /// <summary>
        /// Response message from the server
        /// </summary>
        public String Message { get; private set; }

        /// <summary>
        /// Transaction hash
        /// </summary>
        public String TxHash { get; private set; }

        /// <summary>
        /// Additional response message from the server
        /// </summary>
        public String Notice { get; private set; }
    }
}
