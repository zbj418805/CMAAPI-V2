using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;


namespace West.Presence.CMA.Core.Repositories
{
    public interface ISchoolsRepository
    {
        IEnumerable<School> GetSchools(int districtServerId, string baseUrl);
    }

    public class DBSchoolsRepository : ISchoolsRepository
    {
        IDatabaseProvider _databaseProvider;

        public DBSchoolsRepository(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        public IEnumerable<School> GetSchools(int districtServerId, string baseUrl)
        {
            var schools = _databaseProvider.GetData<School>("[dbo].[cma_server_get_v2]", new { district_server_id = districtServerId }, System.Data.CommandType.StoredProcedure);

            foreach(School s in schools)
            {
                var attrs = _databaseProvider.GetData<Attribute>("[dbo].[cma_server_attributes.get]", new { server_id = s.ServerId }, System.Data.CommandType.StoredProcedure);
                //s.Address = new Address()
                //{
                //    Address1 = attrs.Where(x=>x.)
                //}
            }

            return schools;
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
            return _httpClientProvider.GetData<School>($"{baseUrl}webapi/cma/schools/{districtServerId}");
        }
    }
}