using System;
using System.Collections.Generic;
using System.Text;

namespace West.Presence.CMA.Core.Models
{
    public class CMAOptions
    {
        public string ConnectionString { get; set; }
        public string CacheNewsKey { get; set; }
        public string CachePeopleKey { get; set; }
        public string CacheEventKey { get; set; }
        public string CacheSchoolKey { get; set; }
    }
}
