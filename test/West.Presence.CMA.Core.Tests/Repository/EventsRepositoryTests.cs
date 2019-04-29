using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Repositories;
using Xunit;

namespace West.Presence.CMA.Core.Tests.Repository
{
    public class EventsRepositoryTests
    {
        private Mock<IHttpClientProvider> mockHttpClientProvider;

        public EventsRepositoryTests()
        {

        }

        [Fact]
        public void Test_Events_Repository_Get_Events()
        {
            mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.Setup(p => p.SoapPostData<Event>("http://test.url/Common/controls/WorkspaceCalendar/ws/WorkspaceCalendarWS.asmx/GetEventsByServerId", new
            {
                serverId = 1234,
                startTime = DateTime.Today,
                endTime = DateTime.Today.AddMonths(12)
            })).Returns(GetSampleEvents(10));

            APIEventsRepository eventsRepo = new APIEventsRepository(mockHttpClientProvider.Object);

            var resultEvents = eventsRepo.GetEvents(1234, "http://test.url/", DateTime.Today, DateTime.Today.AddMonths(12));

            Assert.NotNull(resultEvents);

            //Assert.Equal(10, resultEvents.Count());
        }

        [Fact]
        public void Test_Events_Repository_Get_No_Events()
        {
            mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.Setup(p => p.SoapPostData<Event>("http://test.url/Common/controls/WorkspaceCalendar/ws/WorkspaceCalendarWS.asmx/GetEventsByServerId", new
            {
                serverId = 1234,
                startTime = DateTime.Today,
                endTime = DateTime.Today.AddMonths(12)
            })).Returns(GetSampleEvents(0));
            
            APIEventsRepository eventsRepo = new APIEventsRepository(mockHttpClientProvider.Object);

            var resultEvents = eventsRepo.GetEvents(1234, "http://test.url/", System.DateTime.Today, System.DateTime.Today.AddMonths(12));

            Assert.NotNull(resultEvents);

            Assert.Empty(resultEvents);
        }

        private IEnumerable<Event> GetSampleEvents(int count)
        {
            List<Event> lsEvents = new List<Event>();
            for(int i = 0; i < count; i++)
            {
                lsEvents.Add(new Event {
                    Name = $"MyFirstName_{i}",
                    StartTime = DateTime.Today,
                    EndTime = DateTime.Today.AddDays(1),
                    StartTimeUTC = DateTime.Today,
                    EndTimeUTC = DateTime.Today.AddDays(1),
                    IsAllDayEvent = false
                });
            }

            return lsEvents.AsEnumerable();
        }
    }
}
