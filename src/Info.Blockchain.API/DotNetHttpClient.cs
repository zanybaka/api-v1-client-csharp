using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Info.Blockchain.API.Abstractions;
using Newtonsoft.Json;

namespace Info.Blockchain.API
{
	internal class DotNetHttpClient : IHttpClient
	{
		private const string BASE_URI = "https://blockchain.info/";
		private const int TIMEOUT_MS = 100000;
		private HttpClient httpClient { get; }

		public string ApiCode { get; set; }

		public DotNetHttpClient(string apiCode)
		{
			this.ApiCode = apiCode;
			this.httpClient = new HttpClient
			{
				BaseAddress = new Uri(DotNetHttpClient.BASE_URI),
				Timeout = TimeSpan.FromMilliseconds(DotNetHttpClient.TIMEOUT_MS)
			};
		}

		public async Task<T> GetAsync<T>(string route, QueryString queryString = null, Func<string, T> customDeserialization = null)
		{
			//TODO check to see if the route has a query string already
			if (this.ApiCode != null)
			{
				queryString?.Add("api_code", this.ApiCode);
			}
			if (queryString != null && queryString.Count > 0)
			{
				route += queryString.ToString();
			}
			HttpResponseMessage response = await this.httpClient.GetAsync(route);
			this.ValidateResponse(response);
			string responseString = await response.Content.ReadAsStringAsync();
			var responseObject = customDeserialization == null
				? JsonConvert.DeserializeObject<T>(responseString)
				: customDeserialization(responseString);
			return responseObject;
		}

		public async Task<TResponse> PostAsync<TPost, TResponse>(string route, TPost postObject, Func<string, TResponse> customDeserialization = null)
		{
			if (this.ApiCode != null)
			{
				route += "?api_code=" + this.ApiCode;
			}
			string json = JsonConvert.SerializeObject(postObject);
			HttpContent httpContent = new StringContent(json);
			HttpResponseMessage response = await this.httpClient.PostAsync(route, httpContent);
			await this.ValidateResponse(response);
			string responseString = await response.Content.ReadAsStringAsync();
			TResponse responseObject = JsonConvert.DeserializeObject<TResponse>(responseString);
			return responseObject;
		}

		private async Task ValidateResponse(HttpResponseMessage response)
		{
			if (response.IsSuccessStatusCode)
			{
				return;
			}
			string responseContent = await response.Content.ReadAsStringAsync();
			if (string.Equals(responseContent, "Block Not Found"))
			{
				throw new ServerApiException("Block Not Found", HttpStatusCode.NotFound);
			}
			throw new ServerApiException(response.ReasonPhrase + ": " + responseContent, response.StatusCode);
		}

		public void Dispose()
		{
			this.httpClient.Dispose();
		}
	}
}
