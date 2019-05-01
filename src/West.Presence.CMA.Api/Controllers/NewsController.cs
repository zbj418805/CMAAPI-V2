using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using West.Presence.CMA.Api.Model;
using West.Presence.CMA.Core.Presentations;
using West.Presence.CMA.Core.Servies;

namespace West.Presence.CMA.Api.Controllers {
    [ApiController]
    public class NewsController : BaseMethods {
        private readonly ILogger _logger = Log.ForContext<NewsController> ();
        private readonly ISchoolsService _schoolsService;
        private readonly INewsPresentation _newsPresentation;

        public NewsController (ISchoolsService schoolService, INewsPresentation newsPresentation) {
            _schoolsService = schoolService;
            _newsPresentation = newsPresentation;
        }

        [HttpGet ("cmaapi/1/resources/school-messenger.news")]
        public IActionResult GetNews ([FromQuery] QueryPagination page, [FromQuery] QueryFilter filter, [FromQuery] string query = "", [FromQuery] string baseUrl = "") {

            baseUrl = GetBaseUrl(baseUrl);

            string search = GetSearchKey(filter.Search, query);

            if (baseUrl.Length == 0) {
                _logger.Error ("baseUrl not been provided");
                return NoContent ();
            }

            var schools = _schoolsService.GetSchools (baseUrl, "");

            if (IsResourcesRequestValid(filter, schools, new List<int>() { 1, 2 })) {
                int total;
                var news = _newsPresentation.GetNews(filter.ChannelServerIds, baseUrl, search, page.Offset, page.Limit, out total);

                var links = string.IsNullOrEmpty(search) ? this.GetLinks (baseUrl, filter, page, "", true, total) : null;

                if (news.Count () == 0) {
                    _logger.Information ("no news found");
                    return Ok (new { Data = schools, Links = links });
                }

                var dataList = from p in news
                                select new {
                                    id = p.Id.ToString (),
                                    type = "school-messenger.news",
                                    attributes = new {
                                    title = p.Title,
                                    featuredImage = string.IsNullOrEmpty (p.FeaturedImage) ? p.FeaturedImage : Uri.EscapeUriString (p.FeaturedImage),
                                    imageTitle = p.ImageTitle,
                                    summary = p.Summary,
                                    body = p.Body,
                                    linkOfCurrentPage = p.LinkOfCurrentPage,
                                    publishedDate = p.PublishedDate.ToString ("yyyy-MM-ddTHH:mm:ssZ"),
                                    pageLastModified = p.PageLastModified.ToString ("yyyy-MM-ddTHH:mm:ssZ"),
                                    pageTitle = p.PageTitle
                                    },
                                    meta = new {
                                        i18n = new {
                                            translatableFields = new List<string> { "attributes.title", "attributes.summary", "attributes.body", "attributes.pageTitle" }
                                        }
                                    },
                                    relationships = new {
                                        categories = new { data = new object[] { new { type = "school-messenger.categories", id = "2" } } },
                                        channels = new { data = new object[] { new { type = "school-messenger.channels", id = p.ServerId.ToString () } } },
                                    }
                                };

                return Ok (new { Data = dataList, Links = links });
            }

            _logger.Error("validation failed");
            return NoContent ();
        }
    }
}