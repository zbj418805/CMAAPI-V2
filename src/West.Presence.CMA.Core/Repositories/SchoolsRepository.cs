using System;
using System.Collections.Generic;
using System.Net.Http;
using West.Presence.CMA.Core.Models;


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
        public APISchoolsRepository(IHttpClientFactory httpClientFactory) :
            base(httpClientFactory)
        {

        }

        public IEnumerable<School> GetSchools(int districtServerId, string baseUrl)
        {
            return GetData<School>(baseUrl + "/presence/Api/CMA/Schools/" + districtServerId);
        }
    }
}