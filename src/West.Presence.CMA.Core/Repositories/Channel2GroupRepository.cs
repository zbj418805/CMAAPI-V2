using System;
using System.Collections.Generic;
using System.Net.Http;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;

namespace West.Presence.CMA.Core.Repositories
{

    public interface IChannel2GroupRepository
    {
        IEnumerable<Channel2Group> GetChannel2Group(string baseUrl);
        IEnumerable<Channel2Group> SetChannel2Group(string baseUrl, object data);
        bool DeleteChannel2Group(string baseUrl);
    }

    public class DBChannel2GroupRepository : DBBaseRepository, IChannel2GroupRepository
    {
        public DBChannel2GroupRepository()
        {
        }

        public IEnumerable<Channel2Group> GetChannel2Group(string baseUrl)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Channel2Group> SetChannel2Group(string baseUrl, object data)
        {
            throw new NotImplementedException();
        }
        public bool DeleteChannel2Group(string baseUrl)
        {
            throw new NotImplementedException();
        }
    }

    public class APIChannel2GroupRepository : IChannel2GroupRepository
    {
        private readonly IHttpClientProvider _httpClientProvider;

        public APIChannel2GroupRepository(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
        }

        public IEnumerable<Channel2Group> GetChannel2Group(string baseUrl)
        {
            return _httpClientProvider.GetData<Channel2Group>(baseUrl + $"/presence/Api/CMA/Channel2Groups");
        }

        public IEnumerable<Channel2Group> SetChannel2Group(string baseUrl, object data)
        {
            return _httpClientProvider.PostData<Channel2Group>(baseUrl + $"/presence/Api/CMA/Channel2Groups", data);
        }

        public bool DeleteChannel2Group(string baseUrl)
        {
            return _httpClientProvider.DeletetData(baseUrl + $"/presence/Api/CMA/Channel2Groups");
        }
    }
}
