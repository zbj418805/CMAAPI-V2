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
        public IActionResult GetPeople ([FromQuery] QueryPagination page, [FromQuery] QueryFilter filter, [FromQuery] string query = "", [FromQuery] string baseUrl = "") {

            baseUrl = GetBaseUrl(baseUrl);

            string search = GetSearchKey(filter.Search, query);

            if (baseUrl.Length == 0) {
                _logger.Error ("baseUrl not been provided");
                return NoContent ();
            }

            var schools = _schoolsService.GetSchools (baseUrl, "");

            if (IsResourcesRequestValid (filter, schools, new List<int> () { 5, 6 })) {
                int total;
                IEnumerable<Person> simplePeople = _peoplePresentation.GetPeople(filter.ChannelServerIds, baseUrl, search, page.Offset, page.Limit, out total);

                var links = string.IsNullOrEmpty(search) ? this.GetLinks(baseUrl, filter, page, query, true, total) : null;

                if (simplePeople.Count () == 0) {
                    _logger.Information("no people found");
                    return NoContent ();
                }

                var fullPeople = _peopleRepository.GetPeopleInfo(baseUrl, simplePeople);

                var dataList = from p in fullPeople
                            select new {
                                id = p.Id.ToString (),
                                type = "school-messenger.people",
                                attributes = new {
                                firstName = p.FirstName,
                                lastName = p.LastName,
                                jobTitle = p.JobTitle,
                                phoneNumber = p.PhoneNumber,
                                email = p.Email,
                                website = p.Website,
                                twitter = p.Twitter,
                                about = p.Description + p.PersonalMessage,
                                image = p.ImageUrl,
                                name = p.FirstName + " " + p.LastName,
                                description = p.Description,
                                blog = p.Blog,
                                personalMessage = p.PersonalMessage
                                },
                                relationships = new {
                                categories = new { data = new object[] { new { type = "school-messenger.categories", id = "6" } } },
                                channels = new { data = new object[] { new { type = "school-messenger.channels", id = p.ServerId.ToString () } } },
                                }
                            };

                return Ok(new { Data = dataList, Links = links });
            }

            _logger.Error("validation failed");
            return NoContent ();
        }
    }
}