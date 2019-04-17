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
            Assert.IsType<OkResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_GetNewsSchemaEndpoint_Returns_OK()
        {
            // Arrange

            // Act
            var result = _sut.GetNewsSchema();

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_GetEventsSchemaEndpoint_Returns_OK()
        {
            // Arrange

            // Act
            var result = _sut.GetEventsSchema();

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_GetSchoolsSchemaEndpoint_Returns_OK()
        {
            // Arrange

            // Act
            var result = _sut.GetSchoolsSchema();

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_GetPeopleSchemaEndpoint_Returns_OK()
        {
            // Arrange

            // Act
            var result = _sut.GetPeopleSchema();

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }
    }
}
