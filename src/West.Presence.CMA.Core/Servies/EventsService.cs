using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Repositories;

namespace West.Presence.CMA.Core.Servies
{
    public interface IEventsService
    {
        IEnumerable<Event> GetEvents(List<int> serverIds, string baseUrl, string searchKey, DateTime startTime, DateTime endTime, bool cutEvents=true);
    }

    public class EventsService : IEventsService
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly IOptions<CMAOptions> _options;
        private readonly IEventsRepository _eventsRepository;

        public EventsService(ICacheProvider cacheProvider, IOptions<CMAOptions> options, IEventsRepository eventRepository) 
        {
            _cacheProvider = cacheProvider;
            _options = options;
            _eventsRepository = eventRepository;
        }

        public IEnumerable<Event> GetEvents(List<int> serverIds, string baseUrl, string searchKey, DateTime startDate, DateTime endDate, bool cutEvents = true)
        {
            List<Event> allEvents = new List<Event>();
            //Get Cache duration
            int cacheDuration = _options.Value.CacheEventsDurationInSeconds;
            Uri u = new Uri(baseUrl);

            foreach (int serverId in serverIds)
            {
                //Set CacheKey
                string cacheKey = $"{_options.Value.CacheEventsKey}_{u.Host}_{serverId}_{startDate.ToString("yyyyMMdd")}";
                IEnumerable<Event> events;
                if (!_cacheProvider.TryGetValue<IEnumerable<Event>>(cacheKey, out events))
                {
                    //Get Events From Repo
                    events = _eventsRepository.GetEvents(serverId, baseUrl, startDate, endDate, cutEvents);
                    if (events != null)
                    {//Set Cache
                        _cacheProvider.Add(cacheKey, events, cacheDuration);
                    }
                    else
                    {
                        events = new List<Event>();
                    }
                }
                //Add to news collection
                allEvents.AddRange(searchKey == "" ? events : events.Where(e => e.Name.Contains(searchKey)));
            }

            return allEvents.OrderBy(x => x.StartTime);
        }
    }
}
