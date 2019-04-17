using Microsoft.AspNetCore.Mvc;
using Moq;
using West.Presence.CMA.Api.Controllers;
using Xunit;

namespace West.Presence.CMA.Api.Tests.Controllers
{
    public class NewsControllerTests
    {
        private NewsController _sut;

        [Fact]
        public void Test_NewsEndpoint_Returns_OK()
        {
            // Arrange
            _sut = new NewsController();
            // Act
            var result = _sut.GetNews(null, null, null);

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }
    }
}
