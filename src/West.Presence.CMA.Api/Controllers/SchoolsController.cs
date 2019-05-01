using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using West.Presence.CMA.Api.Model;
using West.Presence.CMA.Core.Presentations;

namespace West.Presence.CMA.Api.Controllers
{
    [ApiController]
    public class SchoolsController : BaseMethods
    {
        private readonly ILogger _logger = Log.ForContext<SchoolsController>();
        private readonly ISchoolsPresentation _schoolPresentation;

        public SchoolsController(ISchoolsPresentation schoolPresentation)
        {
            _schoolPresentation = schoolPresentation;
        }

        [HttpGet("cmaapi/1/resources/school-messenger.schools")]
        public IActionResult GetSchools([FromQuery]QueryPagination page, [FromQuery]QueryFilter filter, [FromQuery]string query = "", [FromQuery] string baseUrl = "")
        {
            baseUrl = GetBaseUrl(baseUrl);
            if(filter.categories == 0)
                filter = GetQueryFilter();
            query = GetSearchKey(filter.search, query);

            if (string.IsNullOrEmpty(baseUrl))
            {
                _logger.Error("baseUrl not been provided");
                return NoContent();
            }

            int total;
            var schools = _schoolPresentation.GetSchools(baseUrl, query, page.offset, page.limit, out total);

            if(IsResourcesRequestValid(filter, schools, new List<int>() { 7,8 }))
            {
                var links = string.IsNullOrEmpty(query) ? this.GetLinks(baseUrl, filter, page, "", true, total) : null;

                if (schools.Count() == 0)
                {
                    _logger.Information("no schools");
                    return Ok(new { Data = schools, Links = links });
                }
                
                var dataList = from sch in schools
                               select new
                               {
                                   id = sch.ServerId.ToString(),
                                   type = "school-messenger.schools",
                                   attributes = new
                                   {
                                       name = sch.Description,
                                       address = sch.Address.Address1 + " " + sch.Address.City + " " + sch.Address.Province + " " + sch.Address.PostCode,
                                       latitude = "",
                                       longitude = "",
                                       defaultUrl = sch.Url,
                                       slogan = sch.Slogan,
                                       logo = sch.IconUrl,
                                       phone = sch.Phone,
                                       fax = sch.Fax,
                                       facebook = sch.Facebook,
                                       twitter = sch.Twitter,
                                       youtube = sch.Youtube,
                                       email = sch.Email
                                   },
                                   relationships = new
                                   {
                                       categories = new { data = new object[] { new { type = "school-messenger.categories", id = "8" } } },
                                       channels = new { data = new object[] { new { type = "school-messenger.channels", id = sch.DistrictServerId.ToString() } } },
                                   }
                               };

                return Ok(new { Data = dataList, Links = links });
            }

            _logger.Information("validation failed");
            return NoContent();
        }
    }
}