using Microsoft.AspNetCore.Mvc;
using Moq;
using West.Presence.CMA.Api.Controllers;
using West.Presence.CMA.Api.Model;
using Xunit;

namespace West.Presence.CMA.Api.Controllers.Tests
{
    public class CategoriesControllerTests
    {
        private CategoriesController _sut;

        [Fact]
        public void Test_CategoriesEndpoint_Returns_OK()
        {
            // Arrange
            _sut = new CategoriesController();
            // Act
            var result = _sut.GetAll(new QueryFilter());

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }
    }
}
