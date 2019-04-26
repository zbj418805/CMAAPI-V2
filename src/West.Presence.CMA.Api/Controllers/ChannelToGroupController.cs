﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using West.Presence.CMA.Api.Model;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Repositories;
using West.Presence.CMA.Core.Servies;

namespace West.Presence.CMA.Api.Controllers
{
    [ApiController]
    public class ChannelToGroupController : ControllerBase
    {
        ISchoolsService _schoolsService;
        IChannel2GroupRepository _channel2GroupRepository;

        public ChannelToGroupController(ISchoolsService schoolsService, IChannel2GroupRepository channel2GroupRepository) {
            _schoolsService = schoolsService;
            _channel2GroupRepository = channel2GroupRepository;
        }

        [HttpGet("cmaapi/1/shoutem/integration/{appId}/groups")]
        public IActionResult GetChannelsToGroups(int appId, [FromQuery] string baseUrl = "")
        {
            IEnumerable<Channel2Group> c2g = _channel2GroupRepository.GetChannel2Group(baseUrl, 0);
            return Ok(new { data = new { channelsToGroups = c2g } });
        }

        [HttpPost("cmaapi/1/shoutem/integration/{appId}/groups")]
        public IActionResult SetChannelToGroup(int appId, [FromBody] dynamic value, [FromQuery] string baseurl="")
        {
            string endpointUrl = "";
            string sessionId = "";
            List<Channel2Group> lsGoups = new List<Channel2Group>();

            foreach (var pair in value.data.channelsToGroups)
            {
                lsGoups.Add(new Channel2Group { channelId = int.Parse(pair.Name), groupId = int.Parse(pair.Value) });
            }

            if (value.data.endpointUrl != null)
            {
                endpointUrl = value.data.endpointUrl;
            }

            if (value.data.sessionId != null)
            {
                sessionId = value.data.sessionId;
            }

            _channel2GroupRepository.SetChannel2Group(baseurl, 0, lsGoups, appId, endpointUrl, sessionId);

            return Ok();
        }

        [HttpDelete("cmaapi/1/shoutem/integration/{appId}/groups")]
        public IActionResult DeleteChannelToGroup(int appId, [FromQuery] QueryFilter filter, [FromQuery] string baseurl = "")
        {
            List<Channel2Group> lsGoups = new List<Channel2Group>();

            foreach (string serverId in filter.Channels.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                lsGoups.Add(new Channel2Group { channelId = int.Parse(serverId), groupId = 0} );
            }

            _channel2GroupRepository.SetChannel2Group(baseurl, 0, lsGoups, appId, "", "");

            return Ok();
        }
    }
}