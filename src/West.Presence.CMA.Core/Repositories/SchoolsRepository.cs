using System;
using System.Collections.Generic;
using System.Net.Http;
using West.Presence.CMA.Core.Models;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace West.Presence.CMA.Core.Repositories
{
    public interface ISchoolsRepository
    {
        IEnumerable<School> GetSchools(int districtServerId, string baseUrl);
    }

    public class SchoolsRepository : DBBaseRepository, ISchoolsRepository
    {
        public SchoolsRepository()
        {

        }

        public IEnumerable<School> GetSchools(int districtServerId, string baseUrl)
        {
            throw new NotImplementedException();
        }
    }

    public class APISchoolsRepository : APIBaseRepository, ISchoolsRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public APISchoolsRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IEnumerable<School> GetSchools(int districtServerId, string baseUrl)
        {
            using(var client = _httpClientFactory.CreateClient("PresnceApi"))
            {
                string content =  client.GetStringAsync(baseUrl+"/presence/Api/CMA/Schools/"+ districtServerId).Result;
                return JsonConvert.DeserializeObject<List<School>>(content);
            }
        }
    }
}