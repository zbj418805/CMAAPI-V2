using System;
using System.Collections.Generic;
using System.Net.Http;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;

namespace West.Presence.CMA.Core.Repositories
{
    public interface IPeopleRepository
    {
        IEnumerable<Person> GetPeople(int serverId, string baseUrl, string searchKey);

        IEnumerable<PersonInfo> GetPeopleInfo(string baseUrl, IEnumerable<Person> people);
    }

    public class DBPeopleRepository : IPeopleRepository
    {
        public DBPeopleRepository()
        {

        }

        public IEnumerable<Person> GetPeople(int serverId, string baseUrl, string searchKey)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PersonInfo> GetPeopleInfo(string baseUrl, IEnumerable<Person> people)
        {
            throw new NotImplementedException();
        }
    }

    public class APIPeopleRepository : IPeopleRepository
    {
        IHttpClientProvider _httpClientProvider;

        public APIPeopleRepository(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
        }

        public IEnumerable<Person> GetPeople(int serverId, string baseUrl, string searchKey)
        {
            return _httpClientProvider.GetData<Person>(baseUrl + $"/presence/Api/CMA/People/{serverId}/{searchKey}");
        }

        public IEnumerable<PersonInfo> GetPeopleInfo(string baseUrl, IEnumerable<Person> people)
        {
            return _httpClientProvider.PostData<PersonInfo>(baseUrl + $"/presence/Api/CMA/People", people);
        }
    }
}
