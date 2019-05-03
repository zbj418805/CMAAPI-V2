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

namespace West.Presence.CMA.Api.Controllers.Tests
{
    public class PeopleControllerTests : BaseControllerTest
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
            // Arrange
            mockSchoolsService.Setup(p => p.GetSchools("http://localhost/", "")).Returns(GetSampleSchools(10,10));

            var sampleSimplePerson = GetSampleSimplePerson(10);

            var schoolList = new List<int>() { 0, 1, 2, 3, 4 };
            int total = 0;
            mockPeoplePresentation.Setup(p => p.GetPeople(schoolList, "http://localhost/", "", 0, 20, out total)).Returns(sampleSimplePerson);

            mockPeopleRepository.Setup(p => p.GetPeopleInfo("http://localhost/", sampleSimplePerson)).Returns(GetSamplePersonInfo(10));
            
            
            _sut = new PeopleController(mockSchoolsService.Object, mockPeoplePresentation.Object, mockPeopleRepository.Object);
            // Act
            var result = _sut.GetPeople(new QueryPagination() { limit = 20, offset = 0 },
                new QueryFilter() { categories = 5, channelServerIds = schoolList }, "", "http://localhost/");

            // Assert
            //Assert.IsType<NoContentResult>(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_PeopleEndpoint_NoSimplePersons_Returns_NoContent()
        {
            // Arrange
            mockSchoolsService.Setup(p => p.GetSchools("http://localhost/", "")).Returns(GetSampleSchools(10, 10));

            var sampleSimplePerson = GetSampleSimplePerson(0);

            var schoolList = new List<int>() { 0, 1, 2, 3, 4 };
            int total = 0;
            mockPeoplePresentation.Setup(p => p.GetPeople(schoolList, "http://localhost/", "", 0, 20, out total)).Returns(sampleSimplePerson);

            mockPeopleRepository.Setup(p => p.GetPeopleInfo("http://localhost/", sampleSimplePerson)).Returns(GetSamplePersonInfo(10));

            _sut = new PeopleController(mockSchoolsService.Object, mockPeoplePresentation.Object, mockPeopleRepository.Object);
            // Act
            var result = _sut.GetPeople(new QueryPagination() { limit = 20, offset = 0 },
                new QueryFilter() { categories = 5, channelServerIds = schoolList }, "", "http://localhost/");

            // Assert
            Assert.IsType<NoContentResult>(result);
            //Assert.IsType<OkObjectResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_PeopleEndpoint_NoUrl_Returns_NoContent()
        {
            // Arrange
            mockSchoolsService.Setup(p => p.GetSchools("", "")).Returns(GetSampleSchools(10, 10));

            var sampleSimplePerson = GetSampleSimplePerson(10);
            int total = 10;
            var schoolList = new List<int>() { 0, 1, 2, 3, 4 };
            mockPeoplePresentation.Setup(p => p.GetPeople(schoolList, "", "http://localhost/", 0, 20, out total)).Returns(sampleSimplePerson);

            mockPeopleRepository.Setup(p => p.GetPeopleInfo("", sampleSimplePerson)).Returns(GetSamplePersonInfo(10));
            
            _sut = new PeopleController(mockSchoolsService.Object, mockPeoplePresentation.Object, mockPeopleRepository.Object);
            // Act
            var result = _sut.GetPeople(new QueryPagination() { limit = 20, offset = 0 },
                new QueryFilter() { categories = 5, channelServerIds = schoolList }, "", "");

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }


        [Fact]
        public void Test_PeopleEndpoint_MissCategories_Returns_NoContent()
        {
            // Arrange
            mockSchoolsService.Setup(p => p.GetSchools("http://localhost/", "")).Returns(GetSampleSchools(10, 10));
            var sampleSimplePerson = GetSampleSimplePerson(10);

            var schoolList = new List<int>() { 0, 1, 2, 3, 4 };
            int total = 0;
            mockPeoplePresentation.Setup(p => p.GetPeople(schoolList, "", "http://localhost/", 0, 20, out total)).Returns(sampleSimplePerson);

            mockPeopleRepository.Setup(p => p.GetPeopleInfo("", sampleSimplePerson)).Returns(GetSamplePersonInfo(10));
            
            _sut = new PeopleController(mockSchoolsService.Object, mockPeoplePresentation.Object, mockPeopleRepository.Object);
            // Act
            var result = _sut.GetPeople(new QueryPagination() { limit = 20, offset = 0 },
                new QueryFilter() { categories = 0, channelServerIds = schoolList }, "", "http://localhost/");

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_PeopleEndpoint_NoPeople_Returns_NoContent()
        {
            // Arrange
            mockSchoolsService.Setup(p => p.GetSchools("http://localhost/", "")).Returns(GetSampleSchools(10, 10));
            var sampleSimplePerson = GetSampleSimplePerson(10);

            var schoolList = new List<int>() { 0, 1, 2, 3, 4 };
            int total = 0;
            mockPeoplePresentation.Setup(p => p.GetPeople(schoolList, "", "http://localhost/", 0, 20, out total)).Returns(sampleSimplePerson);

            mockPeopleRepository.Setup(p => p.GetPeopleInfo("", sampleSimplePerson)).Returns(GetSamplePersonInfo(10));
            
            _sut = new PeopleController(mockSchoolsService.Object, mockPeoplePresentation.Object, mockPeopleRepository.Object);
            // Act
            var result = _sut.GetPeople(new QueryPagination() { limit = 20, offset = 0 },
                new QueryFilter() { categories = 0, channelServerIds = schoolList }, "", "http://localhost/");

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_PeopleEndpoint_NoSchools_Returns_NoContent()
        {
            // Arrange
            mockSchoolsService.Setup(p => p.GetSchools("http://localhost/", "")).Returns(GetSampleSchools(10, 10));

            var sampleSimplePerson = GetSampleSimplePerson(0);

            var schoolList = new List<int>() { 0, 1, 2, 3, 4 };
            int total = 0;
            mockPeoplePresentation.Setup(p => p.GetPeople(schoolList, "", "http://localhost/", 0, 20, out total)).Returns(sampleSimplePerson);

            mockPeopleRepository.Setup(p => p.GetPeopleInfo("", sampleSimplePerson)).Returns(GetSamplePersonInfo(10));
            
            _sut = new PeopleController(mockSchoolsService.Object, mockPeoplePresentation.Object, mockPeopleRepository.Object);
            // Act
            var result = _sut.GetPeople(new QueryPagination() { limit = 20, offset = 0 },
                new QueryFilter() { categories = 0, channelServerIds = schoolList }, "", "http://localhost/");

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }
    }
}
