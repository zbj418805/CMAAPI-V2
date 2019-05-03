using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Xml;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Servies;

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

        public DBPeopleRepository(IDatabaseProvider databaseProvider, IDBConnectionService dbConnectionService)
        {
            _databaseProvider = databaseProvider;
            _dbConnectionService = dbConnectionService;
        }

        public IEnumerable<Person> GetPeople(int serverId, string baseUrl)
        {
            string connectionStr = _dbConnectionService.GetConnection(baseUrl);
            
            //1.Get portlet Instance Properties to [selectedGroups], [excludedUsers]
            PortletSettings portletSettings = _databaseProvider.GetData<PortletSettings>(connectionStr, "[dbo].[staff_directory_get_settings_v2]",
                new { server_id = serverId }, CommandType.StoredProcedure).FirstOrDefault();

            if (portletSettings == null)
                return null;

            string selectGroups = ExtractListFromXML(portletSettings.SelectGroups, "SelectedGroups", true, "id");
            string excludedUsers = ExtractListFromXML(portletSettings.ExcludedUsers, "ExcludedUsers", false, "user_id");

            //2.Get all users with [selectedGroups]
            if (string.IsNullOrEmpty(selectGroups))
                return null;

            var people = _databaseProvider.GetData<Person>(connectionStr, "[dbo].[staff_directory_get_basic_users_info_by_groups]", new
            { group_ids = selectGroups }, CommandType.StoredProcedure);

            //3.Convert table [dtSimpleUsers] to list [spUsers] and remove users of [excludedUsers]
            if (string.IsNullOrEmpty(excludedUsers))
            {
                foreach (Person p in people)
                    p.serverId = serverId;
                return people;
            }
            else
            {
                var spUsers = new List<Person>();
                var lsExcludedUsers = excludedUsers.Split(',').Select(int.Parse);
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
            string connectionStr = _dbConnectionService.GetConnection(baseUrl);
            throw new NotImplementedException();
        }

        private string ExtractListFromXML(string xmlString, string nodeName, bool checkVisible,  string key)
        {
            if (string.IsNullOrEmpty(xmlString))
                return string.Empty;
            string listString ="";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            XmlNode node = doc.SelectSingleNode(nodeName);
            if (checkVisible && node.Attributes["visible"].Value == "True")
            {
                foreach (XmlNode groupNode in node.ChildNodes)
                {
                    listString += groupNode.Attributes[key].Value + ",";
                }
            }
            if (!string.IsNullOrEmpty(listString))
            {
                listString = listString.Substring(0, listString.Length - 1);
            }

            return listString;
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
