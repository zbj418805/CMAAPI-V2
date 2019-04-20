using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
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
        private readonly IEventsRepository _eventsRepository;
        private readonly IOptions<CMAOptions> _options;

        public EventsService(ICacheProvider cacheProvider, IEventsRepository eventRepository, IOptions<CMAOptions> options) 
        {
            _cacheProvider = cacheProvider;
            _eventsRepository = eventRepository;
            _options = options;
        }

        public IEnumerable<Event> GetEvents(string serverIds, string searchKey, DateTime startDate, DateTime endDate)
        {
            foreach (string serverId in serverIds.Split(','))
            {
                IEnumerable<Event> events;
                if (_cacheProvider.TryGetValue<IEnumerable<Event>>($"{_options.Value.CacheEventKey}_{serverId}", out events))
                {

                }

            }

            

            return events;
        }
    }
}
