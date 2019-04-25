using System;
using System.Collections.Generic;
using System.Text;

namespace West.Presence.CMA.Core.Models
{
    [Serializable]
    public class Event
    {
        public int EventId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime StartTimeUTC { get; set; }
        public DateTime EndTimeUTC { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAllDayEvent { get; set; }
        public int ServerId { get; set; }
    }
}
