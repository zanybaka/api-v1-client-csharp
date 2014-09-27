using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Info.Blockchain.API
{
    public class HttpClient
    {
        private static string BASE_URI = "https://blockchain.info/";

        /// <summary>
        /// Performs a GET request on a Blockchain.info API resource. 
        /// </summary>
        /// <param name="resource">Resource path after https://blockchain.info/api/ </param>
        /// <param name="parameters">Collection containing request parameters</param>
        /// <returns>String response</returns>
        public static string Get(string resource, NameValueCollection parameters = null)
        {
            return OpenURL(resource, "GET", parameters);
        }

        /// <summary>
        /// Performs a POST request on a Blockchain.info API resource. 
        /// </summary>
        /// <param name="resource">Resource path after https://blockchain.info/api/ </param>
        /// <param name="parameters">Collection containing request parameters</param>
        /// <returns>String response</returns>
        public static string Post(string resource, NameValueCollection parameters = null)
        {
            return OpenURL(resource, "POST", parameters);
        }

        private static string OpenURL(String resource, string method,
            NameValueCollection parameters = null)
        {
            using (var client = new WebClient())
            {
                string response = null;
                try
                {
                    string query = null;
                    if (parameters != null && parameters.Count > 0)
                    {
                        query = string.Join("&", parameters.AllKeys.Select(key => string.Format("{0}={1}", 
                            HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(parameters[key]))));
                    }
                    
                    if (method == "GET")
                    {
                        response = client.DownloadString(BASE_URI + resource + "?" + query);
                    }
                    else if (method == "POST")
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(query);
                        byte[] byteResponse = client.UploadData(BASE_URI + resource, bytes);
                        response = System.Text.Encoding.UTF8.GetString(byteResponse);
                    }
                }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        var httpResponse = e.Response as HttpWebResponse;
                        if (httpResponse != null && httpResponse.StatusCode != HttpStatusCode.OK)
                        {
                            using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                            {
                                throw new APIException(reader.ReadToEnd());
                            }
                        }
                        else
                            throw e;
                    }
                    else
                        throw e;
                }

                return response;
            }
        }
    }
}
