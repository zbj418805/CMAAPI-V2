using System;
using System.Collections.Generic;
using System.Net.Http;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;

namespace West.Presence.CMA.Core.Repositories
{
    public interface IEventsRepository
    {
        IEnumerable<Event> GetEvents(int serverId, string baseUrl, DateTime startDate, DateTime endDate);
    }

    public class DBEventsRepository : IEventsRepository
    {
        public DBEventsRepository(string baseUrl)
        {
        }

        public IEnumerable<Event> GetEvents(int serverId, string baseUrl, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }

    public class APIEventsRepository : IEventsRepository
    {
        private readonly IHttpClientProvider _httpClientProvider;
        public APIEventsRepository(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
        }

        public IEnumerable<Event> GetEvents(int serverId, string baseUrl, DateTime startDate, DateTime endDate)
        {
            var responseData = _httpClientProvider.SoapPostData<Event>($"{baseUrl}common/controls/workspacecalendar/ws/workspacecalendarws.asmx/geteventsbyserverid", new
            {
                serverId = serverId,
                startTime = startDate,
                endTime = endDate
            });

            List<Event> rerangeEvents = new List<Event>();
            foreach(Event ce in responseData)
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
    }
}
