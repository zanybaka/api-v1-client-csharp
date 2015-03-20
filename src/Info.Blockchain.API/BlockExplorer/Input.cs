using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Info.Blockchain.API.BlockExplorer
{
    /// <summary>
    /// Represents a transaction input. If the PreviousOutput object is null,
    /// this is a coinbase input.
    /// </summary>
    public class Input
    {
        public Input(JObject i)
        {
            JObject prevOut = i["prev_out"] as JObject;
            if (prevOut != null)
                PreviousOutput = new Output(prevOut, true);

            Sequence = (long)i["sequence"];
            ScriptSignature = (string)i["script"];
        }

        /// <summary>
        /// Previous output. If null, this is a coinbase input.
        /// </summary>
        public Output PreviousOutput { get; private set; }

        /// <summary>
        /// Sequence number of the input
        /// </summary>
        public long Sequence { get; private set; }

        /// <summary>
        /// Script signature
        /// </summary>
        public string ScriptSignature { get; private set; }
    }
}
