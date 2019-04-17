using Microsoft.AspNetCore.Mvc;
using Moq;
using West.Presence.CMA.Api.Controllers;
using Xunit;

namespace West.Presence.CMA.Api.Tests.Controllers
{
    public class SchoolControllerTests
    {
        private SchoolsController _sut;

        [Fact]
        public void Test_HealthEndpoint_Returns_OK()
        {
            // Arrange
            _sut = new SchoolsController();
            // Act
            var result = _sut.GetSchools(null, null, null);

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }
    }
}
