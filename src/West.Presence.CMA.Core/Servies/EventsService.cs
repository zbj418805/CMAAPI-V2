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
        IEnumerable<Event> GetEvents(string serverIds, string searchKey, DateTime startTime, DateTime endTime);
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

        public IEnumerable<Event> GetEvents(string serverIds, string searchKey, DateTime startDate, DateTime endDate)
        {
            List<Event> allEvents = new List<Event>();
            int cacheDuration = _options.Value.CacheEventsDurationInSeconds;

            foreach (string serverId in serverIds.Split(','))
            {
                string cacheKey = $"{_options.Value.CacheEventsKey}_{_options.Value.Environment}_{serverId}_{startDate.ToString("yyyyMMdd")}";
                IEnumerable<Event> events;
                if (_cacheProvider.TryGetValue<IEnumerable<Event>>(cacheKey, out events))
                {
                    allEvents.AddRange(searchKey == "" ? events : events.Where(e => e.name.Contains(searchKey)));
                }
                else
                {
                    events = _eventsRepository.GetEvents(int.Parse(serverId), startDate, endDate);
                    _cacheProvider.Add(cacheKey, events, cacheDuration);
                    allEvents.AddRange(searchKey == "" ? events : events.Where(e => e.name.Contains(searchKey)));
                }
            }

            return allEvents.OrderBy(x => x.startTime);
        }
    }
}
