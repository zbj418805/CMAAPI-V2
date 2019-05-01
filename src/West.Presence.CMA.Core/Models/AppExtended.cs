using System;
using System.Collections.Generic;

namespace West.Presence.CMA.Core.Models
{
    public class AppSettings
    {
        public int dictrictServerId { get; set; }
        public int appId { get; set; }
        public string endpoint { get; set; }
        public string sessionId { get; set; }
        public DateTime lastModified { get; set; }
        public IEnumerable<Channel2Group> Channel2Groups { get; set; }
    }
}
