using System;
using System.Collections.Generic;
using System.Text;

namespace West.Presence.CMA.Core.Models
{
    public class PersonInfo : Person
    {
        public int id { get; set; }
        public string description { get; set; }
        public string blog { get; set; }
        public string personalMessage { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string website { get; set; }
        public string imageUrl { get; set; }
        public string twitter { get; set; }
        public string facebook { get; set; }
        public string youTube { get; set; }
    }
}
