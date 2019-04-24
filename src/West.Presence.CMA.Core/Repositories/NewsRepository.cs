using System;
using System.Collections.Generic;
using System.Net.Http;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;


namespace West.Presence.CMA.Core.Repositories
{
    public interface INewsRepository
    {
        IEnumerable<News> GetNews(int serverId, string baseUrl);
    }

    public class DBNewsRepository : INewsRepository
    {
        public DBNewsRepository()
        {

        }

        public IEnumerable<News> GetNews(int serverId, string baseUrl)
        {
            throw new NotImplementedException();
        }
    }

    public class APINewsRepository : INewsRepository
    {
        private readonly IHttpClientProvider _httpClientProvider;

        public APINewsRepository(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
        }

        public IEnumerable<News> GetNews(int serverId, string baseUrl)
        {
            return _httpClientProvider.GetData<News>(baseUrl + "/presence/Api/CMA/News/" + serverId);
        }
    }
}
