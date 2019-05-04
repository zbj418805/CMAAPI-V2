using System.Data;
using System.Linq;
using System.Xml;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using Serilog;

namespace West.Presence.CMA.Core.Repositories
{
    public interface IPeopleSettingsRepository
    {
        PeopleSettings GetPeopleSettings(int serverId, string connectionStr);
    }


    public class PeopleSettingsRepository : IPeopleSettingsRepository
    {
        IDatabaseProvider _databaseProvider;
        private readonly ILogger _logger = Log.ForContext<PeopleSettingsRepository>();

        public PeopleSettingsRepository(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        public PeopleSettings GetPeopleSettings(int serverId, string connectionStr)
        {
            if (string.IsNullOrEmpty(connectionStr) || serverId <= 0)
            {
                _logger.Error($"connection is emptry or serverId is not valid: connectionStrin : {connectionStr}, serverId: {serverId}");
                return null;
            }
                       
            PeopleSettingsXML portletSettingsXml = _databaseProvider.GetData<PeopleSettingsXML>(connectionStr, "[dbo].[staff_directory_get_settings_v2]",
                new { server_id = serverId }, CommandType.StoredProcedure).FirstOrDefault();

            if (portletSettingsXml == null)
            {
                _logger.Error($"staff directory no settings found.");
                return null;
            }

            PeopleSettings setting = new PeopleSettings();
            setting.SelectGroups = ExtractListFromXML(portletSettingsXml.SelectGroupsXML, "SelectedGroups", true, false, "id");
            setting.ExcludedUser = ExtractListFromXML(portletSettingsXml.ExcludedUsersXML, "ExcludedUsers", false, false, "user_id");
            setting.HiddenAttributres = ExtractListFromXML(portletSettingsXml.AttributesXML, "Attributes", false, true, "key");
            setting.serverId = serverId;
            return setting;
        }


        private string ExtractListFromXML(string xmlString, string nodeName, bool checkRootVisible, bool checkNodeVisible, string key)
        {
            if (string.IsNullOrEmpty(xmlString))
                return string.Empty;
            string listString = "";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            XmlNode node = doc.SelectSingleNode(nodeName);
            if (checkRootVisible && node.Attributes["visible"].Value == "True")
            {
                foreach (XmlNode groupNode in node.ChildNodes)
                {
                    if (checkNodeVisible && groupNode.Attributes["visible"].Value == "False")
                        listString += groupNode.Attributes[key].Value + ",";
                    else
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
}
