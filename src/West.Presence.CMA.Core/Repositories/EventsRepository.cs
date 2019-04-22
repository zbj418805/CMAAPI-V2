using System;
using System.Collections.Generic;
using System.Net.Http;
using West.Presence.CMA.Core.Models;

namespace West.Presence.CMA.Core.Repositories
{
    public interface IEventsRepository
    {
        IEnumerable<Event> GetEvents(int serverId, string baseUrl, DateTime startDate, DateTime endDate);
    }

    public class EventsRepository : DBBaseRepository, IEventsRepository
    {
        
        public EventsRepository(string baseUrl)
        {

        }

        public IEnumerable<Event> GetEvents(int serverId, string baseUrl, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }

    public class APIEventsRepository : APIBaseRepository, IEventsRepository
    {
       
        //private readonly IHttpClientFactory _httpClientFactory;

        public APIEventsRepository(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory)
        {

        }

        public IEnumerable<Event> GetEvents(int serverId, string baseUrl, DateTime startDate, DateTime endDate)
        {
            return GetData<Event>(baseUrl + $"/presence/Api/CMA/Events/{serverId}/{startDate.ToString("yyyyMMdd")}/{startDate.ToString("yyyyMMdd")}");
        }
    }
}
