using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using West.Presence.CMA.Api.Controllers;
using West.Presence.CMA.Api.Model;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Presentations;
using West.Presence.CMA.Core.Repositories;
using West.Presence.CMA.Core.Servies;
using Xunit;

namespace West.Presence.CMA.Api.Tests.Controllers
{
    public class PeopleControllerTests
    {
        private PeopleController _sut;

        private Mock<IPeoplePresentation> mockPeoplePresentation;
        private Mock<IPeopleRepository> mockPeopleRepository;
        private Mock<ISchoolsService> mockSchoolsService;

        public PeopleControllerTests()
        {
            mockSchoolsService = new Mock<ISchoolsService>();
            mockPeoplePresentation = new Mock<IPeoplePresentation>();
            mockPeopleRepository = new Mock<IPeopleRepository>();
        }

        [Fact]
        public void Test_PeopleEndpoint_Returns_OK()
        {
            List<School> schools = new List<School>();
            for (int i = 0; i < 10; i++)
            {
                schools.Add(new School()
                {
                    Name = $"School Name {i}",
                    Description = $"Description {1} ...",
                    DistrictServerId = 10,
                    ServerId = i
                });
            }

            mockSchoolsService.Setup(p => p.GetSchools("http://localhost/", "")).Returns(schools);


            List<Person> simplePeople = new List<Person>();

            for (int i = 0; i < 10; i++)
            {
                simplePeople.Add(new Person()
                {
                    userId = i,
                    firstName = $"First_{i}",
                    lastName = $"Last_{i}"
                });
            }

            int total = 0;
            mockPeoplePresentation.Setup(p => p.GetPeople(new List<int>() { 0, 1, 2, 3, 4 }, "", "http://localhost/", 0, 20, out total)).Returns(simplePeople);

            List<PersonInfo> fullPeople = new List<PersonInfo>();

            for (int i = 0; i < 10; i++)
            {
                fullPeople.Add(new PersonInfo()
                {
                    userId = i,
                    firstName = $"First_{i}",
                    lastName = $"Last_{i}"
                });
            }

            mockPeopleRepository.Setup(p => p.GetPeopleInfo("http://localhost", simplePeople)).Returns(fullPeople);
            // Arrange
            _sut = new PeopleController(mockSchoolsService.Object, mockPeoplePresentation.Object, mockPeopleRepository.Object);
            // Act
            var result = _sut.GetPeople(new QueryPagination() { Limit = 20, Offset = 0 },
                new QueryFilter() { Categories = 5 }, "", "http://localhost/");

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }
    }
}
