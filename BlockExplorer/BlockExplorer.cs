using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Info.Blockchain.API.BlockExplorer
{
    /// <summary>
    /// The BlockExplorer class reflects the functionality documented at 
    /// https://blockchain.info/api/blockchain_api. It can be used to query the block chain, 
    /// fetch block, transaction and address data, get unspent outputs for an address etc.
    /// </summary>
    public class BlockExplorer
    {
        private string apiCode;

        public BlockExplorer(string apiCode = null)
        {
            this.apiCode = apiCode;
        }

        /// <summary>
        ///  Gets a single transaction based on a transaction index.
        /// </summary>
        /// <param name="txIndex">Transaction index</param>
        /// <returns>An instance of the Transaction class</returns>
        /// <exception cref="APIException">If the server returns an error</exception>
        public Transaction GetTransaction(long txIndex)
        {
            return GetTransaction(txIndex.ToString());
        }

        /// <summary>
        ///  Gets a single transaction based on a transaction hash.
        /// </summary>
        /// <param name="txHash">Transaction hash</param>
        /// <returns>An instance of the Transaction class</returns>
        /// <exception cref="APIException">If the server returns an error</exception>
        public Transaction GetTransaction(string txHash)
        {
            var req = new NameValueCollection();
            if (apiCode != null)
                req["api_code"] = apiCode;

            string response = HttpClient.Get("rawtx/" + txHash, req);
            var txJson = JObject.Parse(response);
            return new Transaction(txJson);
        }

        /// <summary>
        /// Gets a single block based on a block index.
        /// </summary>
        /// <param name="blockIndex">Block index</param>
        /// <returns>An instance of the Block class</returns>
        /// <exception cref="APIException">If the server returns an error</exception>
        public Block GetBlock(long blockIndex)
        {
            return GetBlock(blockIndex.ToString());
        }

        /// <summary>
        /// Gets a single block based on a block hash.
        /// </summary>
        /// <param name="blockHash">Block hash</param>
        /// <returns>An instance of the Block class</returns>
        /// <exception cref="APIException">If the server returns an error</exception>
        public Block GetBlock(string blockHash)
        {
            var req = new NameValueCollection();
            if (apiCode != null)
                req["api_code"] = apiCode;

            string response = HttpClient.Get("rawblock/" + blockHash, req);
            var txJson = JObject.Parse(response);
            return new Block(txJson);
        }

        /// <summary>
        /// Gets data for a single address.
        /// </summary>
        /// <param name="address">Base58check or hash160 address string</param>
        /// <returns>An instance of the Address class</returns>
        /// <exception cref="APIException">If the server returns an error</exception>
        public Address GetAddress(string address)
        {
            var req = new NameValueCollection();
            if (apiCode != null)
                req["api_code"] = apiCode;

            string response = HttpClient.Get("rawaddr/" + address, req);
            var addrJson = JObject.Parse(response);
            return new Address(addrJson);
        }

        /// <summary>
        /// Gets a list of blocks at the specified height. Normally, only one block will be returned, 
        /// but in case of a chain fork, multiple blocks may be present.
        /// </summary>
        /// <param name="height">Block height</param>
        /// <returns>A list of blocks at the specified height</returns>
        /// <exception cref="APIException">If the server returns an error</exception>
        public IEnumerable<Block> GetBlocksAtHeight(long height)
        {
            var req = new NameValueCollection();
            req["format"] = "json";
            if (apiCode != null)
                req["api_code"] = apiCode;

            string response = HttpClient.Get("block-height/" + height, req);
            var blocksJson = JObject.Parse(response);

            List<Block> blocks = new List<Block>();
            foreach (var b in blocksJson["blocks"].AsJEnumerable())
            {
                blocks.Add(new Block((JObject)b));
            }

            return blocks;
        }

        /// <summary>
        /// Gets unspent outputs for a single address.
        /// </summary>
        /// <param name="address">Base58check or hash160 address string</param>
        /// <returns>A list of unspent outputs for the specified address </returns>
        /// <exception cref="APIException">If the server returns an error</exception>
        public IEnumerable<UnspentOutput> GetUnspentOutputs(string address)
        {
            var req = new NameValueCollection();
            req["active"] = address;
            if (apiCode != null)
                req["api_code"] = apiCode;

            string response = null;

            try
            {
                response = HttpClient.Get("unspent", req);
            }
            catch (APIException e)
            {
                // the API isn't supposed to return an error code here. No free outputs is
                // a legitimate situation. We are circumventing that by returning an empty list
                if (e.Message == "No free outputs to spend")
                    return new List<UnspentOutput>();
                else
                    throw e;
            }

            return JObject.Parse(response)["unspent_outputs"].
                AsJEnumerable().Select(x => new UnspentOutput((JObject)x)).ToList();
        }
    }
}
