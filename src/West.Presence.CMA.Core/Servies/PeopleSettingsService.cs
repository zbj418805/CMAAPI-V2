using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Repositories;


namespace West.Presence.CMA.Core.Servies
{
    public interface IPeopleSettingsService
    {
        PeopleSettings GetPeopleSettings(int serverId, string baseUrl, string connectionStr);
    }
    public class PeopleSettingsService : IPeopleSettingsService
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly IOptions<CMAOptions> _options;
        private readonly IPeopleSettingsRepository _peopleSettingsRepository;

        public PeopleSettingsService(ICacheProvider cacheProvider, IOptions<CMAOptions> options, IPeopleSettingsRepository peopleSettingsRepository)
        {
            _cacheProvider = cacheProvider;
            _options = options;
            _peopleSettingsRepository = peopleSettingsRepository;
        }

        public PeopleSettings GetPeopleSettings(int serverId, string baseUrl, string connectionStr)
        {
            Uri u = new Uri(baseUrl);
            int cacheDuration = _options.Value.CachePeopleSettingsDurationInSeconds;
            string cacheKey = $"{_options.Value.CachePeopleSettingsKey}_{u.Host}";
            PeopleSettings settings;
            IEnumerable<PeopleSettings> peopleSettings;
            if (!_cacheProvider.TryGetValue<IEnumerable<PeopleSettings>>(cacheKey, out peopleSettings))
            {
                //Not exists
                settings = _peopleSettingsRepository.GetPeopleSettings(serverId, connectionStr);
                if (settings != null)
                {
                    AddToCache(cacheKey, cacheDuration, settings, new List<PeopleSettings>());
                }
            }
            else
            {
                //Cache Exists
                settings = peopleSettings.Where(x => x.serverId == serverId).FirstOrDefault();
                if (settings == null)
                {
                    settings = _peopleSettingsRepository.GetPeopleSettings(serverId, connectionStr);
                    if (settings != null)
                    {
                        AddToCache(cacheKey, cacheDuration, settings, peopleSettings.ToList());
                    }
                }
            }

            return settings;
        }

        private void AddToCache(string key, int duration, PeopleSettings setting, List<PeopleSettings> list)
        {
            list.Add(setting);
            _cacheProvider.Add(key, list, duration);
        }
    }
}
