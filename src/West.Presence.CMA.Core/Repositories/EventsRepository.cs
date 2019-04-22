using Newtonsoft.Json;
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
        private readonly IHttpClientFactory _httpClientFactory;

        public APIEventsRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IEnumerable<Event> GetEvents(int serverId, string baseUrl, DateTime startDate, DateTime endDate)
        {
            using (var client = _httpClientFactory.CreateClient("PresnceApi"))
            {
                string content = client.GetStringAsync(baseUrl + $"/presence/Api/CMA/Events/{serverId}/{startDate.ToString("yyyyMMdd")}/{startDate.ToString("yyyyMMdd")}").Result;
                return JsonConvert.DeserializeObject<List<Event>>(content);
            }
        }
    }
}
