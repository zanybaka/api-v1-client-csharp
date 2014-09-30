using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Info.Blockchain.API.Receive
{
    /// <summary>
    /// This class is used as a response object to the `Receive.receive` method. 
    /// </summary>
    public class ReceiveResponse
    {
        public ReceiveResponse(int feePercent, string destinationAddress,
            string inputAddress, string callbackUrl)
        {
            FeePercent = feePercent;
            DestinationAddress = destinationAddress;
            InputAddress = inputAddress;
            CallbackUrl = callbackUrl;
        }

        /// <summary>
        /// Forwarding fee
        /// </summary>
        public int FeePercent { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string DestinationAddress { get; private set; }

        /// <summary>
        /// Input address where the funds should be sent
        /// </summary>
        public string InputAddress { get; private set; }

        /// <summary>
        /// Callback URL that will be called upon payment
        /// </summary>
        public string CallbackUrl { get; private set; }
    }
}
