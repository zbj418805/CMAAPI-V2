using Microsoft.AspNetCore.Mvc;
using Moq;
using West.Presence.CMA.Api.Controllers;
using Xunit;

namespace West.Presence.CMA.Api.Tests.Controllers
{
    public class SchemaControllerTests
    {
        private SchemaController _sut;

        public SchemaControllerTests()
        {
            _sut = new SchemaController();
        }

        [Fact]
        public void Test_GetAllSchemaEndpoint_Returns_OK()
        {
            // Arrange
            
            // Act
            var result = _sut.AllSchemas();

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_GetNewsSchemaEndpoint_Returns_OK()
        {
            // Arrange

            // Act
            var result = _sut.NewsSchema();

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_GetEventsSchemaEndpoint_Returns_OK()
        {
            // Arrange

            // Act
            var result = _sut.EventsSchema();

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_GetSchoolsSchemaEndpoint_Returns_OK()
        {
            // Arrange

            // Act
            var result = _sut.SchoolsSchema();

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_GetPeopleSchemaEndpoint_Returns_OK()
        {
            // Arrange

            // Act
            var result = _sut.PeopleSchema();

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }
    }
}
