using System;
using System.Collections.Generic;
using West.Presence.CMA.Core.Models;

namespace West.Presence.CMA.Core.Repositories
{
    public interface IPeopleRepository
    {
        IEnumerable<Person> GetPeople(int serverId, string baseUrl, string searchKey);
    }

    public class PeopleRepository : IPeopleRepository
    {
        public PeopleRepository()
        {

        }

        public IEnumerable<Person> GetPeople(int serverId, string baseUrl, string searchKey)
        {
            throw new NotImplementedException();
        }
    }
}
