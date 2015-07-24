using Info.Blockchain.API.Abstractions;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Info.Blockchain.API
{
	public class DotNetHttpClient : IHttpClient
	{
		private const string baseUri = "https://blockchain.info/";
		private const int timeoutMs = 10000;
		private HttpClient httpClient { get; }

		public string ApiCode { get; set; }

		public DotNetHttpClient(string apiCode)
		{
			this.ApiCode = apiCode;
			this.httpClient = new HttpClient()
			{
				BaseAddress = new Uri(DotNetHttpClient.baseUri),
				Timeout = TimeSpan.FromMilliseconds(timeoutMs)
			};
		}

		public async Task<T> GetAsync<T>(string route, QueryString queryString = null)
		{
			//TODO check to see if the route has a query string already
			if(this.ApiCode != null && queryString != null)
			{
				queryString.Add("api_code", this.ApiCode);
			}
			if (queryString != null && queryString.Count > 0)
			{
				route += queryString.ToString();
			}
			HttpResponseMessage response = await httpClient.GetAsync(route);
			this.ValidateResponse(response);
			string responseString = await response.Content.ReadAsStringAsync();
			T responseObject = JsonConvert.DeserializeObject<T>(responseString);
			return responseObject;
		}

		public async Task<TResponse> PostAsync<TPost, TResponse>(string route, TPost postObject)
		{
			if (this.ApiCode != null)
			{
				route += "?api_code=" + this.ApiCode;
			}
			string json = JsonConvert.SerializeObject(postObject);
			HttpContent httpContent = new StringContent(json);
			HttpResponseMessage response = await httpClient.PostAsync(route, httpContent);
			this.ValidateResponse(response);
			string responseString = await response.Content.ReadAsStringAsync();
			TResponse responseObject = JsonConvert.DeserializeObject<TResponse>(responseString);
			return responseObject;
		}

		private void ValidateResponse(HttpResponseMessage response)
		{
			if (!response.IsSuccessStatusCode)
			{
				throw new ServerApiException(response.ReasonPhrase, response.StatusCode);
			}
		}

		public void Dispose()
		{
			this.httpClient.Dispose();
		}
	}
}
