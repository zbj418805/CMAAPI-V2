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

        public static bool IsPcf() {
            //var env = Environment.GetEnvironmentVariables("VCAP_APPLICATION");
            //return !string.IsNullOrWhiteSpace(env);

            return false;
        }
    }
}
