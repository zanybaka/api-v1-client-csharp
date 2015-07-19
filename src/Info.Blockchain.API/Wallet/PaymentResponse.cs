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
        public PaymentResponse(string message, string txHash, string notice)
        {
            Message = message;
            TxHash = txHash;
            Notice = notice;
        }

        /// <summary>
        /// Response message from the server
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Transaction hash
        /// </summary>
        public string TxHash { get; private set; }

        /// <summary>
        /// Additional response message from the server
        /// </summary>
        public string Notice { get; private set; }
    }
}
