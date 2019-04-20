using Moq;
using System.Collections.Generic;
using System.Linq;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Presentations;
using West.Presence.CMA.Core.Servies;
using Xunit;


namespace West.Presence.CMA.Core.Tests.Presentation
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
                    name = $"Name {i}",
                    description = $"Decription {i} ..."
                });
            }

            mockPeopleService = new Mock<IPeopleService>();
            mockPeopleService.Setup(p => p.GetPeople("1,2", "")).Returns(people);
            peoplePresentation = new PeoplePresentation(mockPeopleService.Object);
        }

        [Fact]
        public void Test_People_First_Page()
        {
            IEnumerable<Person> samplePeople = peoplePresentation.GetPeople("1,2", "", 0, 2);

            Assert.NotNull(samplePeople);
            Assert.Equal(2, samplePeople.Count());
        }

        [Fact]
        public void Test_People_Second_Page()
        {
            IEnumerable<Person> samplePeople = peoplePresentation.GetPeople("1,2", "", 1, 3);

            Assert.NotNull(samplePeople);
            Assert.Equal(3, samplePeople.Count());
            Assert.Equal("Name 3", samplePeople.FirstOrDefault().name);
            Assert.Equal("Name 4", samplePeople.Skip(1).Take(1).FirstOrDefault().name);
            Assert.Equal("Name 5", samplePeople.LastOrDefault().name);
        }


        [Fact]
        public void Test_People_Page_with_no_items()
        {
            IEnumerable<Person> samplePeople = peoplePresentation.GetPeople("1,2", "", 5,3);
            
            Assert.NotNull(samplePeople);
            Assert.Empty(samplePeople);
        }
    }
}
