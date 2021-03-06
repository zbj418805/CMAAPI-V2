﻿using Serilog;
using System.Data;
using West.Presence.CMA.Core.Helper;

namespace West.Presence.CMA.Core.Repositories
{
    public interface IDefaultUrlRepository
    {
        string GetDefaultUrl(int serverId, string connectionStr);
    }

    public class DefaultUrlRepository : IDefaultUrlRepository
    {
        IDatabaseProvider _databaseProvider;
        private readonly ILogger _logger = Log.ForContext<DefaultUrlRepository>();

        public DefaultUrlRepository(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        public string GetDefaultUrl(int serverId, string connectionStr)
        {
            if (string.IsNullOrEmpty(connectionStr) || serverId <= 0)
            {
                _logger.Error($"connection is emptry or serverId is not valid: connectionStrin : {connectionStr}, serverId: {serverId}");
                return "";
            }

            return _databaseProvider.GetCellValue<string>(connectionStr, "SELECT TOP 1 url FROM click_server_urls WHERE default_p=1 AND server_id=@serverId", new { serverId = serverId }, CommandType.Text);
        }
    }
}
