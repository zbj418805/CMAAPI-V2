using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace West.Presence.CMA.Api.Model
{
    public class QueryFilter
    {
        public string Search { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? Parent { get; set; }
        public string Channels { get; set; }
        public List<int> ChannelServerIds { get; set; }
        public int Categories { get; set; }
    }
}
