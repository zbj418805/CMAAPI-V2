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
    public class PeopleServiceTests
    {
        private Mock<IPeopleRepository> mockPeopleRepository;
        private Mock<ICacheProvider> mockCacheProvider;
        private Mock<IOptions<CMAOptions>> mockOptions;
        private IPeopleService peopleService;

        public PeopleServiceTests()
        {
            CMAOptions option = new CMAOptions
            {
                Environment = "Dev",
                CachePeopleKey = "CMAPeopleKey",
                CachePeopleDurationInSeconds = 300
            };

            mockOptions = new Mock<IOptions<CMAOptions>>();
            mockOptions.Setup(p => p.Value).Returns(option);
        }

        [Fact]
        public void Test_News_From_CacheProvider_With_No_Search()
        {
            List<Person> lsPeople1 = new List<Person>();
            for (int i = 0; i < 10; i++)
            {
                lsPeople1.Add(new Person()
                {
                     userId = i,
                     firstName = $"First{i}",
                     lastName = $"Last{i}"
                });
            }
            var people1 = lsPeople1.AsEnumerable();

            List<Person> lsPeople2 = new List<Person>();
            for (int i = 0; i < 10; i++)
            {
                lsPeople2.Add(new Person()
                {
                    userId = i+100,
                    firstName = $"First{i}",
                    lastName = $"Last{i}"
                });
            }
            var people2 = lsPeople2.AsEnumerable();

            mockPeopleRepository = new Mock<IPeopleRepository>();
            //mockNewsRepository.Setup(p => p.GetNews(1)).Returns(lsNews1);

            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<Person>>("CMAPeopleKey_localhost_1", out people1)).Returns(true);
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<Person>>("CMAPeopleKey_localhost_2", out people2)).Returns(true);

            peopleService = new PeopleService(mockCacheProvider.Object, mockOptions.Object, mockPeopleRepository.Object);
            var resultPeople = peopleService.GetPeople(new List<int>() { 1, 2 }, "http://localhost/", "");

            Assert.NotNull(resultPeople);
            Assert.Equal(20, resultPeople.Count());
        }


        [Fact]
        public void Test_News_From_Repo_With_No_Search()
        {
            List<Person> lsPeople1 = new List<Person>();
            for (int i = 0; i < 10; i++)
            {
                lsPeople1.Add(new Person()
                {
                    userId = i,
                    firstName = $"First{i}",
                    lastName = $"Last{i}"
                });
            }
            //var people1 = lsPeople1.AsEnumerable();

            List<Person> lsPeople2 = new List<Person>();
            for (int i = 0; i < 10; i++)
            {
                lsPeople2.Add(new Person()
                {
                    userId = i + 100,
                    firstName = $"First{i}",
                    lastName = $"Last{i}"
                });
            }
            //var people2 = lsPeople2.AsEnumerable();

            mockPeopleRepository = new Mock<IPeopleRepository>();
            mockPeopleRepository.Setup(p => p.GetPeople(1, "http://localhost/")).Returns(lsPeople1);
            mockPeopleRepository.Setup(p => p.GetPeople(2, "http://localhost/")).Returns(lsPeople2);

            mockCacheProvider = new Mock<ICacheProvider>();
            //mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<Person>>("CMAPeopleKey_Dev_1", out people1)).Returns(true);
            //mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<Person>>("CMAPeopleKey_Dev_2", out people2)).Returns(true);

            peopleService = new PeopleService(mockCacheProvider.Object, mockOptions.Object, mockPeopleRepository.Object);
            var resultPeople = peopleService.GetPeople(new List<int>() { 1, 2 }, "http://localhost/", "");

            Assert.NotNull(resultPeople);
            Assert.Equal(20, resultPeople.Count());
        }

        [Fact]
        public void Test_News_From_Repo_And_CacheProvider_With_No_Search()
        {
            List<Person> lsPeople1 = new List<Person>();
            for (int i = 0; i < 10; i++)
            {
                lsPeople1.Add(new Person()
                {
                    userId = i,
                    firstName = $"First{i}",
                    lastName = $"Last{i}"
                });
            }
            var people1 = lsPeople1.AsEnumerable();

            List<Person> lsPeople2 = new List<Person>();
            for (int i = 0; i < 10; i++)
            {
                lsPeople2.Add(new Person()
                {
                    userId = i + 100,
                    firstName = $"First{i}",
                    lastName = $"Last{i}"
                });
            }
            var people2 = lsPeople2.AsEnumerable();

            mockPeopleRepository = new Mock<IPeopleRepository>();
            //mockPeopleRepository.Setup(p => p.GetPeople(1, "")).Returns(lsPeople1);
            mockPeopleRepository.Setup(p => p.GetPeople(2, "http://localhost/")).Returns(lsPeople2);

            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<Person>>("CMAPeopleKey_localhost_1", out people1)).Returns(true);
            //mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<Person>>("CMAPeopleKey_Dev_2", out people2)).Returns(true);

            peopleService = new PeopleService(mockCacheProvider.Object, mockOptions.Object, mockPeopleRepository.Object);
            var resultPeople = peopleService.GetPeople(new List<int>() { 1, 2 }, "http://localhost/", "");

            Assert.NotNull(resultPeople);
            Assert.Equal(20, resultPeople.Count());
        }

        [Fact]
        public void Test_News_From_Repo_And_CacheProvider_With_Search()
        {
            List<Person> lsPeople1 = new List<Person>() {
                new Person() { userId = 1, firstName = "First1", lastName = "Last1" }
            };
            var people1 = lsPeople1.AsEnumerable();

            List<Person> lsPeople2 = new List<Person>()
            {
                new Person() { userId = 2, firstName = "First1", lastName = "Last1" }
            };

            var people2 = lsPeople2.AsEnumerable();

            mockPeopleRepository = new Mock<IPeopleRepository>();
            mockPeopleRepository.Setup(p => p.GetPeople(1, "http://localhost/")).Returns(lsPeople1);
            mockPeopleRepository.Setup(p => p.GetPeople(2, "http://localhost/")).Returns(lsPeople2);

            mockCacheProvider = new Mock<ICacheProvider>();
            //mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<Person>>("CMAPeopleKey_Dev_1", out people1)).Returns(true);
            //mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<Person>>("CMAPeopleKey_Dev_2", out people2)).Returns(true);

            peopleService = new PeopleService(mockCacheProvider.Object, mockOptions.Object, mockPeopleRepository.Object);
            var resultPeople = peopleService.GetPeople(new List<int>() { 1, 2 }, "http://localhost/", "First1");

            Assert.NotNull(resultPeople);
            Assert.Equal(2,resultPeople.Count());
            Assert.Equal("First1", resultPeople.FirstOrDefault().firstName);
            Assert.Equal("First1", resultPeople.LastOrDefault().firstName);
        }
    }
}
