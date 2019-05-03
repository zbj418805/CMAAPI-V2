using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using West.Presence.CMA.Api.Controllers;
using West.Presence.CMA.Api.Model;
using West.Presence.CMA.Core.Repositories;
using Xunit;


namespace West.Presence.CMA.Api.Controllers.Tests
{
    public class ChannelToGroupControllerTests : BaseControllerTest
    {
        private ChannelToGroupController _sut;
        private readonly Mock<IChannel2GroupRepository> mockIChannel2GroupRepository;

        public ChannelToGroupControllerTests()
        {
            mockIChannel2GroupRepository = new Mock<IChannel2GroupRepository>();
        }

        [Fact]
        public void Test_GetChannelsToGroups_Returns_OK()
        {
            // Arrange
            mockIChannel2GroupRepository.Setup(p => p.GetChannel2Group("http://localhost/", 0)).Returns(GetSamepleC2Gs(5));


            var appid = 123;
            _sut = new ChannelToGroupController(mockIChannel2GroupRepository.Object);


            // Act
            var result = _sut.GetChannelsToGroups(appid, "http://localhost/");

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_GetChannelsToGroups_noc2g_Returns_Nocontent()
        {
            // Arrange
            mockIChannel2GroupRepository.Setup(p => p.GetChannel2Group("http://localhost/", 0)).Returns(GetSamepleC2Gs(5));


            var appid = 123;
            _sut = new ChannelToGroupController(mockIChannel2GroupRepository.Object);


            // Act
            var result = _sut.GetChannelsToGroups(appid, "http://noc2gs/");

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        //[Fact]
        public void Test_SetChannelsToGroups_Returns_OK()
        {
            // Arrange
            var c2gs = GetSamepleC2Gs(5);
            var appid = 123;
            var endpoint = "http://endpoint";
            var sessionId = "seccsioid";
            Dictionary<int, int> dicC2Gs = new Dictionary<int, int>();
            dicC2Gs.Add(1, 3);
            dicC2Gs.Add(4, 8);

            var value = new
            {
                data = new {
                    channelsToGroups = dicC2Gs,
                    endpoint = endpoint,
                    sessionId = sessionId
                }
            };

            mockIChannel2GroupRepository.Setup(p => p.SetChannel2Group("http://localhost/", 0, c2gs, appid, endpoint, sessionId));

            _sut = new ChannelToGroupController(mockIChannel2GroupRepository.Object);

            // Act

            var result = _sut.SetChannelToGroup(appid, value, "http://localhost/");

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        //[Fact]
        public void Test_DeleteChannelsToGroups_Return_OK()
        {
            // Arrange
            var c2gs = GetSamepleC2Gs(5);
            var appid = 123;
            var endpointUrl = "http://endpoint";
            var sessionId = "seccsiondi";
            Dictionary<int, int> dicC2Gs = new Dictionary<int, int>();
            dicC2Gs.Add(1, 3);
            dicC2Gs.Add(4, 8);

            var serverList = new List<int> { 1, 2 };

            mockIChannel2GroupRepository.Setup(p => p.SetChannel2Group("http://localhost/", 0, c2gs, appid, endpointUrl, sessionId));

            _sut = new ChannelToGroupController(mockIChannel2GroupRepository.Object);

            // Act

            var result = _sut.DeleteChannelToGroup(appid, new QueryFilter() { categories = 1, channelServerIds = serverList }, "http://localhost/");

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }
    }
}
