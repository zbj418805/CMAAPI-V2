using System;
using System.Collections.Generic;
using System.Text;

namespace West.Presence.CMA.Core.Models
{
    [Serializable]
    public class Person
    {
        public int userId { get; set; }
        public string firstName{ get; set; }
        public string lastName { get; set; }
        public string jobTitle { get; set; }
        public int serverId { get; set; }
    }
}
