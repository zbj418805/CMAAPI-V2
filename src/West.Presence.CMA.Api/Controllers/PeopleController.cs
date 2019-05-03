using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using West.Presence.CMA.Api.Model;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Presentations;
using West.Presence.CMA.Core.Repositories;
using West.Presence.CMA.Core.Servies;

namespace West.Presence.CMA.Api.Controllers {
    [ApiController]
    public class PeopleController : BaseMethods {
        private readonly ILogger _logger = Log.ForContext<PeopleController> ();
        private readonly ISchoolsService _schoolsService;
        private readonly IPeoplePresentation _peoplePresentation;
        private readonly IPeopleRepository _peopleRepository;

        public PeopleController (ISchoolsService schoolService, IPeoplePresentation peoplePresentation, IPeopleRepository peopleRepository) {
            _schoolsService = schoolService;
            _peoplePresentation = peoplePresentation;
            _peopleRepository = peopleRepository;
        }

        [HttpGet ("cmaapi/1/resources/school-messenger.people")]
        public IActionResult GetPeople ([FromQuery] QueryPagination page, [FromQuery] QueryFilter filter, [FromQuery] string query = "", [FromQuery] string baseUrl = "")
        {
            baseUrl = GetBaseUrl(baseUrl);
            if (filter.categories == 0)
                filter = GetQueryFilter();
            query = GetSearchKey(filter.search, query);

            if (string.IsNullOrEmpty(baseUrl))
            {
                _logger.Error ("baseUrl not been provided");
                return NoContent ();
            }

            var schools = _schoolsService.GetSchools (baseUrl, "");

            if (IsResourcesRequestValid (filter, schools, new List<int> () { 5, 6 })) {
                int total;
                IEnumerable<Person> simplePeople = _peoplePresentation.GetPeople(filter.channelServerIds, baseUrl, query, page.offset, page.limit, out total);

                var links = string.IsNullOrEmpty(query) ? this.GetLinks(baseUrl, filter, page, query, true, total) : null;

                if (simplePeople.Count () == 0) {
                    _logger.Information("no people found");
                    return NoContent ();
                }

                var fullPeople = _peopleRepository.GetPeopleInfo(baseUrl, simplePeople);

                var dataList = from p in fullPeople
                            select new {
                                id = p.id.ToString (),
                                type = "school-messenger.people",
                                attributes = new {
                                firstName = p.firstName,
                                lastName = p.lastName,
                                jobTitle = p.jobTitle,
                                phoneNumber = p.phoneNumber,
                                email = p.email,
                                website = p.website,
                                twitter = p.twitter,
                                about = p.description + p.personalMessage,
                                image = p.imageUrl,
                                name = p.firstName + " " + p.lastName,
                                description = p.description,
                                blog = p.blog,
                                personalMessage = p.personalMessage
                                },
                                relationships = new {
                                categories = new { data = new object[] { new { type = "school-messenger.categories", id = "6" } } },
                                channels = new { data = new object[] { new { type = "school-messenger.channels", id = p.serverId.ToString () } } },
                                }
                            };

                return Ok(new { Data = dataList, Links = links });
            }

            _logger.Error("validation failed");
            return NoContent ();
        }
    }
}