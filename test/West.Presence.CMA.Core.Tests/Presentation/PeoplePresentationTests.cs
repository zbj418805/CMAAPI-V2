using Moq;
using System.Collections.Generic;
using System.Linq;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Presentations;
using West.Presence.CMA.Core.Servies;
using Xunit;


namespace West.Presence.CMA.Core.Presentation.Tests
{
    public class PeoplePresentationTests
    {
        private Mock<IPeopleService> mockPeopleService;
        private IPeoplePresentation peoplePresentation;

        public PeoplePresentationTests()
        {
            mockPeopleService = new Mock<IPeopleService>();
            mockPeopleService.Setup(p => p.GetPeople(new List<int>() { 1, 2 }, "", "")).Returns(GetSamplePeople(10));
            peoplePresentation = new PeoplePresentation(mockPeopleService.Object);
        }

        [Fact]
        public void Test_People_First_Page()
        {
            int total;
            IEnumerable<Person> samplePeople = peoplePresentation.GetPeople(new List<int>() { 1, 2 }, "", "", 0, 2, out total);

            Assert.NotNull(samplePeople);
            Assert.Equal(2, samplePeople.Count());
        }

        [Fact]
        public void Test_People_Second_Page()
        {
            int total;
            IEnumerable<Person> samplePeople = peoplePresentation.GetPeople(new List<int>() { 1, 2 }, "", "", 1, 3, out total);

            Assert.NotNull(samplePeople);
            Assert.Equal(3, samplePeople.Count());
            Assert.Equal("First_3", samplePeople.FirstOrDefault().FirstName);
            Assert.Equal("First_4", samplePeople.Skip(1).Take(1).FirstOrDefault().FirstName);
            Assert.Equal("First_5", samplePeople.LastOrDefault().FirstName);
        }


        [Fact]
        public void Test_People_Page_with_no_items()
        {
            int total;
            IEnumerable<Person> samplePeople = peoplePresentation.GetPeople(new List<int>() { 1, 2 }, "", "", 5 ,3, out total);
            
            Assert.NotNull(samplePeople);
            Assert.Empty(samplePeople);
        }

        private List<Person> GetSamplePeople(int count)
        {
            List<Person> people = new List<Person>();

            for (int i = 0; i < 10; i++)
            {
                people.Add(new Person()
                {
                    UserId = i,
                    FirstName = $"First_{i}",
                    LastName = $"Last_{i}"
                });
            }
            return people;
        }
    }
}
