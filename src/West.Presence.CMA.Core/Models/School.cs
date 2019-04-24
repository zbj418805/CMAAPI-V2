using System;
using System.Collections.Generic;
using System.Text;

namespace West.Presence.CMA.Core.Models
{
    public class School
    {
        public int districtServerId { get; set; }
        public int serverId { get; set; }
        public int defaultTimeZoneId { get; set; }
        public string serverName { get; set; }
        public string serverDescription { get; set; }
        public string geoLat { get; set; }
        public string geoLong { get; set; }
        public string url { get; set; }
        public string defaultLocal { get; set; }
    }
}
