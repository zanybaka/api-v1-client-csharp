using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Info.Blockchain.API
{
	public class HttpClientUtil
	{
		private static string BASE_URI = "https://blockchain.info/";

		private const int timeoutMs = 10000;

		/// <summary>
		/// Performs a GET request on a Blockchain.info API resource. 
		/// </summary>
		/// <param name="resource">Resource path after https://blockchain.info/api/ </param>
		/// <param name="parameters">Collection containing request parameters</param>
		/// <returns>String response</returns>
		public static string Get(string resource, NameValueCollection parameters = null)
		{
			return OpenURL(resource, Method.Get, parameters).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Performs an asynchronous GET request on a Blockchain.info API resource. 
		/// </summary>
		/// <param name="resource">Resource path after https://blockchain.info/api/ </param>
		/// <param name="parameters">Collection containing request parameters</param>
		/// <returns>String response</returns>
		public static async Task<string> GetAsync(string resource, NameValueCollection parameters = null)
		{
			return await OpenURL(resource, Method.Get, parameters);
		}

		/// <summary>
		/// Performs a POST request on a Blockchain.info API resource. 
		/// </summary>
		/// <param name="resource">Resource path after https://blockchain.info/api/ </param>
		/// <param name="parameters">Collection containing request parameters</param>
		/// <returns>String response</returns>
		public static string Post(string resource, NameValueCollection parameters = null)
		{
			return OpenURL(resource, Method.Post, parameters).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Performs an asynchronous POST request on a Blockchain.info API resource. 
		/// </summary>
		/// <param name="resource">Resource path after https://blockchain.info/api/ </param>
		/// <param name="parameters">Collection containing request parameters</param>
		/// <returns>String response</returns>
		public static async Task<string> PostAsync(string resource, NameValueCollection parameters = null)
		{
			return await OpenURL(resource, Method.Post, parameters);
		}

		private static async Task<string> OpenURL(string resource, Method method,
			NameValueCollection parameters = null)
		{
			string query = null;

			if (parameters != null && parameters.Count > 0)
			{
				query = string.Join("&", parameters.AllKeys.Select(key => string.Format("{0}={1}",
					Uri.EscapeDataString(key), Uri.EscapeDataString(parameters[key]))));
			}
			HttpClient httpClient = new HttpClient()
			{
				BaseAddress = new Uri(HttpClientUtil.BASE_URI),
				Timeout = TimeSpan.FromMilliseconds(timeoutMs)
			};

			HttpResponseMessage response;

			switch (method)
			{
				case Method.Get:
					if (query != null)
					{
						resource += "?" + query;
					}
					response = await httpClient.GetAsync(resource);
					break;
				case Method.Post:
					HttpContent httpContent = new StringContent(query);
					response = await httpClient.PostAsync(resource, httpContent);
					break;
				default:
					throw new NotImplementedException();
			}

			if (!response.IsSuccessStatusCode)
			{
				throw new APIException(response.ReasonPhrase);
			}

			return await response.Content.ReadAsStringAsync();
		}

		private enum Method
		{
			Post,
			Get
		}
	}
}
