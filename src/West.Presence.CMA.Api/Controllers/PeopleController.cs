using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using West.Presence.CMA.Api.Model;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Presentations;
using West.Presence.CMA.Core.Repositories;
using West.Presence.CMA.Core.Servies;

namespace West.Presence.CMA.Api.Controllers
{
    [ApiController]
    public class PeopleController : BaseMethods
    {
        private readonly ILogger _logger = Log.ForContext<PeopleController>();
        private readonly ISchoolsService _schoolService;
        private readonly IPeoplePresentation _peoplePresentation;
        private readonly IPeopleRepository _peopleRepository;

        public PeopleController(ISchoolsService schoolService, IPeoplePresentation peoplePresentation, IPeopleRepository peopleRepository) {
            _schoolService = schoolService;
            _peoplePresentation = peoplePresentation;
        }

        [HttpGet("cmaapi/1/resources/school-messenger.people")]
        public IActionResult GetPeople([FromQuery]QueryPagination page, [FromQuery]QueryFilter filter, [FromQuery]string query = "", [FromQuery] string baseUrl = "")
        {
            string search = string.IsNullOrEmpty(query) ? filter.Search == null ? "" : filter.Search.ToLower().Trim() : query.ToLower().Trim();
            if (baseUrl.Length == 0)
            {
                _logger.Error("baseUrl not been provided");
                return NoContent();
            }
            var schools = _schoolService.GetSchools(baseUrl, "");

            if (IsResourcesRequestValid(filter, schools, new List<int>() { 5, 6 }))
            {
                int total;
                IEnumerable<Person> simplePeople = _peoplePresentation.GetPeople(filter.ChannelServerIds, baseUrl, search, page.Offset, page.Limit, out total);

                var links = string.IsNullOrEmpty(search) ? this.GetLinks(baseUrl, filter, page, query, true, total) : null;

                if (simplePeople.Count() == 0)
                {
                    _logger.Information("nocotent, success");
                    return NoContent();
                }
                
                var fullPeople = _peopleRepository.GetPeopleInfo(baseUrl, simplePeople.ToList());

                var dataList = from p in fullPeople
                               select new
                               {
                                   id = p.Id.ToString(),
                                   type = "school-messenger.people",
                                   attributes = new
                                   {
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
                                   relationships = new
                                   {
                                       categories = new { data = new object[] { new { type = "school-messenger.categories", id = "6" } } },
                                       channels = new { data = new object[] { new { type = "school-messenger.channels", id = p.ServerId.ToString() } } },
                                   }
                               };
            }

            return NoContent();
        }
    }
}