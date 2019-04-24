using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace West.Presence.CMA.Core.Helper
{
    public interface IHttpClientProvider
    {
        IEnumerable<T> GetData<T>(string url);
        IEnumerable<T> PostData<T>(string url, object data);
        bool DeletetData(string url);
    }

    public class HttpClientProvider : IHttpClientProvider
    {
        IHttpClientFactory _httpClientFactory;

        public HttpClientProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IEnumerable<T> GetData<T>(string url)
        {
            using (var client = _httpClientFactory.CreateClient("PresnceApi"))
            {
                string content = client.GetStringAsync(url).Result;
                return JsonConvert.DeserializeObject<List<T>>(content);
            }
        }

        public IEnumerable<T> PostData<T>(string url, object data)
        {
            using (var client = _httpClientFactory.CreateClient("PresnceApi"))
            {
                var jsonString = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                var response = client.PostAsync(url, content).Result;

                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<T>>(response.Content.ToString());
                }
                else
                {
                    return null;
                }
            }
        }

        public bool DeletetData(string url)
        {
            using (var client = _httpClientFactory.CreateClient("PresnceApi"))
            {
                var response = client.DeleteAsync(url).Result;
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }
    }
}
