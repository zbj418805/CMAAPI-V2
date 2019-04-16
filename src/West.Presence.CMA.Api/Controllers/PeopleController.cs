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
    public class PeopleController : ControllerBase
    {
        private readonly ILogger _logger = Log.ForContext<PeopleController>();

        [HttpGet("cmaapi/1/resources/school-messenger.people")]
        public IActionResult GetPeople([FromQuery]QueryPagination page, [FromQuery]QueryFilter filter, [FromQuery]string query = null)
        {
            return Ok();
        }
    }
}