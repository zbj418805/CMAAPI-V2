using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Moq;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Repositories;
using West.Presence.CMA.Core.Servies;
using Xunit;


namespace West.Presence.CMA.Core.Tests.Services
{
    public class EventsServiceTests
    {
        private Mock<IEventsRepository> mockEventsRepository;
        private Mock<ICacheProvider> mockCacheProvider;
        private Mock<IOptions<CMAOptions>> mockOptions;
        private IEventsService eventsService;

        public EventsServiceTests()
        {
            CMAOptions option = new CMAOptions
            {
                Environment = "Dev",
                CacheEventsKey = "CMAEventsKey",
                CacheEventsDurationInSeconds = 300
            };

            mockOptions = new Mock<IOptions<CMAOptions>>();
            mockOptions.Setup(p => p.Value).Returns(option);
        }

        [Fact]
        public void Test_Events_From_CacheProvider_No_Search()
        {
            List<Event> lsEvents1 = new List<Event>();
            for (int i = 0; i < 10; i++)
            {
                lsEvents1.Add(new Event()
                {
                    name = $"Cached Events 1-{i}",
                    startTime = DateTime.UtcNow,
                    endTime = DateTime.UtcNow
                });
            }
            var events1 = lsEvents1.AsEnumerable();

            List<Event> lsEvents2 = new List<Event>();
            for (int i = 0; i < 10; i++)
            {
                lsEvents2.Add(new Event()
                {
                    name = $"Cached Events 2-{i}",
                    startTime = DateTime.UtcNow,
                    endTime = DateTime.UtcNow
                });
            }
            var events2 = lsEvents2.AsEnumerable();

            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<Event>>($"CMAEventsKey_Dev_1_{DateTime.Today.ToString("yyyyMMdd")}", out events1)).Returns(true);
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<Event>>($"CMAEventsKey_Dev_2_{DateTime.Today.ToString("yyyyMMdd")}", out events2)).Returns(true);

            mockEventsRepository = new Mock<IEventsRepository>();
            eventsService = new EventsService(mockCacheProvider.Object, mockOptions.Object, mockEventsRepository.Object);

            var resultEvents = eventsService.GetEvents("1,2", "", "", DateTime.Today,DateTime.Today.AddMonths(12));

            Assert.NotNull(resultEvents);
            Assert.Equal(20, resultEvents.Count());
        }

        [Fact]
        public void Test_Events_From_Repo_No_Search()
        {
            List<Event> lsEvents1 = new List<Event>();
            for (int i = 0; i < 10; i++)
            {
                lsEvents1.Add(new Event()
                {
                    name = $"Repo Events 1-{i}",
                    startTime = DateTime.UtcNow,
                    endTime = DateTime.UtcNow
                });
            }
            var events1 = lsEvents1.AsEnumerable();

            List<Event> lsEvents2 = new List<Event>();
            for (int i = 0; i < 10; i++)
            {
                lsEvents2.Add(new Event()
                {
                    name = $"Repo Events 2-{i}",
                    startTime = DateTime.UtcNow,
                    endTime = DateTime.UtcNow
                });
            }
            var events2 = lsEvents2.AsEnumerable();

            //mock up cache provider
            mockCacheProvider = new Mock<ICacheProvider>();
            //mock up repo
            mockEventsRepository = new Mock<IEventsRepository>();
            mockEventsRepository.Setup(p => p.GetEvents(1,"", DateTime.Today, DateTime.Today.AddMonths(12))).Returns(lsEvents1);
            mockEventsRepository.Setup(p => p.GetEvents(2,"", DateTime.Today, DateTime.Today.AddMonths(12))).Returns(lsEvents2);


            eventsService = new EventsService(mockCacheProvider.Object, mockOptions.Object, mockEventsRepository.Object);
            var resultEvents = eventsService.GetEvents("1,2","", "", DateTime.Today, DateTime.Today.AddMonths(12));

            Assert.NotNull(resultEvents);
            Assert.Equal(20, resultEvents.Count());
        }
        
        [Fact]
        public void Test_Events_From_Repo_And_Cache_No_Search()
        {
            List<Event> lsEvents1 = new List<Event>();
            for (int i = 0; i < 10; i++)
            {
                lsEvents1.Add(new Event()
                {
                    name = $"Cache Events 1-{i}",
                    startTime = DateTime.UtcNow,
                    endTime = DateTime.UtcNow
                });
            }
            var events1 = lsEvents1.AsEnumerable();

            List<Event> lsEvents2 = new List<Event>();
            for (int i = 0; i < 10; i++)
            {
                lsEvents2.Add(new Event()
                {
                    name = $"Repo Events 1-{i}",
                    startTime = DateTime.UtcNow,
                    endTime = DateTime.UtcNow
                });
            }
            var events2 = lsEvents2.AsEnumerable();

            //mock up cache provider
            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<Event>>($"CMAEventsKey_Dev_1_{DateTime.Today.ToString("yyyyMMdd")}", out events1)).Returns(true);
            //mock up repo
            mockEventsRepository = new Mock<IEventsRepository>();
            mockEventsRepository.Setup(p => p.GetEvents(2, "", DateTime.Today, DateTime.Today.AddMonths(12))).Returns(lsEvents2);

            eventsService = new EventsService(mockCacheProvider.Object, mockOptions.Object, mockEventsRepository.Object);

            var resultEvents = eventsService.GetEvents("1,2", "", "", DateTime.Today, DateTime.Today.AddMonths(12));

            Assert.NotNull(resultEvents);
            Assert.Equal(20, resultEvents.Count());
        }

        [Fact]
        public void Test_Events_From_Repo_And_Cache_Search()
        {
            List<Event> lsEvents1 = new List<Event>();
            for (int i = 0; i < 10; i++)
            {
                lsEvents1.Add(new Event()
                {
                    name = $"Cache Events 1-{i}",
                    startTime = DateTime.UtcNow,
                    endTime = DateTime.UtcNow
                });
            }
            var events1 = lsEvents1.AsEnumerable();

            List<Event> lsEvents2 = new List<Event>();
            for (int i = 0; i < 10; i++)
            {
                lsEvents2.Add(new Event()
                {
                    name = $"Repo Events 1-{i}",
                    startTime = DateTime.UtcNow,
                    endTime = DateTime.UtcNow
                });
            }
            var events2 = lsEvents2.AsEnumerable();

            //mock up cache provider
            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<Event>>($"CMAEventsKey_Dev_1_{DateTime.Today.ToString("yyyyMMdd")}", out events1)).Returns(true);
            //mock up repo
            mockEventsRepository = new Mock<IEventsRepository>();
            mockEventsRepository.Setup(p => p.GetEvents(2, "", DateTime.Today, DateTime.Today.AddMonths(12))).Returns(lsEvents2);


            eventsService = new EventsService(mockCacheProvider.Object, mockOptions.Object, mockEventsRepository.Object);
            var resultEvents = eventsService.GetEvents("1,2", "1-1", "", DateTime.Today, DateTime.Today.AddMonths(12));

            Assert.NotNull(resultEvents);
            Assert.Equal(2, resultEvents.Count());
        }
    }
}
