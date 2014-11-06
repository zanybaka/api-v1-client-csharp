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

        private static volatile int timeoutMs = 10000;
        public static int TimeoutMs { get { return timeoutMs; } set { timeoutMs = value; } }

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
            string responseStr = null;
            string query = null;

            if (parameters != null && parameters.Count > 0)
            {
                query = string.Join("&", parameters.AllKeys.Select(key => string.Format("{0}={1}", 
                    HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(parameters[key]))));
            }

            var request = WebRequest.Create(BASE_URI + resource + (method == "GET" && query != null ? '?' + query : null));
            request.Timeout = timeoutMs;

            if (method == "POST")
            {
                byte[] bytes = Encoding.UTF8.GetBytes(query);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                var requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
            }

            try
            {
                var webResponse = (HttpWebResponse)request.GetResponse();
                responseStr = ReadStream(webResponse.GetResponseStream());
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    var httpResponse = e.Response as HttpWebResponse;
                    if (httpResponse != null && httpResponse.StatusCode != HttpStatusCode.OK)
                        throw new APIException(ReadStream(httpResponse.GetResponseStream()));
                    else
                        throw e;
                }
                else
                    throw e;
            }

            return responseStr;
        }

        private static string ReadStream(Stream stream)
        {
            using (var streamReader = new StreamReader(stream, Encoding.UTF8))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}
