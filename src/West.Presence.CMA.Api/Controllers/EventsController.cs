using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using West.Presence.CMA.Api.Model;

namespace West.Presence.CMA.Api.Controllers
{
    [ApiController]
    public class EventsController : ControllerBase
    {
        [HttpGet("cmaapi/1/resources/school-messenger.events")]
        public IActionResult GetEvents([FromQuery]QueryPagination page, [FromQuery]QueryFilter filter, [FromQuery]string query = null)
        {
            return Ok();
        }
    }
}