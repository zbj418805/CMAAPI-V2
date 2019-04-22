using System;
using System.Collections.Generic;
using System.Net.Http;
using West.Presence.CMA.Core.Models;

namespace West.Presence.CMA.Core.Repositories
{

    public interface IChannel2GroupRepository
    {
        IEnumerable<Channel2Group> GetChannel2Group(string baseUrl);
        IEnumerable<Channel2Group> SetChannel2Group(string baseUrl, object data);
        bool DeleteChannel2Group(string baseUrl, object data);
    }

    public class DBChanel2GroupRepository : DBBaseRepository, IChannel2GroupRepository
    {
        public DBChanel2GroupRepository()
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
        public bool DeleteChannel2Group(string baseUrl, object data)
        {
            throw new NotImplementedException();
        }
    }

    public class APIChannel2GroupRepository : APIBaseRepository, IChannel2GroupRepository
    {
        public APIChannel2GroupRepository(IHttpClientFactory httpClientFactory)
            :base(httpClientFactory)
        {

        }

        public IEnumerable<Channel2Group> GetChannel2Group(string baseUrl)
        {
            return GetData<Channel2Group>(baseUrl + $"/presence/Api/CMA/Channel2Groups");
        }

        public IEnumerable<Channel2Group> SetChannel2Group(string baseUrl, object data)
        {
            return PostData<Channel2Group>(baseUrl + $"/presence/Api/CMA/Channel2Groups", data);
        }

        public bool DeleteChannel2Group(string baseUrl, object data)
        {
            return DeletetData<Channel2Group>(baseUrl + $"/presence/Api/CMA/Channel2Groups", data);
        }
    }
}
