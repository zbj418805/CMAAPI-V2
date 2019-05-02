using System;
using System.Collections.Generic;
using System.Text;

namespace West.Presence.CMA.Core.Models
{
    [Serializable]
    public class Connection
    {
        public string baseUrl { get; set; }
        public string connectionString { get; set; }
    }
}
