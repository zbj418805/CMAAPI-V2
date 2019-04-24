using System;
using System.Collections.Generic;
using System.Net.Http;
using West.Presence.CMA.Core.Models;


namespace West.Presence.CMA.Core.Repositories
{
    public interface IChannelsRepository
    {
        IEnumerable<Channel> GetChannels(string baseUrl);
    }

    public class DBChannelsRepository : DBBaseRepository, IChannelsRepository
    {
        public DBChannelsRepository()
        {

        }

        public IEnumerable<Channel> GetChannels(string baseUrl)
        {
            throw new NotImplementedException();
        }
    }


    public class APIChannelsRepository : APIBaseRepository, IChannelsRepository
    {
        public APIChannelsRepository(IHttpClientFactory httpClientFactory)
           : base(httpClientFactory)
        {
        }

        public IEnumerable<Channel> GetChannels(string baseUrl)
        {
            return GetData<Channel>(baseUrl + $"/presence/Api/CMA/Channels");
        }
    }
}
