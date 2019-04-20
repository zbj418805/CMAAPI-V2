using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Presentations;
using West.Presence.CMA.Core.Servies;
using Xunit;


namespace West.Presence.CMA.Core.Tests.Presentation
{
    public class EventsPresentationTests
    {
        private Mock<IEventsService> mockEventsService;
        private IEventsPresentation eventPresentation;

        public EventsPresentationTests()
        {
            List<Event> events = new List<Event>();

            for(int i = 0; i < 10; i++)
            {
                events.Add(new Event() {
                     name = $"Event Name {i}",
                     startTime = DateTime.Today,
                     endTime = DateTime.Today.AddDays(1)
                });
            }

            mockEventsService = new Mock<IEventsService>();
            mockEventsService.Setup(p => p.GetEvents("1,2","",DateTime.Today, DateTime.Today)).Returns(events);
            eventPresentation = new EventsPresentation(mockEventsService.Object);
        }

        [Fact]
        public void Test_Events_First_Page()
        {
            IEnumerable<Event> sampleEvents = eventPresentation.GetEvents("1,2", "", DateTime.Today, DateTime.Today, 0, 2);

            Assert.NotNull(sampleEvents);
            Assert.Equal(2,sampleEvents.Count());
        }

        [Fact]
        public void Test_Events_Second_Page()
        {
            IEnumerable<Event> sampleEvents = eventPresentation.GetEvents("1,2", "", DateTime.Today, DateTime.Today, 1, 3);

            Assert.NotNull(sampleEvents);
            Assert.Equal(3, sampleEvents.Count());
            Assert.Equal("Event Name 3", sampleEvents.FirstOrDefault().name);
            Assert.Equal("Event Name 4", sampleEvents.Skip(1).Take(1).FirstOrDefault().name);
            Assert.Equal("Event Name 5", sampleEvents.LastOrDefault().name);
        }


        [Fact]
        public void Test_Events_Page_with_no_items()
        {
            IEnumerable<Event> sampleEvents = eventPresentation.GetEvents("1,2", "", DateTime.Today, DateTime.Today, 5, 3);

            Assert.NotNull(sampleEvents);
            Assert.Empty(sampleEvents);
        }
    }
}
