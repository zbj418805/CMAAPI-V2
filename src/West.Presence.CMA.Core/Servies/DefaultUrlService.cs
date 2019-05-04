using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Repositories;

namespace West.Presence.CMA.Core.Servies
{
    public interface IDefaultUrlService
    {
        string GetDefaultUrl(int serverId, string baseUrl, string connectionStr);
    }
    public class DefaultUrlService : IDefaultUrlService
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly IOptions<CMAOptions> _options;
        private readonly IDefaultUrlRepository _defaultUrlRepository;

        public DefaultUrlService(ICacheProvider cacheProvider, IOptions<CMAOptions> options, IDefaultUrlRepository defaultUrlRepository)
        {
            _cacheProvider = cacheProvider;
            _options = options;
            _defaultUrlRepository = defaultUrlRepository;
        }

        public string GetDefaultUrl(int serverId, string baseUrl, string connectionStr)
        {
            Uri u = new Uri(baseUrl);
            int cacheDuration = _options.Value.CacheDefaultUrlsDurationInSceconds;
            string cacheKey = $"{_options.Value.CacheDefaultUrlsKey}_{u.Host}";
            string defaultUrl = "";
            IEnumerable<ServerDefaultUrl> serverUrls;

            if (!_cacheProvider.TryGetValue<IEnumerable<ServerDefaultUrl>>(cacheKey, out serverUrls))
            {
                //Not exists
                defaultUrl = _defaultUrlRepository.GetDefaultUrl(serverId, connectionStr);
                if (!string.IsNullOrEmpty(defaultUrl))
                {
                    AddToCache(cacheKey, cacheDuration,
                            new ServerDefaultUrl() { defaultUrl = defaultUrl, serverId = serverId },
                            new List<ServerDefaultUrl>());
                }
            }
            else
            {
                //Cache Exists
                defaultUrl = serverUrls.Where(x => x.serverId == serverId).Select(y=>y.defaultUrl).FirstOrDefault();
                if (string.IsNullOrEmpty(defaultUrl))
                {
                    defaultUrl = _defaultUrlRepository.GetDefaultUrl(serverId, connectionStr);
                    if (!string.IsNullOrEmpty(defaultUrl))
                    {
                        AddToCache(cacheKey, cacheDuration, 
                            new ServerDefaultUrl() { defaultUrl = defaultUrl, serverId = serverId }, 
                            serverUrls.ToList());
                    }
                }
            }

            return defaultUrl;
        }

        private void AddToCache(string key, int duration, ServerDefaultUrl url, List<ServerDefaultUrl> list)
        {
            list.Add(url);
            _cacheProvider.Add(key, list, duration);
        }
    }
}
