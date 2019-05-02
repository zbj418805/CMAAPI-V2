using System;
using System.Collections.Generic;
using System.Net.Http;
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
            /*
            //1.Get portlet Instance id
            int portletInstanceId = Utility.ToInteger(Database.ExecuteScalar("Mobile.Cma", "GetStaffDirectoryInstanceId", serverId));


            //2.Get portlet Instance Properties to [selectedGroups], [excludedUsers](Using Presence Class)
            PortletInstance ptlInstance = new PortletInstance(portletInstanceId);
            if (!Utility.IsPositiveInteger(ptlInstance.PortletInstanceId))
                return null;
            List<int> selectedGroups = new List<int>();
            List<int> excludedUsers = new List<int>();
            XmlDocument doc = new XmlDocument();
            if (ptlInstance.Properties.ContainsProperty("SelectedGroups"))
            {
                doc.LoadXml(ptlInstance.Properties["SelectedGroups"]);
                XmlNode node = doc.SelectSingleNode("SelectedGroups");
                if (Utility.ToBoolean(node.Attributes["visible"].Value))
                {
                    foreach (XmlNode groupNode in node.ChildNodes)
                    {
                        selectedGroups.Add(Utility.ToInteger(groupNode.Attributes["id"].Value));
                    }
                }
            }
            if (ptlInstance.Properties.ContainsProperty("ExcludedUsers"))
            {
                doc.LoadXml(ptlInstance.Properties["ExcludedUsers"]);
                XmlNode node = doc.SelectSingleNode("ExcludedUsers");
                foreach (XmlNode userNode in node.ChildNodes)
                {
                    excludedUsers.Add(Utility.ToInteger(userNode.Attributes["user_id"].Value));
                }
            }


            //5.Get all users to [dtSimpleUsers] with [selectedGroups]
            if (selectedGroups.Count == 0)
                return null;
            string groupIdsString = string.Join(",", selectedGroups.ToArray());
            DataTable dtSimpleUsers = Database.Execute("Mobile.Cma", "GetSimpleUserInfoList", groupIdsString);

            //6.Convert table [dtSimpleUsers] to list [spUsers] and remove users of [excludedUsers]
            List<SimplePerson> spUsers = new List<SimplePerson>();
            foreach (DataRow dr in dtSimpleUsers.Rows)
            {
                if (!excludedUsers.Contains(Utility.ToInteger(dr["user_id"])))
                {
                    spUsers.Add(new SimplePerson
                    {
                        userId = Utility.ToInteger(dr["user_id"]),
                        firstName = Utility.ToString(dr["first_names"]),
                        lastName = Utility.ToString(dr["last_name"]),
                        jobTitle = Utility.ToString(dr["job_title"])
                    });
                }
            }

            //7.Return result [dtSimpleUsers]
            return spUsers;
            */

            throw new NotImplementedException();
        }

        public IEnumerable<PersonInfo> GetPeopleInfo(string baseUrl, IEnumerable<Person> people)
        {
            string connectionStr = _dbConnectionService.GetConnection(baseUrl);
            throw new NotImplementedException();
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
