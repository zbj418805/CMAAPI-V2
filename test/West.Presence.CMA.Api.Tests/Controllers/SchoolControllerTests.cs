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
    public class SchoolControllerTests
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
            List<School> schools = new List<School>();

            for (int i = 0; i < 10; i++)
            {
                schools.Add(new School()
                {
                    Name = $"School Name {i}",
                    Description = $"Description {1} ...",
                    DistrictServerId = 10
                });
            }

            int total = 10;
            mockSchoolsPresentation.Setup(p => p.GetSchools("http://localhost/", "", 0, 20, out total)).Returns(schools);
            // Arrange
            _sut = new SchoolsController(mockSchoolsPresentation.Object);
            // Act
            var result = _sut.GetSchools(new QueryPagination() { Limit = 20, Offset = 0 }, 
                new QueryFilter() { Categories = 7, }, "", "http://localhost/");

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_SchoolController_wrong_categories_Returns_Notent()
        {
            List<School> schools = new List<School>();

            for (int i = 0; i < 10; i++)
            {
                schools.Add(new School()
                {
                    Name = $"School Name {i}",
                    Description = $"Description {1} ...",
                    DistrictServerId = 10
                });
            }

            int total = 10;
            mockSchoolsPresentation.Setup(p => p.GetSchools("http://localhost/", "", 0, 20, out total)).Returns(schools);
            // Arrange
            _sut = new SchoolsController(mockSchoolsPresentation.Object);
            // Act
            var result = _sut.GetSchools(new QueryPagination() { Limit = 20, Offset = 0 },
                new QueryFilter() { Categories = 0 }, "", "http://localhost/");

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_SchoolController_no_url_Returns_Notent()
        {
            List<School> schools = new List<School>();

            for (int i = 0; i < 10; i++)
            {
                schools.Add(new School()
                {
                    Name = $"School Name {i}",
                    Description = $"Description {1} ...",
                    DistrictServerId = 10
                });
            }

            int total = 10;
            mockSchoolsPresentation.Setup(p => p.GetSchools("", "", 0, 20, out total)).Returns(schools);
            // Arrange
            _sut = new SchoolsController(mockSchoolsPresentation.Object);
            // Act
            var result = _sut.GetSchools(new QueryPagination() { Limit = 20, Offset = 0 },
                new QueryFilter() { Categories = 0 }, "", "");

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_SchoolController_no_schools_Returns_Notent()
        {
            List<School> schools = new List<School>();

            //for (int i = 0; i < 10; i++)
            //{
            //    schools.Add(new School()
            //    {
            //        Name = $"School Name {i}",
            //        Description = $"Description {1} ...",
            //        DistrictServerId = 10
            //    });
            //}

            int total = 10;
            mockSchoolsPresentation.Setup(p => p.GetSchools("", "", 0, 20, out total)).Returns(schools);
            // Arrange
            _sut = new SchoolsController(mockSchoolsPresentation.Object);
            // Act
            var result = _sut.GetSchools(new QueryPagination() { Limit = 20, Offset = 0 },
                new QueryFilter() { Categories = 0 }, "", "");

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_SchoolController_district_is_0_Returns_Notent()
        {
            List<School> schools = new List<School>();

            for (int i = 0; i < 10; i++)
            {
                schools.Add(new School()
                {
                    Name = $"School Name {i}",
                    Description = $"Description {1} ...",
                    DistrictServerId = 0
                });
            }

            int total = 10;
            mockSchoolsPresentation.Setup(p => p.GetSchools("", "", 0, 20, out total)).Returns(schools);
            // Arrange
            _sut = new SchoolsController(mockSchoolsPresentation.Object);
            // Act
            var result = _sut.GetSchools(new QueryPagination() { Limit = 20, Offset = 0 },
                new QueryFilter() { Categories = 0 }, "", "");

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }
    }
}
