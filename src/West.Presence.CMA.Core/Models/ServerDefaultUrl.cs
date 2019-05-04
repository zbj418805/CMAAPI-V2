using System;
using System.Collections.Generic;
using System.Text;

namespace West.Presence.CMA.Core.Models
{
    [Serializable]
    public class ServerDefaultUrl
    {
        public string defaultUrl { get; set; }
        public int serverId { get; set; }
    }
}
