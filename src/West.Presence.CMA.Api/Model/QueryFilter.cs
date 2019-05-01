using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace West.Presence.CMA.Api.Model
{
    public class QueryFilter
    {
        public string search { get; set; }
        public DateTime starttime { get; set; }
        public DateTime endtime { get; set; }
        public int? parent { get; set; }
        public string channels { get; set; }
        public List<int> channelServerIds { get; set; }
        public int categories { get; set; }
    }
}
