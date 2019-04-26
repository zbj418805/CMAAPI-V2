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
    public class NewsController : BaseMethods
    {
        private readonly ILogger _logger = Log.ForContext<NewsController>();
        private readonly ISchoolsPresentation _schoolPresentation;
        private readonly INewsPresentation _newsPresentation;

        public NewsController(ISchoolsPresentation schoolPresentation, INewsPresentation newsPresentation) {
            _schoolPresentation = schoolPresentation;
            _newsPresentation = newsPresentation;
        }

        [HttpGet("cmaapi/1/resources/school-messenger.news")]
        public IActionResult GetNews([FromQuery]QueryPagination page, [FromQuery]QueryFilter filter, [FromQuery]string query = "", [FromQuery] string baseUrl = "")
        {
            string search = string.IsNullOrEmpty(query) ? filter.Search == null ? "" : filter.Search.ToLower().Trim() : query.ToLower().Trim();
            if (baseUrl.Length == 0)
            {
                _logger.Error("baseUrl not been provided");
                return NoContent();
            }
            int total;
            var schools = _schoolPresentation.GetSchools(baseUrl, "", page.Offset, page.Limit, out total);

            if (IsResourcesRequestValid(filter, schools, new List<int>() { 1, 2 }))
            {
                var news = _newsPresentation.GetNews(filter.ChannelServerIds, baseUrl, search, page.Offset, page.Limit, out total);

                var links = string.IsNullOrEmpty(search) ? this.GetLinks(baseUrl, filter, page, "", true, total) : null;

                if (news.Count() == 0)
                {
                    _logger.Information("nocotent, success");
                    return Ok(new { Data = schools, Links = links });
                }


                

                return Ok(new { Data = news, Links = links });
            }

            return NoContent();
        }
    }
}