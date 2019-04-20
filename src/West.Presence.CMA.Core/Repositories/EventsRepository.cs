using System;
using System.Collections.Generic;
using System.Text;
using West.Presence.CMA.Core.Models;

namespace West.Presence.CMA.Core.Repositories
{
    public interface IEventsRepository
    {
        IEnumerable<Event> GetEvents(int serverId, string searchKey, DateTime startDate, DateTime endDate)
    }

    public class EventsRepository
    {
        public EventsRepository()
        {

        }

        public IEnumerable<Event> GetEvents(int serverId, string searchKey, DateTime startDate, DateTime endDate)
        {
            return null;
        }
    }
}
