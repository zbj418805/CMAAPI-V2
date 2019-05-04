using System;

namespace West.Presence.CMA.Core.Models
{
    [Serializable]
    public class DefaultUrl
    {
        public int serverId { get; set; }
        public string defaultUrl { get; set; }
    }
}
