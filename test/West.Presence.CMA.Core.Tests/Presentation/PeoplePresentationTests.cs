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
            List<Person> people = new List<Person>();

            for (int i = 0; i < 10; i++)
            {
                people.Add(new Person()
                {
                    userId = i,
                    firstName = $"First_{i}",
                    lastName = $"Last_{i}"
                });
            }

            mockPeopleService = new Mock<IPeopleService>();
            mockPeopleService.Setup(p => p.GetPeople(new List<int>() { 1, 2 }, "", "")).Returns(people);
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
            Assert.Equal("First_3", samplePeople.FirstOrDefault().firstName);
            Assert.Equal("First_4", samplePeople.Skip(1).Take(1).FirstOrDefault().firstName);
            Assert.Equal("First_5", samplePeople.LastOrDefault().firstName);
        }


        [Fact]
        public void Test_People_Page_with_no_items()
        {
            int total;
            IEnumerable<Person> samplePeople = peoplePresentation.GetPeople(new List<int>() { 1, 2 }, "", "", 5 ,3, out total);
            
            Assert.NotNull(samplePeople);
            Assert.Empty(samplePeople);
        }
    }
}
