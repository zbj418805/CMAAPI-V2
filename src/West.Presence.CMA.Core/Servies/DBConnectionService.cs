using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Repositories;

namespace West.Presence.CMA.Core.Servies
{
    public interface IDBConnectionService
    {
        string GetConnection(string baseUrl);
    }
    public class DBConnectionService : IDBConnectionService
    {
        
        private readonly ICacheProvider _cacheProvider;
        private readonly IOptions<CMAOptions> _options;
        private readonly IConnectionRepository _connectionRepository;

        public DBConnectionService(ICacheProvider cacheProvider, IOptions<CMAOptions> options, IConnectionRepository connectionRepository)
        {
            _cacheProvider = cacheProvider;
            _options = options;
            _connectionRepository = connectionRepository;
        }

        public string GetConnection(string baseUrl)
        {
            Uri u = new Uri(baseUrl);
            string DBString;
            string cacheKey = $"{_options.Value.CacheConnStrKey}_{u.Host}";
            //Check weather connection in Cache
            if (!_cacheProvider.TryGetValue<string>(cacheKey, out DBString))
            {
                //Retrieve Connection String from Repo
                DBString = _connectionRepository.GetConnection(baseUrl);
                if (!string.IsNullOrEmpty(DBString))
                {
                    //Add to Cache
                    _cacheProvider.Add(cacheKey, DBString, 0);
                }
            }

            return DBString;
        }
    }
}
