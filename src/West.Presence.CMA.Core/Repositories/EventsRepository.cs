using System;
using System.Collections.Generic;
using West.Presence.CMA.Core.Models;

namespace West.Presence.CMA.Core.Repositories
{
    public interface IEventsRepository
    {
        IEnumerable<Event> GetEvents(int serverId, string searchKey, DateTime startDate, DateTime endDate);
    }

    public class EventsRepository : IEventsRepository
    {
        public EventsRepository()
        {

        }

        public IEnumerable<Event> GetEvents(int serverId, string searchKey, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}
