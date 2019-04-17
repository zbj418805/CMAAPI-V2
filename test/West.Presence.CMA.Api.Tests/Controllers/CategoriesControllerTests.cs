using Microsoft.AspNetCore.Mvc;
using Moq;
using West.Presence.CMA.Api.Controllers;
using Xunit;

namespace West.Presence.CMA.Api.Tests.Controllers
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
            var result = _sut.GetAll();

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }
    }
}
