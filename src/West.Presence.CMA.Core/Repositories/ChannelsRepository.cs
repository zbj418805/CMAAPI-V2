using System;
using System.Collections.Generic;
using System.Net.Http;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;


namespace West.Presence.CMA.Core.Repositories
{
    public interface IChannelRepository
    {
        IEnumerable<Channel> GetChannels(string baseUrl);
    }

    public class DBChannelRepository : IChannelRepository
    {
        public DBChannelRepository()
        {

        }

        public IEnumerable<Channel> GetChannels(string baseUrl)
        {
            throw new NotImplementedException();
        }
    }


    public class APIChannelsRepository : IChannelRepository
    {
        private readonly IHttpClientProvider _httpClientProvider;

        public APIChannelsRepository(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
        }

        public IEnumerable<Channel> GetChannels(string baseUrl)
        {
            return _httpClientProvider.GetData<Channel>(baseUrl + $"/presence/Api/CMA/Channels");
        }
    }
}
