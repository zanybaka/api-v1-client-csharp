using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Info.Blockchain.API.BlockExplorer
{
    /// <summary>
    /// This class contains data related to inventory messages 
    /// that Blockchain.info received for an object.
    /// </summary>
    public class InventoryData
    {
        public InventoryData(JObject i)
        {
            Hash = (string)i["hash"];
            Type = (string)i["type"];
            InitialTime = (long)i["initial_time"];
            LastTime = (long)i["last_time"];
            InitialIP = (string)i["initial_ip"];
            NConnected = (int)i["nconnected"];
            RelayedCount = (int)i["relayed_count"];
            RelayedPercent = (int)i["relayed_percent"];
        }

        /// <summary>
        /// Object hash
        /// </summary>
        public string Hash { get; private set; }

        /// <summary>
        /// Object type
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// The time Blockchain.info first received an inventory message
        /// containing a hash for this transaction (unix time in ms).
        /// </summary>
        public long InitialTime { get; private set; }

        /// <summary>
        /// The last time Blockchain.info received an inventory message 
        /// containing a hash for this transaction (unix time in ms).
        /// </summary>
        public long LastTime { get; private set; }

        /// <summary>
        /// IP of the peer from which Blockchain.info first received an inventory 
        /// message containing a hash for this transaction.
        /// </summary>
        public string InitialIP { get; private set; }

        /// <summary>
        /// Number of nodes that Blockchain.info is currently connected to.
        /// </summary>
        public int NConnected { get; private set; }
        
        /// <summary>
        /// Number of nodes Blockchain.info received an inventory message containing 
        /// a hash for this transaction from.
        /// </summary>
        public int RelayedCount { get; private set; }

        /// <summary>
        /// Ratio of nodes that Blockchain.info received an inventory message
        /// containing a hash for this transaction from and the number of connected nodes.
        /// </summary>
        public int RelayedPercent { get; private set; }
    }
}
