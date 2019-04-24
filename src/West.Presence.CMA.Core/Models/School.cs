using System;
using System.Collections.Generic;
using System.Text;

namespace West.Presence.CMA.Core.Models
{
    [Serializable]
    public class School
    {
        public int DistrictServerId { get; set; }
        public int ServerId { get; set; }
        public int TimezoneId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Phone { get; set; }
        public string IconUrl { get; set; }
        public Address Address { get; set; }
    }

    [Serializable]
    public class Address
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
    }
}
