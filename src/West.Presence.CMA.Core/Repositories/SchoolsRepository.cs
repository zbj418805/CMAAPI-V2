using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Servies;

namespace West.Presence.CMA.Core.Repositories
{
    public interface ISchoolsRepository
    {
        IEnumerable<School> GetSchools(string baseUrl);
    }

    public class DBSchoolsRepository : ISchoolsRepository
    {
        IDatabaseProvider _databaseProvider;
        IDBConnectionService _dbConnectionService;

        public DBSchoolsRepository(IDatabaseProvider databaseProvider, IDBConnectionService dbConnectionService)
        {
            _databaseProvider = databaseProvider;
            _dbConnectionService = dbConnectionService;
        }

        public IEnumerable<School> GetSchools(string baseUrl)
        {
            string connectionStr = _dbConnectionService.GetConnection(baseUrl);

            int serverId = _databaseProvider.GetCellValue<int>(connectionStr, "SELECT server_id FROM click_server_urls WHERE url = @url ", new { url = baseUrl }, CommandType.Text);

            var schools = _databaseProvider.GetData<School>(connectionStr, "[dbo].[cma_server_get_v2]", new { district_server_id = serverId }, CommandType.StoredProcedure);

            foreach(School s in schools)
            {
                var atts = _databaseProvider.GetData<MAttribute>(connectionStr, "[dbo].[cma_server_attributes.get]", new { server_id = s.ServerId }, System.Data.CommandType.StoredProcedure);
                s.Address = new Address()
                {
                    Address1 = atts.Where(a => a.attributeName == "org_address1").Select(x => x.attributeValue).FirstOrDefault(),
                    Address2 = atts.Where(a => a.attributeName == "org_address2").Select(x => x.attributeValue).FirstOrDefault(),
                    City = atts.Where(a => a.attributeName == "org_city").Select(x => x.attributeValue).FirstOrDefault(),
                    Country = atts.Where(a => a.attributeName == "org_postal").Select(x => x.attributeValue).FirstOrDefault(),
                    PostCode = atts.Where(a => a.attributeName == "org_postal").Select(x => x.attributeValue).FirstOrDefault(),
                    Province = atts.Where(a => a.attributeName == "org_province").Select(x => x.attributeValue).FirstOrDefault()
                };

                s.Phone = atts.Where(a => a.attributeName == "org_phone").Select(x => x.attributeValue).FirstOrDefault();
                s.Slogan = atts.Where(a => a.attributeName == "org_slogan").Select(x => x.attributeValue).FirstOrDefault();
                s.Fax = atts.Where(a => a.attributeName == "org_fax").Select(x => x.attributeValue).FirstOrDefault();
                s.Facebook = atts.Where(a => a.attributeName == "org_facebook_website").Select(x => x.attributeValue).FirstOrDefault();
                s.Twitter = atts.Where(a => a.attributeName == "org_twitter_website").Select(x => x.attributeValue).FirstOrDefault();
                s.Youtube = atts.Where(a => a.attributeName == "org_youtube_channel").Select(x => x.attributeValue).FirstOrDefault();
                s.Email = atts.Where(a => a.attributeName == "org_email_address").Select(x => x.attributeValue).FirstOrDefault();
                s.Total = schools.Count();
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

        public IEnumerable<School> GetSchools(string baseUrl)
        {
            return _httpClientProvider.GetData<School>($"{baseUrl}webapi/cma/schools", "PresenceApi");
        }
    }
}