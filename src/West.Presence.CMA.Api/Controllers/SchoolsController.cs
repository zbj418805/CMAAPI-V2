using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using West.Presence.CMA.Api.Model;
using West.Presence.CMA.Core.Presentations;

namespace West.Presence.CMA.Api.Controllers
{
    [ApiController]
    public class SchoolsController : ControllerBase
    {
        private readonly ILogger _logger = Log.ForContext<SchoolsController>();
        //private readonly ISchoolsPresentation _schoolPresentation;

        public SchoolsController()
        {

        }

        //public SchoolsController(ISchoolsPresentation schoolPresentation)
        //{
        //    _schoolPresentation = schoolPresentation;
        //}

        [HttpGet("cmaapi/1/resources/school-messenger.schools")]
        public IActionResult GetSchools([FromQuery]QueryPagination page, [FromQuery]QueryFilter filter, [FromQuery]string query = null)
        {
            //var sch = _schoolPresentation.GetSchools(1291956, "", "http://presence.kingzad.local/", 0, 100);
            return Ok();
        }
    }
}