using Microsoft.AspNetCore.Mvc;
using Moq;
using West.Presence.CMA.Api.Controllers;
using Xunit;


namespace West.Presence.CMA.Api.Tests.Controllers
{
    public class ChannelToGroupControllerTests
    {
        private ChannelToGroupController _sut;

        [Fact]
        public void Test_ChannelToGroupEndpoint_Returns_OK()
        {
            var appid = 123;
            
            // Arrange
            _sut = new ChannelToGroupController();
            // Act
            var result = _sut.GetChannelsToGroups(appid);

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }
    }
}
