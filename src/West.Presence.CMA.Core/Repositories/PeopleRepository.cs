using System;
using System.Collections.Generic;
using System.Net.Http;
using West.Presence.CMA.Core.Models;

namespace West.Presence.CMA.Core.Repositories
{
    public interface IPeopleRepository
    {
        IEnumerable<Person> GetPeople(int serverId, string baseUrl, string searchKey);
    }

    public class DBPeopleRepository : DBBaseRepository, IPeopleRepository
    {
        public DBPeopleRepository()
        {

        }

        public IEnumerable<Person> GetPeople(int serverId, string baseUrl, string searchKey)
        {
            throw new NotImplementedException();
        }
    }

    public class APIPeopleRepository : APIBaseRepository, IPeopleRepository
    {
        public APIPeopleRepository(IHttpClientFactory httpClientFactory) :
            base(httpClientFactory)
        {

        }

        public IEnumerable<Person> GetPeople(int serverId, string baseUrl, string searchKey)
        {
            return GetData<Person>(baseUrl + $"/presence/Api/CMA/People/{serverId}/{searchKey}");

        }

        public IEnumerable<PersonInfo> GetPeopleInfo(int serverId, string baseUrl, string searchKey)
        {
            return GetData<PersonInfo>(baseUrl + $"/presence/Api/CMA/People/{serverId}/{searchKey}");
        }
    }
}
