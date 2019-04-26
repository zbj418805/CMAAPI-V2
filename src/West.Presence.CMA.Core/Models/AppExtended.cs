using System;
using System.Collections.Generic;
using System.Text;

namespace West.Presence.CMA.Core.Models
{
    public class AppExtended
    {
        public int dictrictServerId { get; set; }
        public int appId { get; set; }
        public string endpoint { get; set; }
        public string sessionId { get; set; }
        public DateTime lastModified { get; set; }
    }
}
