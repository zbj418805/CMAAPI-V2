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
            string DBString;

            string cacheKey = _options.Value.CacheConnKey;

            IEnumerable<Connection> connections;

            if (!_cacheProvider.TryGetValue<IEnumerable<Connection>>(cacheKey, out connections))
            {
                List<Connection> lsConnections = new List<Connection>();
                //Get connection  from repo
                DBString = _connectionRepository.GetConnection(baseUrl);
                if (!string.IsNullOrEmpty(DBString))
                {
                    lsConnections.Add(new Connection() { baseUrl = baseUrl, connectionString = DBString });
                    //Set to Cache
                    _cacheProvider.Add(cacheKey, lsConnections, 0);
                }
            }
            else {
                DBString = connections.Where(c => c.baseUrl == baseUrl).Select(x => x.connectionString).FirstOrDefault();
                if (string.IsNullOrEmpty(DBString)) {
                    List<Connection> lsConnections = connections.ToList();
                    DBString = _connectionRepository.GetConnection(baseUrl);
                    lsConnections.Add(new Connection() { baseUrl = baseUrl, connectionString = DBString });
                    _cacheProvider.Add(cacheKey, lsConnections, 0);
                }
            }

            return DBString;
        }
    }
}
