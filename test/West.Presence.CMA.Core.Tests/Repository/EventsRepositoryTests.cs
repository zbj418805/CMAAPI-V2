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
            List<Event> lsEvents = new List<Event>();
            for (int i = 0; i < 10; i++)
            {
                lsEvents.Add(new Event()
                {
                    name = $"MyFirstName_{i}"
                });
            }
            var events = lsEvents.AsEnumerable();

            mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.Setup(p => p.GetData<Event>("http://test.url/" + "/presence/Api/CMA/Events/" + "1234/" +$"{DateTime.Today.ToString("yyyyMMdd")}/{DateTime.Today.AddMonths(12).ToString("yyyyMMdd")}")).Returns(events);

            APIEventsRepository eventsRepo = new APIEventsRepository(mockHttpClientProvider.Object);

            var resultEvents = eventsRepo.GetEvents(1234, "http://test.url/", DateTime.Today, DateTime.Today.AddMonths(12));

            Assert.NotNull(resultEvents);

            Assert.Empty(resultEvents);
        }

        [Fact]
        public void Test_Events_Repository_Get_No_Events()
        {
            List<Event> lsEvents = new List<Event>();
            for (int i = 0; i < 10; i++)
            {
                lsEvents.Add(new Event()
                {
                    name = $"MyFirstName_{i}"
                });
            }
            var events = lsEvents.AsEnumerable();

            mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.Setup(p => p.GetData<Event>("http://test.url/" + "/presence/Api/CMA/Events/" + "12344/" + $"{DateTime.Today.ToString("yyyyMMdd")}/{DateTime.Today.AddMonths(12).ToString("yyyyMMdd")}")).Returns(events);

            APIEventsRepository eventsRepo = new APIEventsRepository(mockHttpClientProvider.Object);

            var resultEvents = eventsRepo.GetEvents(1234, "http://test.url/", System.DateTime.Today, System.DateTime.Today.AddMonths(12));

            Assert.NotNull(resultEvents);

            Assert.Empty(resultEvents);
        }
    }
}
