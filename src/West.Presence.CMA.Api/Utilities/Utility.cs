using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace West.Presence.CMA.Api.Utilities
{
    public class Utility
    {
        protected Utility() { }

        public static string ApplicationName()
        {
            return AppDomain.CurrentDomain.FriendlyName.Replace(".exe", "");
        }
    }
}
