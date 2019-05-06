using System;

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
            return !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("VCAP_APPLICATION"));
        }
    }
}
