using System.Collections.Generic;
using System.Linq;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Repositories;
using Xunit;
using Moq;
using System;
using System.Net.Http;

namespace West.Presence.CMA.Core.Repository.Tests
{
    public class ApiEventsRepositoryTests
    {
        private Mock<IHttpClientProvider> mockHttpClientProvider;
        private IEventsRepository _eventsRepository;

        public ApiEventsRepositoryTests()
        {
            mockHttpClientProvider = new Mock<IHttpClientProvider>();
        }

        [Fact]
        public void Test_Events_Reposity_Get_Events()
        {
            var news = GetSampleEvents(10, "Repo", false).AsEnumerable();

            mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.Setup(p => p.SoapPostData<Event>("http://test.url/"+ "common/controls/workspacecalendar/ws/workspacecalendarws.asmx/geteventsbyserverid", 
                It.IsAny<object>(), "PresenceApi")).Returns(news);

            _eventsRepository = new APIEventsRepository(mockHttpClientProvider.Object);

            var events = _eventsRepository.GetEvents(1234, "http://test.url/", DateTime.Today, DateTime.Today.AddMonths(12));

            Assert.NotEmpty(events);
            Assert.Equal(10, events.Count());
        }

        [Fact]
        public void Test_Events_Reposity_Get_Separated_Events()
        {
            var news = GetSampleEvents(10, "Repo", true).AsEnumerable();

            mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.Setup(p => p.SoapPostData<Event>("http://test.url/" + "common/controls/workspacecalendar/ws/workspacecalendarws.asmx/geteventsbyserverid",
                It.IsAny<object>(), "PresenceApi")).Returns(news);

            _eventsRepository = new APIEventsRepository(mockHttpClientProvider.Object);

            var events = _eventsRepository.GetEvents(1234, "http://test.url/", DateTime.Today, DateTime.Today.AddMonths(12));

            Assert.NotEmpty(events);
            Assert.Equal(16, events.Count());
        }


        private List<Event> GetSampleEvents(int count, string title, bool addMultipleDaysEvents)
        {
            List<Event> lsEvents = new List<Event>();
            for (int i = 0; i < 10; i++)
            {
                lsEvents.Add(new Event()
                {
                    Name = $"{title} Events 1-{i}",
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow
                });
            }

            if (addMultipleDaysEvents)
            {
                for (int i = 0; i < 2; i++)
                {
                    lsEvents.Add(new Event()
                    {
                        Name = $"{title} Events 1-{i}",
                        StartTime = DateTime.UtcNow,
                        EndTime = DateTime.UtcNow.AddDays(2)
                    });
                }
            }

            return lsEvents;
        }
    }
}
