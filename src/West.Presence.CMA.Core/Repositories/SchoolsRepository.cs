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

    public class DBSchoolsRepository : DBBaseRepository, ISchoolsRepository
    {
        IDatabaseProvider _databaseProvider;

        public DBSchoolsRepository(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        public IEnumerable<School> GetSchools(int districtServerId, string baseUrl)
        {
            var schools = _databaseProvider.GetData<School>("[dbo].[cma_server.get]", new { server_id = districtServerId }, System.Data.CommandType.StoredProcedure);

            return schools;
            //// school icon
            //if (school.IconUrl != null && school.IconUrl.Length > 0)
            //    if (school.IconUrl.StartsWith("/"))
            //        school.IconUrl = school.Url + school.IconUrl.Substring(1);
            //    else
            //        school.IconUrl = school.Url + school.IconUrl;


            //if (serverId <= 0)
            //    return null;
            //using (var con = _dbConnectionFactory.CreateConnection())
            //{
            //    IEnumerable<SchoolInfo> empList = con.Query<SchoolInfo>("[dbo].[cma_server.get]", new { server_id = serverId }, commandType: CommandType.StoredProcedure).ToList();
            //    SchoolInfo schoolInfo = empList.ElementAtOrDefault(0);
            //    schoolInfo.contentyEntries = _cmaEntryRepo.GetAll(serverId);

            //    schoolInfo.ServerAttributes = _clickAttributeRepo.GetServerAttributes(serverId);
            //    return empList.ElementAtOrDefault(0);
            //}


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