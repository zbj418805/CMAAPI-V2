using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using West.Presence.CMA.Api.Controllers;
using West.Presence.CMA.Api.Model;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Presentations;
using Xunit;

namespace West.Presence.CMA.Api.Tests.Controllers
{
    public class SchoolControllerTests : BaseControllerTest
    {
        private SchoolsController _sut;

        private Mock<ISchoolsPresentation> mockSchoolsPresentation;


        public SchoolControllerTests()
        {
            mockSchoolsPresentation = new Mock<ISchoolsPresentation>();


            // mockSchoolsPresentation.Setup(p => p.GetEvents(new List<int>() { 1, 2 }, "", "", DateTime.Today, DateTime.Today)).Returns(events);
        }


        [Fact]
        public void Test_SchoolController_Returns_OK()
        {
            int total = 10;
            mockSchoolsPresentation.Setup(p => p.GetSchools("http://localhost/", "", 0, 20, out total)).Returns(GetSampleSchools(10, 10));
            // Arrange
            _sut = new SchoolsController(mockSchoolsPresentation.Object);
            // Act
            var result = _sut.GetSchools(new QueryPagination() { limit = 20, offset = 0 }, 
                new QueryFilter() { categories = 7, }, "", "http://localhost/");

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_SchoolController_wrong_categories_Returns_Notent()
        {
            int total = 10;
            mockSchoolsPresentation.Setup(p => p.GetSchools("http://localhost/", "", 0, 20, out total)).Returns(GetSampleSchools(10, 10));
            // Arrange
            _sut = new SchoolsController(mockSchoolsPresentation.Object);
            // Act
            var result = _sut.GetSchools(new QueryPagination() { limit = 20, offset = 0 },
                new QueryFilter() { categories = 0 }, "", "http://localhost/");

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_SchoolController_no_url_Returns_Notent()
        {
            int total = 10;
            mockSchoolsPresentation.Setup(p => p.GetSchools("", "", 0, 20, out total)).Returns(GetSampleSchools(10, 10));
            // Arrange
            _sut = new SchoolsController(mockSchoolsPresentation.Object);
            // Act
            var result = _sut.GetSchools(new QueryPagination() { limit = 20, offset = 0 },
                new QueryFilter() { categories = 0 }, "", "");

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_SchoolController_no_schools_Returns_Notent()
        {
            int total = 10;
            mockSchoolsPresentation.Setup(p => p.GetSchools("", "", 0, 20, out total)).Returns(GetSampleSchools(0, 10));
            // Arrange
            _sut = new SchoolsController(mockSchoolsPresentation.Object);
            // Act
            var result = _sut.GetSchools(new QueryPagination() { limit = 20, offset = 0 },
                new QueryFilter() { categories = 0 }, "", "");

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_SchoolController_district_is_0_Returns_Notent()
        {
            int total = 10;
            mockSchoolsPresentation.Setup(p => p.GetSchools("", "", 0, 20, out total)).Returns(GetSampleSchools(10, 0));
            // Arrange
            _sut = new SchoolsController(mockSchoolsPresentation.Object);
            // Act
            var result = _sut.GetSchools(new QueryPagination() { limit = 20, offset = 0 },
                new QueryFilter() { categories = 0 }, "", "");

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }
    }
}
