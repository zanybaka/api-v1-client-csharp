using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Info.Blockchain.API.Wallet
{
    /// <summary>
    /// This class reflects the functionality documented
    /// at https://blockchain.info/api/blockchain_wallet_api. It allows users to interact
    /// with their Blockchain.info wallet.
    /// </summary>
    public class Wallet
    {
        private string identifier;
        private string password;
        private string secondPassword;
        private string apiCode;

        /// <summary>
        /// </summary>
        /// <param name="identifier">Wallet identifier (GUID)</param>
        /// <param name="password">Decryption password</param>
        /// <param name="secondPassword">Second password</param>
        /// <param name="apiCode">Blockchain.info API code</param>
        /// <exception cref="APIException">If the server returns an error</exception>
        public Wallet(String identifier, String password,
            String secondPassword = null, String apiCode = null)
        {
            this.identifier = identifier;
            this.password = password;
            this.secondPassword = secondPassword;
            this.apiCode = apiCode;
        }

        /// <summary>
        /// Fetches the wallet balance. Includes unconfirmed transactions 
        /// and possibly double spends.
        /// </summary>
        /// <returns>Wallet balance in satoshi</returns>
        /// <exception cref="Info.Blockchain.API.APIException">If the server returns an error</exception>
        public long GetBalance()
        {
            String response = HttpClient.Get(string.Format("merchant/{0}/balance", identifier),
                BuildBasicRequest());
            JObject topElem = ParseResponse(response);

            return Convert.ToInt64(topElem.Property("balance").Value);
        }

        /// <summary>
        /// Lists all active addresses in the wallet.
        /// </summary>
        /// <param name="confirmations">Minimum number of confirmations transactions 
        /// must have before being included in the balance of addresses (can be 0)</param>
        /// <returns>A list of Address objects</returns>
        /// <exception cref="Info.Blockchain.API.APIException">If the server returns an error</exception>
        public List<Address> ListAddresses(int confirmations = 0)
        {
            var req = BuildBasicRequest();
            req["confirmations"] = confirmations.ToString();

            string response = HttpClient.Get(String.Format("merchant/{0}/list",
                identifier), req);
            JObject topElem = ParseResponse(response);
            JArray jAddresses = (JArray)topElem["addresses"];

            List<Address> addresses = new List<Address>();
            foreach (var jaddr in jAddresses)
            {
                addresses.Add(new Address((JObject)jaddr));
            }

            return addresses;
        }

        private JObject ParseResponse(string response)
        {
            JObject topElem = JObject.Parse(response);
            var error = topElem["error"];
            if (error != null)
                throw new APIException((string)error);
            
            return topElem;
        }

        private NameValueCollection BuildBasicRequest()
        {
            var req = new NameValueCollection();
            
            req["password"] = password;
            if (apiCode != null)
                req["api_code"] = apiCode;
            if (secondPassword != null)
                req["second_password"] = secondPassword;

            return req;
        }
    }
}
