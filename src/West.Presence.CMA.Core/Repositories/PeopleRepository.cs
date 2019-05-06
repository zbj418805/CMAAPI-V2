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
        IDefaultUrlService _defaultUrlService;
        private readonly ILogger _logger = Log.ForContext<DBPeopleRepository>();

        public DBPeopleRepository(IDatabaseProvider databaseProvider, 
            IDBConnectionService dbConnectionService, 
            IPeopleSettingsService peopleSettingsService,
            IDefaultUrlService defaultUrlService)
        {
            _databaseProvider = databaseProvider;
            _dbConnectionService = dbConnectionService;
            _peopleSettingsService = peopleSettingsService;
            _defaultUrlService = defaultUrlService;
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
            var people = _databaseProvider.GetData<Person>(connectionStr, "[dbo].[cma_people_simple]", new
            { group_ids = peopleSetting.SelectGroups }, CommandType.StoredProcedure);

            //3.Convert table [dtSimpleUsers] to list [spUsers] and remove users of [excludedUsers]
            if (string.IsNullOrEmpty(peopleSetting.ExcludedUser))
            {
                foreach (Person p in people)
                    p.ServerId = serverId;
                return people;
            }
            else
            {
                var spUsers = new List<Person>();
                var lsExcludedUsers = peopleSetting.ExcludedUser.Split(',').Select(int.Parse);
                foreach (Person p in people)
                {
                    if (!lsExcludedUsers.Contains(p.UserId))
                    {
                        spUsers.Add(new Person
                        {
                            UserId = p.UserId,
                            FirstName = p.FirstName,
                            LastName = p.LastName,
                            JobTitle = p.JobTitle,
                            ServerId = serverId
                        });
                    }
                }
                return spUsers;
            }
        }

        public IEnumerable<PersonInfo> GetPeopleInfo(string baseUrl, IEnumerable<Person> people)
        {           
            string connectionStr = _dbConnectionService.GetConnection(baseUrl);

            string imageUrlFormat = "{0}common/pages/GalleryPhoto.aspx?photoId={1}&width=180&height=180";

            int[] userIds = people.Select(s => s.UserId).Distinct().ToArray();
            int[] userFromServerIds = people.Select(s => s.ServerId).Distinct().ToArray();
            string strUserIds = string.Join(",", userIds);
            //1. Load server information for [serverHiddenAttributes], [serverDefaultUrls]
            Dictionary<int, List<string>> serverHiddenAttributes = new Dictionary<int, List<string>>();
            Dictionary<int, string> serverDefaultUrls = new Dictionary<int, string>();
            foreach (int serverId in userFromServerIds) {
                PeopleSettings peopleSetting = _peopleSettingsService.GetPeopleSettings(serverId, baseUrl, connectionStr);
                serverHiddenAttributes.Add(serverId, peopleSetting.HiddenAttributres.Split(',').ToList());
                string defaultUrl = _defaultUrlService.GetDefaultUrl(serverId, baseUrl, connectionStr);
                if(!string.IsNullOrEmpty(defaultUrl))
                    serverDefaultUrls.Add(serverId, defaultUrl);
            }

            //2. Get data for all users details and attributes
            var peopleList = _databaseProvider.GetData<PersonInfo>(connectionStr, "[dbo].[cma_people_userinfo]", new { userIds = strUserIds }, CommandType.StoredProcedure);
            var peopleListAttributes = _databaseProvider.GetData<MAttribute>(connectionStr, "[dbo].[cma_people_attributes]", new { userIds = strUserIds }, CommandType.StoredProcedure);

            //3. Loop by [simplePersonList], add details to person to [personList]
            List <PersonInfo> resultPeople = new List<PersonInfo>();
            foreach (Person p in people)
            {
                //3.1 Add person details
                PersonInfo personInfo = peopleList.Where(x => x.UserId == p.UserId).FirstOrDefault();
                personInfo.ServerId = p.ServerId;
                if(personInfo.ImageUrl != null)
                    personInfo.ImageUrl = int.Parse(personInfo.ImageUrl) > 0 ? string.Format(imageUrlFormat, serverDefaultUrls[p.ServerId], int.Parse(personInfo.ImageUrl)) : "";

                if (!serverHiddenAttributes[p.ServerId].Contains("youtube"))
                    personInfo.YouTube = peopleListAttributes.Where(x => x.ObjectId == p.UserId && x.AttributeName == "youtube").Select(v=>v.AttributeValue).FirstOrDefault();

                if (!serverHiddenAttributes[p.ServerId].Contains("facebook"))
                    personInfo.Facebook = peopleListAttributes.Where(x => x.ObjectId == p.UserId && x.AttributeName == "facebook").Select(v => v.AttributeValue).FirstOrDefault();

                if (!serverHiddenAttributes[p.ServerId].Contains("twitter"))
                    personInfo.Twitter = peopleListAttributes.Where(x => x.ObjectId == p.UserId && x.AttributeName == "twitter").Select(v => v.AttributeValue).FirstOrDefault();
               
                if (!serverHiddenAttributes[p.ServerId].Contains("blog"))
                    personInfo.Blog = peopleListAttributes.Where(x => x.ObjectId == p.UserId && x.AttributeName == "blog").Select(v => v.AttributeValue).FirstOrDefault();

                if (!serverHiddenAttributes[p.ServerId].Contains("personal_message"))
                    personInfo.PersonalMessage = peopleListAttributes.Where(x => x.ObjectId == p.UserId && x.AttributeName == "personal_message").Select(v => v.AttributeValue).FirstOrDefault();

                if (!serverHiddenAttributes[p.ServerId].Contains("website"))
                    personInfo.Website = peopleListAttributes.Where(x => x.ObjectId == p.UserId && x.AttributeName == "website").Select(v => v.AttributeValue).FirstOrDefault();

                if (personInfo.Website!=null && personInfo.Website.StartsWith("/"))
                    personInfo.Website = serverDefaultUrls[p.ServerId] + personInfo.Website.Substring(1);

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
                p.ServerId = serverId;
            }
            return people;
        }

        public IEnumerable<PersonInfo> GetPeopleInfo(string baseUrl, IEnumerable<Person> people)
        {
            return _httpClientProvider.PostData<PersonInfo>($"{baseUrl}presence/api/cma/people", people, "PresenceApi");
        }
    }
}
