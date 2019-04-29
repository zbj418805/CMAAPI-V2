using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using System.Data;

namespace West.Presence.CMA.Core.Repositories
{

    public interface IChannel2GroupRepository
    {
        IEnumerable<Channel2Group> GetChannel2Group(string baseUrl, int districtServerId);
        void SetChannel2Group(string baseUrl, int districtServerId, List<Channel2Group> c2gs, int appId, string endpointUrl, string sessionId);
        int GetGroupId(string baseUrl, int serverId);
        AppExtended GetAppExtended(string baseUrl, int districtServerId);
    }

    public class DBChannel2GroupRepository : IChannel2GroupRepository
    {
        IDatabaseProvider _databaseProvider;
        public DBChannel2GroupRepository(IDatabaseProvider databaseProvider)
        {
            _databaseProvider=databaseProvider;
        }
        public IEnumerable<Channel2Group> GetChannel2Group(string baseUrl, int districtServerId)
        {
            return _databaseProvider.GetData<Channel2Group>("[dbo].[cma_c2g.get_all]", new { district_server_Id = districtServerId }, CommandType.StoredProcedure);
        }
        public void SetChannel2Group(string baseUrl, int districtServerId, List<Channel2Group> c2gs, int appId, string endpointUrl, string sessionId)
        {
            foreach(Channel2Group c2g in c2gs)
            {
                _databaseProvider.Excute("[dbo].[cma_c2g.set]", new { server_id = c2g.channelId, group_id = c2g.groupId }, CommandType.StoredProcedure);
            }
            
            SetAppExtended(new AppExtended
            {
                dictrictServerId = districtServerId,
                appId = appId,
                endpoint = endpointUrl,
                sessionId = sessionId
            });
        }
        public int GetGroupId(string baseUrl, int serverId)
        {
            return _databaseProvider.GetCellValue<int>("SELECT TOP 1 shoutem_group_id FROM cma_entries WHERE server_id=@sId and content_type='news'",
                new { sId = serverId },
                CommandType.Text);
        }
        public AppExtended GetAppExtended(string baseUrl, int districtServerId)
        {
            return _databaseProvider.GetData<AppExtended>("SELECT app_id as appId, endpoint, session_id as sessionId, last_modified as lastModified, district_server_id as dictrictServerId FROM cma_extended WHERE district_server_id=@dsi ",
                new { dsi = districtServerId },
                System.Data.CommandType.Text).FirstOrDefault();
        }
        private void SetAppExtended(AppExtended appExt)
        {
            _databaseProvider.Excute("[dbo].[cma_extended.set]",
                new
                {
                    district_server_id = appExt.dictrictServerId,
                    app_id = appExt.appId,
                    endpoint_url = appExt.endpoint,
                    session_id = appExt.sessionId
                },
                CommandType.StoredProcedure);
        }
    }

    public class APIChannel2GroupRepository : IChannel2GroupRepository
    {
        private readonly IHttpClientProvider _httpClientProvider;
        public APIChannel2GroupRepository(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
        }
        public AppExtended GetAppExtended(string baseUrl, int districtServerId)
        {
            return _httpClientProvider.GetData<AppExtended>($"{baseUrl}webapi/cma/getAppExtended/{districtServerId}").FirstOrDefault();
        }
        public IEnumerable<Channel2Group> GetChannel2Group(string baseUrl, int districtServerId)
        {
            return _httpClientProvider.GetData<Channel2Group>($"{baseUrl}webapi/cma/channel2group/{districtServerId}");
        }
        public int GetGroupId(string baseUrl, int serverId)
        {
            return _httpClientProvider.GetData<int>($"{baseUrl}webapi/cma/channel2group/{serverId}" ).FirstOrDefault();
        }
        public void SetChannel2Group(string baseUrl, int districtServerId, List<Channel2Group> c2gs, int appId, string endpointUrl, string sessionId)
        {
            var payload = new {
                Channel2Group = c2gs,
                appId = appId,
                endpointUrl = endpointUrl,
                sessionId = sessionId
            };

            _httpClientProvider.PostData<string>($"{baseUrl}webapi/cma/channel2group/", payload);

            throw new NotImplementedException();
        }
    }
}
