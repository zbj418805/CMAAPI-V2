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
            string search = string.IsNullOrEmpty(query) ? filter.Search == null ? "" : filter.Search.ToLower().Trim() : query.ToLower().Trim();
            if (baseUrl.Length == 0)
            {
                _logger.Error("baseUrl not been provided");
                return NoContent();
            }
            int total;
            var schools = _schoolPresentation.GetSchools(baseUrl, query, page.Offset, page.Limit, out total);

            if(IsResourcesRequestValid(filter, schools, new List<int>() { 7,8 }))
            {
                var links = string.IsNullOrEmpty(search) ? this.GetLinks(baseUrl, filter, page, "", true, total) : null;

                if (schools.Count() == 0)
                {
                    _logger.Information("nocotent, success");
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

            return NoContent();
        }
    }
}