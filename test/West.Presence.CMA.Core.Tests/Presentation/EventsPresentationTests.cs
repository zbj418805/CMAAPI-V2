using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Presentations;
using West.Presence.CMA.Core.Servies;
using Xunit;


namespace West.Presence.CMA.Core.Presentation.Tests
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
                    Name = $"Events 1-{i}",
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow
                });
            }

            mockEventsService = new Mock<IEventsService>();
            mockEventsService.Setup(p => p.GetEvents(new List<int>(){ 1, 2 }, "", "",DateTime.Today, DateTime.Today, true)).Returns(events);
            eventPresentation = new EventsPresentation(mockEventsService.Object);
        }

        [Fact]
        public void Test_Events_First_Page()
        {
            int total;
            IEnumerable<Event> sampleEvents = eventPresentation.GetEvents(new List<int>() { 1, 2 }, "", "", DateTime.Today, DateTime.Today, 0, 2, true, out total);

            Assert.NotNull(sampleEvents);
            Assert.Equal(2,sampleEvents.Count());
        }

        [Fact]
        public void Test_Events_Second_Page()
        {
            int total;
            IEnumerable<Event> sampleEvents = eventPresentation.GetEvents(new List<int>() { 1, 2 }, "", "", DateTime.Today, DateTime.Today, 1, 3, true, out total);

            Assert.NotNull(sampleEvents);
            Assert.Equal(3, sampleEvents.Count());
            Assert.Equal("Events 1-3", sampleEvents.FirstOrDefault().Name);
            Assert.Equal("Events 1-4", sampleEvents.Skip(1).Take(1).FirstOrDefault().Name);
            Assert.Equal("Events 1-5", sampleEvents.LastOrDefault().Name);
        }

        [Fact]
        public void Test_Events_Page_with_no_items()
        {
            int total;
            IEnumerable<Event> sampleEvents = eventPresentation.GetEvents(new List<int>() { 1, 2 }, "", "", DateTime.Today, DateTime.Today, 5, 3, true, out total);

            Assert.NotNull(sampleEvents);
            Assert.Empty(sampleEvents);
        }
    }
}
