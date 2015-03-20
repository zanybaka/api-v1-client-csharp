using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Info.Blockchain.API.CreateWallet
{
    /// <summary>
    /// This class reflects the functionality documented at https://blockchain.info/api/create_walleti. 
    /// It allows users to create new wallets as long as their API code has the 'generate wallet' permission. 
    /// </summary>
    public class CreateWallet
    {
        /// <summary>
        /// Creates a new Blockchain.info wallet. It can be created containing a pre-generated private key
        /// or will otherwise generate a new private key.
        /// </summary>
        /// <param name="password">Password for the new wallet. At least 10 characters.</param>
        /// <param name="apiCode">API code with create wallets permission</param>
        /// <param name="privateKey">Private key to add to the wallet</param>
        /// <param name="label">Label for the first address in the wallet</param>
        /// <param name="email">Email to associate with the new wallet</param>
        /// <returns>An instance of the CreateWalletResponse class</returns>
        /// <exception cref="APIException">If the server returns an error</exception>
        public static CreateWalletResponse Create(string password, string apiCode,
            string privateKey = null, string label = null, string email = null)
        {
            var req = new NameValueCollection();
            req["password"] = password;
            req["api_code"] = apiCode;
            if (privateKey != null)
                req["priv"] = privateKey;
            if (label != null)
                req["label"] = label;
            if (email != null)
                req["email"] = email;

            string response = HttpClient.Post("api/v2/create_wallet", req);
            JObject topElem = JObject.Parse(response);

            return new CreateWalletResponse((string)topElem["guid"],
                (string)topElem["address"], (string)topElem["link"]);
        }
    }
}
