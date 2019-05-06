using System.Collections.Generic;
using System.Linq;
using Moq;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Repositories;
using Xunit;

namespace West.Presence.CMA.Core.Repository.Tests
{
    public class ApiPeopleRepositoryTests
    {
        private Mock<IHttpClientProvider> mockHttpClientProvider;

        public ApiPeopleRepositoryTests()
        {

        }

        [Fact]
        public void Test_People_Repository_Get_People()
        {
            List<Person> lsPeople = GetSamplePeople(10);
            var people = lsPeople.AsEnumerable();

            mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.Setup(p => p.GetData<Person>("http://test.url/presence/api/cma/people/1234","PresenceApi")).Returns(people);

            APIPeopleRepository peopleRepo = new APIPeopleRepository(mockHttpClientProvider.Object);

            var resultPeople = peopleRepo.GetPeople(1234, "http://test.url/");

            Assert.NotNull(peopleRepo);

            Assert.Equal(10, resultPeople.Count());
        }

        [Fact]
        public void Test_People_Repository_Get_No_People()
        {
            List<Person> lsPeople = GetSamplePeople(10);
            var people = lsPeople.AsEnumerable();

            mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.Setup(p => p.GetData<Person>("http://test.url/presence/api/cma/people/12334", "PresenceApi")).Returns(people);

            APIPeopleRepository peopleRepo = new APIPeopleRepository(mockHttpClientProvider.Object);

            var resultPeople = peopleRepo.GetPeople(1234, "http://test.url/");

            Assert.NotNull(peopleRepo);

            Assert.Empty(resultPeople);
        }

        private List<Person> GetSamplePeople(int count)
        {
            List<Person> lsPeople = new List<Person>();
            for (int i = 0; i < count; i++)
            {
                lsPeople.Add(new Person()
                {
                    UserId = i,
                    FirstName = $"MyFirstName_{i}",
                    LastName = $"LastName--{i}",
                    JobTitle = $"JobTitle"
                });
            }

            return lsPeople;
        }
    }
}
