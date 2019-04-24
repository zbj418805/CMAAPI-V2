using System;
using System.Collections.Generic;
using System.Net.Http;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;

namespace West.Presence.CMA.Core.Repositories
{
    public interface IEventsRepository
    {
        IEnumerable<Event> GetEvents(int serverId, string baseUrl, DateTime startDate, DateTime endDate);
    }

    public class EventsRepository : IEventsRepository
    {
        
        public EventsRepository(string baseUrl)
        {

        }

        public IEnumerable<Event> GetEvents(int serverId, string baseUrl, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }

    public class APIEventsRepository : IEventsRepository
    {

        private readonly IHttpClientProvider _httpClientProvider;

        public APIEventsRepository(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
        }

        public IEnumerable<Event> GetEvents(int serverId, string baseUrl, DateTime startDate, DateTime endDate)
        {
            return _httpClientProvider.GetData<Event>(baseUrl + $"/presence/Api/CMA/Events/{serverId}/{startDate.ToString("yyyyMMdd")}/{startDate.ToString("yyyyMMdd")}");
        }
    }
}
