using System;
using System.Collections.Generic;
using System.Text;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Servies;

namespace West.Presence.CMA.Core.Presentations
{
    public interface IEventsPresentation
    {
        IEnumerable<Event> GetEvents(string serverIds, string baseUrl, string searchKey, DateTime startDate, DateTime endDate, int pageIndex, int pageSize);
    }

    public class EventsPresentation : PresentationBase, IEventsPresentation
    {
        private readonly IEventsService _eventsServise;

        public EventsPresentation(IEventsService eventsServise)
        {
            _eventsServise = eventsServise;
        }

        public IEnumerable<Event> GetEvents (string serverIds, string baseUrl, string searchKey, DateTime startDate, DateTime endDate, int pageIndex, int pageSize)
        {
            return GetPageItems<Event>(_eventsServise.GetEvents(serverIds, baseUrl, searchKey, startDate, endDate), pageIndex, pageSize);
        }
    }
}
