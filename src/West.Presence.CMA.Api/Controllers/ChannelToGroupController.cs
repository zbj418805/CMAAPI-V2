using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using West.Presence.CMA.Api.Model;

namespace West.Presence.CMA.Api.Controllers
{
    [ApiController]
    public class ChannelToGroupController : ControllerBase
    {
        public ChannelToGroupController() { }

        [HttpGet("cmaapi/1/shoutem/integration/{appId}/groups")]
        public IActionResult GetChannelsToGroups(int appId)
        {
            return Ok();
        }

        [HttpPost("cmaapi/1/shoutem/integration/{appId}/groups")]
        public IActionResult SetChannelToGroup(int appId, [FromBody] dynamic value)
        {
            return Ok();
        }

        [HttpDelete("cmaapi/1/shoutem/integration/{appId}")]
        public IActionResult DeleteChannelToGroup(int appId, [FromQuery] QueryFilter value)
        {
            return Ok();
        }
    }
}