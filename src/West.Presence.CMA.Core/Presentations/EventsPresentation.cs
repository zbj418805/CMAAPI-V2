using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Servies;

namespace West.Presence.CMA.Core.Presentations
{
    public interface IEventsPresentation
    {
        IEnumerable<Event> GetEvents(List<int> serverIds, string baseUrl, string searchKey, DateTime startDate, DateTime endDate, int pageIndex, int pageSize, out int total);
    }

    public class EventsPresentation : PresentationBase, IEventsPresentation
    {
        private readonly IEventsService _eventsServise;

        public EventsPresentation(IEventsService eventsServise)
        {
            _eventsServise = eventsServise;
        }

        public IEnumerable<Event> GetEvents (List<int> serverIds, string baseUrl, string searchKey, DateTime startDate, DateTime endDate, int pageIndex, int pageSize, out int total)
        {
            var events = _eventsServise.GetEvents(serverIds, baseUrl, searchKey, startDate, endDate);
            total = events.Count();

            return GetPageItems<Event>(events, pageIndex, pageSize);
        }
    }
}
