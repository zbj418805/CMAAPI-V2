using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using West.Presence.CMA.Core.Servies;

namespace West.Presence.CMA.Api.Controllers
{
    [ApiController]
    public class ChannelsController : ControllerBase
    {
        private readonly ISchoolsService _schoolsService;

        public ChannelsController(ISchoolsService schoolsService)
        {
            _schoolsService = schoolsService;
        }

        [HttpGet("cmaapi/1/channels")]
        public IActionResult GetChannels([FromQuery] string baseUrl = "")
        {
            if (baseUrl.Length == 0)
            {
                return NoContent();
            }

            var schools = _schoolsService.GetSchools(baseUrl, "");

            if (schools.Count() == 0)
            {
                return NoContent();
            }

            int index = 0;
            var dataSchool = from s in schools
                             select new
                             {
                                 id = s.ServerId.ToString(),
                                 type = "school-messenger.channels",
                                 attributes = new
                                 {
                                     name = s.Description,
                                     index = index++,
                                     iconUrl = "",
                                     mandatory = s.ServerId == s.DistrictServerId
                                 }
                             };

            return Ok(new { data = dataSchool });
        }
    }
}