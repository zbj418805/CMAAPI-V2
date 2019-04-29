using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using West.Presence.CMA.Core.Models;

namespace West.Presence.CMA.Core.Helper
{
    public interface IHttpClientProvider
    {
        IEnumerable<T> GetData<T>(string url);
        IEnumerable<T> PostData<T>(string url, object data);
        IEnumerable<T> SoapPostData<T>(string url, object data);
        bool DeletetData(string url);
    }

    public class HttpClientProvider : IHttpClientProvider
    {
        IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger = Log.ForContext<HttpClientProvider>();

        public HttpClientProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IEnumerable<T> GetData<T>(string url)
        {
            using (var client = _httpClientFactory.CreateClient("PresnceApi"))
            {
                Uri u = new Uri(url);

                if (u.Scheme == "https")
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                }

                try
                {
                    string content = client.GetStringAsync(url).Result;
                    return JsonConvert.DeserializeObject<List<T>>(content);
                }
                catch(Exception e)
                {
                    _logger.Error("GetData", url, e.Message);
                    return null;
                }
            }
        }

        public IEnumerable<T> PostData<T>(string url, object data)
        {
            using (var client = _httpClientFactory.CreateClient("PresnceApi"))
            {
                Uri u = new Uri(url);
                if (u.Scheme == "https")
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                }

                var jsonString = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                try
                {

                    var response = client.PostAsync(url, content).Result;

                    int statusCode = (int)response.StatusCode;

                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<List<T>>(response.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch(Exception e)
                {
                    _logger.Error("SoapPostData", url, e.Message);
                    return null;
                }
            }
        }

        public IEnumerable<T> SoapPostData<T>(string url, object data)
        {
            using (var client = _httpClientFactory.CreateClient("PresnceApi"))
            {
                Uri u = new Uri(url);
                if (u.Scheme == "https")
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                }

                var jsonString = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                try
                {
                    var response = client.PostAsync(url, content).Result;

                    int statusCode = (int)response.StatusCode;

                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        var results =  JsonConvert.DeserializeObject<SoapReturnData<T>>(response.Content.ReadAsStringAsync().Result);
                        return results.d;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    _logger.Error("PostData", url, e.Message);
                    return null;
                }
            }
        }

        public bool DeletetData(string url)
        {
            using (var client = _httpClientFactory.CreateClient("PresnceApi"))
            {

                Uri u = new Uri(url);
                if (u.Scheme == "https")
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                }

                try
                {
                    var response = client.DeleteAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                        return true;
                    else
                        return false;
                }
                catch (Exception e)
                {
                    _logger.Error("DeletetData", url, e.Message);
                    return false;
                }
            }
        }
    }
}
