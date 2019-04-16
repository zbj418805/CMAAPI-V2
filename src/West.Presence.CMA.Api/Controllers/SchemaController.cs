using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace West.Presence.CMA.Api.Controllers
{
    [ApiController]
    public class SchemaController : ControllerBase
    {
        private readonly ILogger _logger = Log.ForContext<SchemaController>();

        [HttpGet("cmaapi/1/schemas")]
        public IActionResult AllSchemas()
        {
            return Ok();
        }

        [HttpGet("cmaapi/1/schemas/school-messenger.news")]
        public IActionResult GetNewsSchema()
        {
            return Ok();
        }


        [HttpGet("cmaapi/1/schemas/school-messenger.events")]
        public IActionResult GetEventsSchema()
        {
            return Ok();
        }

        [HttpGet("cmaapi/1/schemas/school-messenger.people")]
        public IActionResult GetPeopleSchema()
        {
            return Ok();
        }

        [HttpGet("cmaapi/1/schemas/school-messenger.schools")]
        public IActionResult GetSchoolsSchema()
        {
            return Ok();
        }
    }
}