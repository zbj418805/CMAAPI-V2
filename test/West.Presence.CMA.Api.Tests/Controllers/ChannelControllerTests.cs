using Microsoft.AspNetCore.Mvc;
using Moq;
using West.Presence.CMA.Api.Controllers;
using West.Presence.CMA.Core.Servies;
using Xunit;

namespace West.Presence.CMA.Api.Controllers.Tests
{
    public class ChannelControllerTests : BaseControllerTest
    {
        private ChannelsController _sut;
        private readonly Mock<ISchoolsService> mockSchoolsService;

        public ChannelControllerTests()
        {
            mockSchoolsService = new Mock<ISchoolsService>();
            mockSchoolsService.Setup(p => p.GetSchools("http://localhost/", "")).Returns(GetSampleSchools(10, 10));
            mockSchoolsService.Setup(p => p.GetSchools("http://noschools/", "")).Returns(GetSampleSchools(0, 10));
        }

        [Fact]
        public void Test_ChannelEndpoint_Returns_OK()
        {
            // Arrange

            _sut = new ChannelsController(mockSchoolsService.Object);
            // Act
            var result = _sut.GetChannels("http://localhost/");

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_ChannelEndpoint_WithUrlNotbackslash_Returns_OK()
        {
            // Arrange

            _sut = new ChannelsController(mockSchoolsService.Object);
            // Act
            var result = _sut.GetChannels("http://localhost");

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_ChannelEndpoint_NoUrl_Returns_Contents()
        {
            // Arrange

            _sut = new ChannelsController(mockSchoolsService.Object);
            // Act
            var result = _sut.GetChannels("");

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_ChannelEndpoint_NoSchools_Returns_Contents()
        {
            // Arrange

            _sut = new ChannelsController(mockSchoolsService.Object);
            // Act
            var result = _sut.GetChannels("http://noschools/");

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }
    }
}
