using System;
using System.Collections.Generic;
using System.Net.Http;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;


namespace West.Presence.CMA.Core.Repositories
{
    public interface ISchoolsRepository
    {
        IEnumerable<School> GetSchools(int districtServerId, string baseUrl);
    }

    public class SchoolsRepository : ISchoolsRepository
    {
        public SchoolsRepository()
        {

        }

        public IEnumerable<School> GetSchools(int districtServerId, string baseUrl)
        {
            throw new NotImplementedException();
        }
    }

    public class APISchoolsRepository : ISchoolsRepository
    {
        IHttpClientProvider _httpClientProvider;

        public APISchoolsRepository(IHttpClientProvider httpClientProvider) 
        {
            _httpClientProvider = httpClientProvider;
        }

        public IEnumerable<School> GetSchools(int districtServerId, string baseUrl)
        {
            return _httpClientProvider.GetData<School>(baseUrl + "/presence/Api/CMA/Schools/" + districtServerId);
        }
    }
}