using System;

namespace West.Presence.CMA.Core.Models
{
    public class PeopleSettingsXML
    {
        public string SelectGroupsXML { get; set; }
        public string ExcludedUsersXML { get; set; }
        public string AttributesXML { get; set; }
    }

    
    [Serializable]
    public class PeopleSettings
    {
        public string SelectGroups { get; set; }
        public string ExcludedUser { get; set; }
        public string HiddenAttributres { get; set; }
        public int serverId { get; set; }
    }
}
