using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Servies;
using Serilog;

namespace West.Presence.CMA.Core.Repositories
{
    public interface IPeopleRepository
    {
        IEnumerable<Person> GetPeople(int serverId, string baseUrl);
        IEnumerable<PersonInfo> GetPeopleInfo(string baseUrl, IEnumerable<Person> people);
    }

    public class DBPeopleRepository : IPeopleRepository
    {
        IDatabaseProvider _databaseProvider;
        IDBConnectionService _dbConnectionService;
        IPeopleSettingsService _peopleSettingsService;
        private readonly ILogger _logger = Log.ForContext<DBPeopleRepository>();

        public DBPeopleRepository(IDatabaseProvider databaseProvider, 
            IDBConnectionService dbConnectionService, 
            IPeopleSettingsService peopleSettingsService)
        {
            _databaseProvider = databaseProvider;
            _dbConnectionService = dbConnectionService;
            _peopleSettingsService = peopleSettingsService;
        }

        public IEnumerable<Person> GetPeople(int serverId, string baseUrl)
        {
            string connectionStr = _dbConnectionService.GetConnection(baseUrl);

            if (string.IsNullOrEmpty(connectionStr))
            {
                _logger.Error($"Get connection string based on {baseUrl} failed.");
                return null;
            }

            //1.Get portlet Instance Properties to [selectedGroups], [excludedUsers], [HiddenAttributes]
            PeopleSettings peopleSetting = _peopleSettingsService.GetPeopleSettings(serverId, baseUrl, connectionStr);

            //2.Get all users with [selectedGroups]
            if (peopleSetting == null || string.IsNullOrEmpty(peopleSetting.SelectGroups))
            {
                _logger.Error($"No selected groups found based on {baseUrl}.");
                return new List<Person>();
            }
            var people = _databaseProvider.GetData<Person>(connectionStr, "[dbo].[staff_directory_get_basic_users_info_by_groups]", new
            { group_ids = peopleSetting.SelectGroups }, CommandType.StoredProcedure);

            //3.Convert table [dtSimpleUsers] to list [spUsers] and remove users of [excludedUsers]
            if (string.IsNullOrEmpty(peopleSetting.ExcludedUser))
            {
                foreach (Person p in people)
                    p.serverId = serverId;
                return people;
            }
            else
            {
                var spUsers = new List<Person>();
                var lsExcludedUsers = peopleSetting.ExcludedUser.Split(',').Select(int.Parse);
                foreach (Person p in people)
                {
                    if (!lsExcludedUsers.Contains(p.userId))
                    {
                        spUsers.Add(new Person
                        {
                            userId = p.userId,
                            firstName = p.firstName,
                            lastName = p.lastName,
                            jobTitle = p.jobTitle,
                            serverId = serverId
                        });
                    }
                }
                return spUsers;
            }
        }

        public IEnumerable<PersonInfo> GetPeopleInfo(string baseUrl, IEnumerable<Person> people)
        {
            string sqlscrpt_GetDefaultURL = "SELECT TOP 1 url FROM click_server_urls WHERE default_p=1 AND server_id=@serverId";
            string sqlscrpt_GetUsersDetails = @"SELECT
usr.user_id as Id ,   
per.first_names as FirstName,    
per.last_name as LastName, 
ext.job_title as JobTitle,    
ext.work_phone as PhoneNumber, 
ext.description as Description,    
ext.priv_profile_picture as ImageUrl , 
par.email as Email      
FROM       users                 AS usr WITH (NOLOCK)
INNER JOIN persons               AS per WITH (NOLOCK)  ON usr.user_id=per.person_id
INNER JOIN parties               AS par WITH (NOLOCK)  ON usr.user_id=par.party_id
LEFT  JOIN user_extended         AS ext WITH (NOLOCK)  ON usr.user_id=ext.user_id
WHERE      usr.user_id IN (SELECT value FROM string_split(@userIds, ','))";

            string sqlscript_GetUsersAttributes = @"SELECT  
   AV.[object_id] AS objectId
  ,CA.[attribute_name] AS attributeName
  ,AV.[attr_value] AS attributeValue
  ,CA.[attribute_id] as attributeId
  ,CA.[category] AS category

  FROM         click_attribute_values AS AV WITH (NOLOCK)
INNER JOIN    click_attributes       AS CA WITH (NOLOCK) ON CA.[attribute_id] = AV.[attribute_id] 
                                                         AND CA.[category] IN ('Social Media','User Profile')                  
WHERE   CA.enable_p = 1
  AND   AV.object_id IN (SELECT value FROM string_split(@userIds, ','))
  AND   CA.parent_id = 0";

            string connectionStr = _dbConnectionService.GetConnection(baseUrl);

            string imageUrlFormat = "{0}common/pages/GalleryPhoto.aspx?photoId={1}&width=180&height=180";

            int[] userIds = people.Select(s => s.userId).Distinct().ToArray();
            int[] userFromServerIds = people.Select(s => s.serverId).Distinct().ToArray();
            string strUserIds = string.Join(",", userIds);
            //1. Load server information for [serverHiddenAttributes], [serverDefaultUrls]
            Dictionary<int, List<string>> serverHiddenAttributes = new Dictionary<int, List<string>>();
            Dictionary<int, string> serverDefaultUrls = new Dictionary<int, string>();
            foreach (int serverId in userFromServerIds) {
                PeopleSettings peopleSetting = _peopleSettingsService.GetPeopleSettings(serverId, baseUrl, connectionStr);
                serverHiddenAttributes.Add(serverId, peopleSetting.HiddenAttributres.Split(',').ToList());
                serverDefaultUrls.Add(serverId, _databaseProvider.GetCellValue<string>(connectionStr, sqlscrpt_GetDefaultURL, new { serverId = serverId }, CommandType.Text));
            }

            //2. Get data for all users details and attributes
            var peopleList = _databaseProvider.GetData<PersonInfo>(connectionStr, sqlscrpt_GetUsersDetails, new { userIds = strUserIds }, CommandType.Text);
            var peopleListAttributes = _databaseProvider.GetData<MAttribute>(connectionStr, sqlscript_GetUsersAttributes, new { userIds = strUserIds }, CommandType.Text);

            //3. Loop by [simplePersonList], add details to person to [personList]
            List<PersonInfo> resultPeople = new List<PersonInfo>();
            foreach (Person p in people)
            {
                //3.1 Add person details
                PersonInfo personInfo = peopleList.Where(x => x.id == p.userId).FirstOrDefault();
                personInfo.serverId = p.serverId;
                if(personInfo.imageUrl != null)
                    personInfo.imageUrl = int.Parse(personInfo.imageUrl) > 0 ? string.Format(imageUrlFormat, serverDefaultUrls[p.serverId], int.Parse(personInfo.imageUrl)) : "";

                if (!serverHiddenAttributes[p.serverId].Contains("youtube"))
                    personInfo.youTube = peopleListAttributes.Where(x => x.objectId == p.userId && x.attributeName == "youtube").Select(v=>v.attributeValue).FirstOrDefault();

                if (!serverHiddenAttributes[p.serverId].Contains("facebook"))
                    personInfo.facebook = peopleListAttributes.Where(x => x.objectId == p.userId && x.attributeName == "facebook").Select(v => v.attributeValue).FirstOrDefault();

                if (!serverHiddenAttributes[p.serverId].Contains("twitter"))
                    personInfo.twitter = peopleListAttributes.Where(x => x.objectId == p.userId && x.attributeName == "twitter").Select(v => v.attributeValue).FirstOrDefault();

               
                if (!serverHiddenAttributes[p.serverId].Contains("blog"))
                    personInfo.blog = peopleListAttributes.Where(x => x.objectId == p.userId && x.attributeName == "blog").Select(v => v.attributeValue).FirstOrDefault();

                if (!serverHiddenAttributes[p.serverId].Contains("personal_message"))
                    personInfo.personalMessage = peopleListAttributes.Where(x => x.objectId == p.userId && x.attributeName == "personal_message").Select(v => v.attributeValue).FirstOrDefault();

                if (!serverHiddenAttributes[p.serverId].Contains("website"))
                    personInfo.website = peopleListAttributes.Where(x => x.objectId == p.userId && x.attributeName == "website").Select(v => v.attributeValue).FirstOrDefault();

                if (personInfo.website!=null && personInfo.website.StartsWith("/"))
                    personInfo.website = serverDefaultUrls[p.serverId] + personInfo.website.Substring(1);

                resultPeople.Add(personInfo);
            }

            return resultPeople;
        }
    }

    public class APIPeopleRepository : IPeopleRepository
    {
        IHttpClientProvider _httpClientProvider;

        public APIPeopleRepository(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
        }

        public IEnumerable<Person> GetPeople(int serverId, string baseUrl)
        {
            var people = _httpClientProvider.GetData<Person>($"{baseUrl}presence/api/cma/people/{serverId}", "PresenceApi");
            foreach(Person p in people)
            {
                p.serverId = serverId;
            }

            return people;
        }

        public IEnumerable<PersonInfo> GetPeopleInfo(string baseUrl, IEnumerable<Person> people)
        {
            return _httpClientProvider.PostData<PersonInfo>($"{baseUrl}presence/api/cma/people", people, "PresenceApi");
        }
    }
}
