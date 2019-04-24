using System.Collections.Generic;
using System.Linq;
using Moq;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Repositories;
using Xunit;

namespace West.Presence.CMA.Core.Tests.Repository
{
    public class ChannelRepositoryTests
    {
        private Mock<IHttpClientProvider> mockHttpClientProvider;
        public ChannelRepositoryTests()
        {

        }

        [Fact]
        public void Test_Channel_Repository_Get_Channels()
        {
            List<Channel> lsChannels = new List<Channel>();
            for (int i = 0; i < 10; i++)
            {
                lsChannels.Add(new Channel()
                {
                     channelId = 123
                });
            }
            var channels = lsChannels.AsEnumerable();

            mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.Setup(p => p.GetData<Channel>("http://test.url//presence/Api/CMA/Channels")).Returns(channels);

            APIChannelsRepository channelRepo = new APIChannelsRepository(mockHttpClientProvider.Object);

            var resultChannels = channelRepo.GetChannels("http://test.url/");

            Assert.NotNull(resultChannels);

            Assert.Equal(10, resultChannels.Count());
        }

        [Fact]
        public void Test_Channel_Repository_Get_NO_Channels()
        {
            List<Channel> lsChannels = new List<Channel>();
            for (int i = 0; i < 10; i++)
            {
                lsChannels.Add(new Channel()
                {
                    channelId = 123
                });
            }
            var channels = lsChannels.AsEnumerable();

            mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.Setup(p => p.GetData<Channel>("http://test.url//presence/Api/CMA/Channel")).Returns(channels);

            APIChannelsRepository channelRepo = new APIChannelsRepository(mockHttpClientProvider.Object);

            var resultChannels = channelRepo.GetChannels("http://test.url/");

            Assert.NotNull(resultChannels);

            Assert.Empty(resultChannels);
        }
    }
}
