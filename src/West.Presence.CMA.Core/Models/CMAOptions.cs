using System;
using System.Collections.Generic;
using System.Text;

namespace West.Presence.CMA.Core.Models
{
    public class CMAOptions
    {
        public string ConnectionString { get; set; }
        public string PresenceAccessToken { get; set; }
        public string CentralServiceUrl { get; set; }
        public string CacheConnectionKey { get; set; }
        public string CacheNewsKey { get; set; }
        public string CachePeopleKey { get; set; }
        public string CacheEventsKey { get; set; }
        public string CacheSchoolsKey { get; set; }
        public string CachePeopleSettingsKey { get; set; }
        public string CacheDefaultUrlsKey { get; set; }
        public int CacheConnectionDurationInSeconds { get; set; }
        public int CacheNewsDurationInSeconds { get; set; }
        public int CachePeopleDurationInSeconds { get; set; }
        public int CacheEventsDurationInSeconds { get; set; }
        public int CacheSchoolsDurationInSeconds { get; set; }
        public int CachePeopleSettingsDurationInSeconds { get; set; }
        public int CacheDefaultUrlsDurationInSceconds { get; set; }
    }
}
