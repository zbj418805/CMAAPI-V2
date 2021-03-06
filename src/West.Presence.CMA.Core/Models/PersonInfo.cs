﻿using System;
using System.Collections.Generic;
using System.Text;

namespace West.Presence.CMA.Core.Models
{
    public class PersonInfo : Person
    {
        public string Description { get; set; }
        public string Blog { get; set; }
        public string PersonalMessage { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public string ImageUrl { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string YouTube { get; set; }
    }
}
