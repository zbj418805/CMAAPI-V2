using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace West.Presence.CMA.Api.Controllers
{
    [ApiController]
    public class ChannelToGroupController : ControllerBase
    {
        [HttpGet("cmaapi/1/shoutem/integration/{appId}/groups")]
        public IActionResult GetChannelsToGroups(int appId)
        {
            return Ok();
        }
    }
}