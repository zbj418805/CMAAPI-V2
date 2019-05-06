using System;
using System.Collections.Generic;
using System.Text;

namespace West.Presence.CMA.Core.Models
{
    [Serializable]
    public class Person
    {
        public int UserId { get; set; }
        public string FirstName{ get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public int ServerId { get; set; }
    }
}
