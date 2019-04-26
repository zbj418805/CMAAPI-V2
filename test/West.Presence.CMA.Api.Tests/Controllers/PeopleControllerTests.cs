using Microsoft.AspNetCore.Mvc;
using Moq;
using West.Presence.CMA.Api.Controllers;
using Xunit;

namespace West.Presence.CMA.Api.Tests.Controllers
{
    public class PeopleControllerTests
    {
        private PeopleController _sut;

        [Fact]
        public void Test_PeopleEndpoint_Returns_OK()
        {
            // Arrange
            _sut = new PeopleController(null, null, null);
            // Act
            var result = _sut.GetPeople(null, null, null);

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }
    }
}
