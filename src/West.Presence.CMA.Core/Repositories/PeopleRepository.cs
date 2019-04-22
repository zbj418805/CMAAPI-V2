using Newtonsoft.Json;
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

    public class PeopleRepository : DBBaseRepository, IPeopleRepository
    {
        public PeopleRepository()
        {

        }

        public IEnumerable<Person> GetPeople(int serverId, string baseUrl, string searchKey)
        {
            throw new NotImplementedException();
        }
    }

    public class APIPeopleRepository : APIBaseRepository, IPeopleRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public APIPeopleRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IEnumerable<Person> GetPeople(int serverId, string baseUrl, string searchKey)
        {
            using (var client = _httpClientFactory.CreateClient("PresnceApi"))
            {
                string content = client.GetStringAsync(baseUrl + $"/presence/Api/CMA/People/{serverId}/{searchKey}").Result;
                return JsonConvert.DeserializeObject<List<Person>>(content);
            }
        }

        public IEnumerable<PersonInfo> GetPeopleInfo(int serverId, string baseUrl, string searchKey)
        {
            using (var client = _httpClientFactory.CreateClient("PresnceApi"))
            {
                string content = client.GetStringAsync(baseUrl + $"/presence/Api/CMA/PeopleInfo/{serverId}/{searchKey}").Result;
                return JsonConvert.DeserializeObject<List<PersonInfo>>(content);
            }
        }
    }
}
