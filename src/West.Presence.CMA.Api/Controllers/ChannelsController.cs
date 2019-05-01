using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using West.Presence.CMA.Core.Servies;

namespace West.Presence.CMA.Api.Controllers
{
    [ApiController]
    public class ChannelsController : BaseMethods
    {
        private readonly ISchoolsService _schoolsService;
        private readonly ILogger _logger = Log.ForContext<ChannelsController>();

        public ChannelsController(ISchoolsService schoolsService)
        {
            _schoolsService = schoolsService;
        }

        [HttpGet("cmaapi/1/channels")]
        public IActionResult GetChannels([FromQuery] string baseUrl = "")
        {
            baseUrl = GetBaseUrl(baseUrl);

            if (string.IsNullOrEmpty(baseUrl))
            {
                _logger.Error("baseUrl not been provided");
                return NoContent();
            }

            var schools = _schoolsService.GetSchools(baseUrl, "");

            if (schools == null || schools.Count() == 0)
            {
                _logger.Error("no schools been found");
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
                                 },
                                 relationships = new
                                 {
                                     categories = new { data = new object[] { new { type = "school-messenger.categories", id = "7" } } },
                                     channels = new { data = new object[] { new { type = "school-messenger.channels", id = s.ServerId.ToString() } } },
                                 }
                             };

            return Ok(new { data = dataSchool });
        }
    }
}