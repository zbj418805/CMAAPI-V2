﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Servies;
using Serilog;

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
        private readonly ILogger _logger = Log.ForContext<DBSchoolsRepository>();

        public DBSchoolsRepository(IDatabaseProvider databaseProvider, IDBConnectionService dbConnectionService)
        {
            _databaseProvider = databaseProvider;
            _dbConnectionService = dbConnectionService;
        }

        public IEnumerable<School> GetSchools(string baseUrl)
        {
            string connectionStr = _dbConnectionService.GetConnection(baseUrl);

            if (string.IsNullOrEmpty(connectionStr))
            {
                _logger.Error($"Get connection string based on {baseUrl} failed.");
                return null;
            }
            int serverId = _databaseProvider.GetCellValue<int>(connectionStr, "SELECT TOP 1 server_id FROM click_server_urls WHERE url = @url", new { url = baseUrl }, CommandType.Text);

            if (serverId <= 0)
            {
                _logger.Error($"No server_id found based on {baseUrl}.");
                return null;
            }
            var schools = _databaseProvider.GetData<School>(connectionStr, "[dbo].[cma_server_v2]", new { district_server_id = serverId }, CommandType.StoredProcedure);

            if (schools.Count() == 0)
            {
                _logger.Information($"No school found by based on {baseUrl} : {serverId}");
                return schools;
            }

            foreach(School s in schools)
            {
                var atts = _databaseProvider.GetData<MAttribute>(connectionStr, "[dbo].[cma_server_attributes.get]", new { server_id = s.ServerId }, CommandType.StoredProcedure);
                s.Address = new Address()
                {
                    Address1 = atts.Where(a => a.AttributeName == "org_address1").Select(x => x.AttributeValue).FirstOrDefault(),
                    Address2 = atts.Where(a => a.AttributeName == "org_address2").Select(x => x.AttributeValue).FirstOrDefault(),
                    City = atts.Where(a => a.AttributeName == "org_city").Select(x => x.AttributeValue).FirstOrDefault(),
                    Country = atts.Where(a => a.AttributeName == "org_country").Select(x => x.AttributeValue).FirstOrDefault(),
                    PostCode = atts.Where(a => a.AttributeName == "org_postal").Select(x => x.AttributeValue).FirstOrDefault(),
                    Province = atts.Where(a => a.AttributeName == "org_province").Select(x => x.AttributeValue).FirstOrDefault()
                };

                s.Phone = atts.Where(a => a.AttributeName == "org_phone").Select(x => x.AttributeValue).FirstOrDefault();
                s.Slogan = atts.Where(a => a.AttributeName == "org_slogan").Select(x => x.AttributeValue).FirstOrDefault();
                s.Fax = atts.Where(a => a.AttributeName == "org_fax").Select(x => x.AttributeValue).FirstOrDefault();
                s.Facebook = atts.Where(a => a.AttributeName == "org_facebook_website").Select(x => x.AttributeValue).FirstOrDefault();
                s.Twitter = atts.Where(a => a.AttributeName == "org_twitter_website").Select(x => x.AttributeValue).FirstOrDefault();
                s.Youtube = atts.Where(a => a.AttributeName == "org_youtube_channel").Select(x => x.AttributeValue).FirstOrDefault();
                s.Email = atts.Where(a => a.AttributeName == "org_email_address").Select(x => x.AttributeValue).FirstOrDefault();
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