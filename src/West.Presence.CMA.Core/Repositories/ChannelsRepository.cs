using System;
using System.Collections.Generic;
using System.Net.Http;
using West.Presence.CMA.Core.Models;


namespace West.Presence.CMA.Core.Repositories
{
    public interface IChannelRepository
    {
        IEnumerable<Channel> GetChannels(string baseUrl);
    }

    public class DBChannelRepository : DBBaseRepository, IChannelRepository
    {
        public DBChannelRepository()
        {

        }

        public IEnumerable<Channel> GetChannels(string baseUrl)
        {
            throw new NotImplementedException();
        }
    }


    public class ApiChannelsRepository : APIBaseRepository, IChannelRepository
    {
        public ApiChannelsRepository(IHttpClientFactory httpClientFactory)
           : base(httpClientFactory)
        {
        }

        public IEnumerable<Channel> GetChannels(string baseUrl)
        {
            return GetData<Channel>(baseUrl + $"/presence/Api/CMA/Channels");
        }
    }
}
