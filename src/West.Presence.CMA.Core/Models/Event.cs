using System;
using System.Collections.Generic;
using System.Text;

namespace West.Presence.CMA.Core.Models
{
    [Serializable]
    public class Event
    {
        public string name { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
    }
}
