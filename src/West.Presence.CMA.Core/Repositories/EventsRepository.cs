using System;
using System.Data;
using System.Collections.Generic;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Servies;

namespace West.Presence.CMA.Core.Repositories
{
    public interface IEventsRepository
    {
        IEnumerable<Event> GetEvents(int serverId, string baseUrl, DateTime startDate, DateTime endDate, bool cutEvents = true);
    }

    public class APIEventsRepositoryOld {
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly IDatabaseProvider _databaseProvider;
        private readonly IDBConnectionService _dBConnectionService;

        public APIEventsRepositoryOld(IHttpClientProvider httpClientProvider, IDatabaseProvider databaseProvider, IDBConnectionService dBConnectionService)
        {
            _httpClientProvider = httpClientProvider;
            _databaseProvider = databaseProvider;
            _dBConnectionService = dBConnectionService;
        }

        public IEnumerable<Event> GetEvents(int serverId, string baseUrl, DateTime startDate, DateTime endDate, bool cutEvents = true)
        {
            string connectionStr = _dBConnectionService.GetConnection(baseUrl);
            if (string.IsNullOrEmpty(connectionStr))
                return new List<Event>();

            int calendarId = _databaseProvider.GetCellValue<int>(connectionStr, "SELECT TOP 1 object_id FROM cma_entries WHERE server_id=@serverId AND content_type='calendar'",new { serverId = serverId }, CommandType.Text);
            if(calendarId<=0)
                return new List<Event>();

            var responseData = _httpClientProvider.SoapPostData<Event>($"{baseUrl}common/controls/workspacecalendar/ws/workspacecalendarws.asmx/GetEventsByCalendarId", new
            {
                calendarId = serverId,
                startTime = startDate,
                endTime = endDate
            }, "PresenceApi");

            if (responseData == null)
                return responseData;

            if (cutEvents)
            {
                List<Event> rerangeEvents = new List<Event>();
                foreach (Event ce in responseData)
                {
                    if (ce.EndTime.Date > ce.StartTime.Date)
                    {
                        // Split Multiple Event for cross date event
                        double days = Convert.ToInt32((ce.EndTime.Date - ce.StartTime.Date).TotalDays) + 1;
                        for (int i = 1; i <= days; i++)
                        {
                            DateTime tStartTime = i == 1 ? ce.StartTime : ce.StartTime.Date.AddDays(i - 1);
                            DateTime tEndTime = i == days ? ce.EndTime : ce.StartTime.Date.AddDays(i).AddSeconds(-1);
                            if (tStartTime == tEndTime && tStartTime.ToString("HH:mm:ss") == "23:59:59")
                                continue;
                            Event newCe = new Event();
                            newCe.EventId = int.Parse(ce.EventId.ToString() + i.ToString());
                            newCe.Description = ce.Description;
                            newCe.StartTime = tStartTime;
                            newCe.EndTime = tEndTime;
                            newCe.ServerId = ce.ServerId;
                            rerangeEvents.Add(newCe);
                        }
                    }
                    else
                    {
                        ce.ServerId = serverId;
                        rerangeEvents.Add(ce);
                    }
                }

                return rerangeEvents;
            }
            else
            {
                foreach (Event e in responseData)
                    e.ServerId = serverId;

                return responseData;
            }
        }

    }

    public class APIEventsRepository : IEventsRepository
    {
        private readonly IHttpClientProvider _httpClientProvider;

        public APIEventsRepository(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
        }

        public IEnumerable<Event> GetEvents(int serverId, string baseUrl, DateTime startDate, DateTime endDate, bool cutEvents=true)
        {
            var responseData = _httpClientProvider.SoapPostData<Event>($"{baseUrl}common/controls/workspacecalendar/ws/workspacecalendarws.asmx/geteventsbyserverid", new
            {
                serverId = serverId,
                startTime = startDate,
                endTime = endDate
            }, "PresenceApi");

            if (responseData == null)
                return responseData;

            if (cutEvents)
            {
                List<Event> rerangeEvents = new List<Event>();
                foreach (Event ce in responseData)
                {
                    if (ce.EndTime.Date > ce.StartTime.Date)
                    {
                        // Split Multiple Event for cross date event
                        double days = Convert.ToInt32((ce.EndTime.Date - ce.StartTime.Date).TotalDays) + 1;
                        for (int i = 1; i <= days; i++)
                        {
                            DateTime tStartTime = i == 1 ? ce.StartTime : ce.StartTime.Date.AddDays(i - 1);
                            DateTime tEndTime = i == days ? ce.EndTime : ce.StartTime.Date.AddDays(i).AddSeconds(-1);
                            if (tStartTime == tEndTime && tStartTime.ToString("HH:mm:ss") == "23:59:59")
                                continue;
                            Event newCe = new Event();
                            newCe.EventId = int.Parse(ce.EventId.ToString() + i.ToString());
                            newCe.Description = ce.Description;
                            newCe.StartTime = tStartTime;
                            newCe.EndTime = tEndTime;
                            newCe.ServerId = ce.ServerId;
                            rerangeEvents.Add(newCe);
                        }
                    }
                    else
                    {
                        ce.ServerId = serverId;
                        rerangeEvents.Add(ce);
                    }
                }

                return rerangeEvents;
            }
            else {
                foreach (Event e in responseData)
                    e.ServerId = serverId;

                return responseData;
            }
        }
    }
}
