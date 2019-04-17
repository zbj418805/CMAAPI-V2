using Microsoft.AspNetCore.Mvc;
using Moq;
using West.Presence.CMA.Api.Controllers;
using Xunit;

namespace West.Presence.CMA.Api.Tests.Controllers
{
    public class EventsControllerTests
    {
        private EventsController _sut;

        [Fact]
        public void Test_ChannelEndpoint_Returns_OK()
        {
            // Arrange
            _sut = new EventsController();
            // Act
            var result = _sut.GetEvents(null, null, null);

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }
    }
}
