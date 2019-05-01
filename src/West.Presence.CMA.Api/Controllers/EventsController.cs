using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using West.Presence.CMA.Api.Model;
using West.Presence.CMA.Core.Presentations;
using West.Presence.CMA.Core.Servies;

namespace West.Presence.CMA.Api.Controllers
{
    [ApiController]
    public class EventsController : BaseMethods
    {
        private readonly ILogger _logger = Log.ForContext<EventsController>();
        private readonly ISchoolsService _schoolsService;
        private readonly IEventsPresentation _eventsPresentation;
        const int ENDPROID = 12;

        public EventsController(ISchoolsService schoolService, IEventsPresentation eventsPresentation) {
            _schoolsService = schoolService;
            _eventsPresentation = eventsPresentation;
        }

        [HttpGet("cmaapi/1/resources/school-messenger.events")]
        public IActionResult GetEvents([FromQuery]QueryPagination page, [FromQuery]QueryFilter filter, [FromQuery]string query = "", [FromQuery] string baseUrl = "")
        {
            baseUrl = GetBaseUrl(baseUrl);

            string search = string.IsNullOrEmpty(query) ? filter.Search == null ? "" : filter.Search.ToLower().Trim() : query.ToLower().Trim();
            if (baseUrl.Length == 0)
            {
                _logger.Error("baseUrl not been provided");
                return NoContent();
            }

            var schools = _schoolsService.GetSchools(baseUrl, "");

            if (IsResourcesRequestValid(filter, schools, new List<int>() { 3, 4 }))
            {
                if (filter.StartTime == null || filter.StartTime.Year < DateTime.Today.AddYears(-2).Year)
                {
                    filter.StartTime = DateTime.Today.AddDays(-10);
                    filter.EndTime = DateTime.Today.AddMonths(ENDPROID);
                }

                if (filter.EndTime == null || filter.StartTime.Year < DateTime.Today.AddYears(-2).Year)
                {
                    filter.EndTime = DateTime.Today.AddMonths(ENDPROID);
                }

                if (filter.EndTime < filter.StartTime)
                {
                    filter.StartTime = DateTime.Today.AddDays(-10);
                    filter.EndTime = DateTime.Today.AddMonths(ENDPROID);
                }

                int total;
                var events = _eventsPresentation.GetEvents(filter.ChannelServerIds, baseUrl, search, filter.StartTime, filter.EndTime, page.Offset, page.Limit, out total);

                var links = string.IsNullOrEmpty(search) ? this.GetLinks(baseUrl, filter, page, "", true, total) : null;

                if (events.Count() == 0)
                {
                    return NoContent();
                }

                List<string> lsTranslatableFields = new List<string> { "attributes.name", "attributes.description" };

                var eventsData = from c in events
                                       select new
                                       {
                                           id = c.EventId.ToString(),
                                           type = "school-messenger.events",
                                           attributes = new
                                           {
                                               name = c.Name,
                                               description = c.Description,
                                               starttime = c.StartTime,
                                               endtime = c.EndTime,
                                               starttimeutc = c.StartTimeUTC,
                                               endtimeutc = c.EndTimeUTC,
                                               isallday = c.IsAllDayEvent
                                           },
                                           meta = new
                                           {
                                               i18n = new
                                               {
                                                   translatableFields = lsTranslatableFields
                                               }
                                           },
                                           relationships = new
                                           {
                                               categories = new { data = new object[] { new { type = "school-messenger.categories", id = "4" } } },
                                               channels = new { data = new object[] { new { type = "school-messenger.channels", id = c.ServerId.ToString() } } },
                                           }
                                       };
                return Ok(new { data = eventsData, links = links });
            }

            return NoContent();
        }
    }
}