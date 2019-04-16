using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;

namespace West.Presence.CMA.Api.Controllers
{
    [ApiController]
    public class ChannelsController : ControllerBase
    {
        [HttpGet("cmaapi/1/channels")]
        public IActionResult GetAll([FromQuery] string baseUrl = "")
        {
            return Ok();
        }
    }
}