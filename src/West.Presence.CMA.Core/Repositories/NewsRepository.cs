using System;
using System.Collections.Generic;
using System.Net.Http;
using West.Presence.CMA.Core.Models;


namespace West.Presence.CMA.Core.Repositories
{
    public interface INewsRepository
    {
        IEnumerable<News> GetNews(int serverId, string baseUrl);
    }

    public class DBNewsRepository : DBBaseRepository, INewsRepository
    {
        public DBNewsRepository()
        {

        }

        public IEnumerable<News> GetNews(int serverId, string baseUrl)
        {
            throw new NotImplementedException();
        }
    }

    public class APINewsRepository : APIBaseRepository, INewsRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public APINewsRepository(IHttpClientFactory httpClientFactory) :
            base(httpClientFactory)
        {

        }

        public IEnumerable<News> GetNews(int serverId, string baseUrl)
        {
            return GetData<News>(baseUrl + "/presence/Api/CMA/News/" + serverId);
        }
    }
}
