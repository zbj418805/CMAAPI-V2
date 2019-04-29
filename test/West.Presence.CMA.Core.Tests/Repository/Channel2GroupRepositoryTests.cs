using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Repositories;
using Xunit;

namespace West.Presence.CMA.Core.Tests.Repository
{
    public class Channel2GroupRepositoryTests
    {
        private Mock<IHttpClientProvider> mockHttpClientProvider;

        public Channel2GroupRepositoryTests()
        {

        }

        [Fact]
        public void Test_Channel2Group_Get_Channel2Group()
        {
            List<Channel2Group> lsC2G = new List<Channel2Group>();
            for (int i = 0; i < 10; i++)
            {
                lsC2G.Add(new Channel2Group()
                {
                    channelId = 123
                });
            }
            var c2g = lsC2G.AsEnumerable();

            mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.Setup(p => p.GetData<Channel2Group>("http://test.url/webapi/cma/channel2group/0")).Returns(lsC2G);

            APIChannel2GroupRepository channelRepo = new APIChannel2GroupRepository(mockHttpClientProvider.Object);

            var resultChannel2Group = channelRepo.GetChannel2Group("http://test.url/", 0);

            Assert.NotNull(resultChannel2Group);

            Assert.Equal(10, resultChannel2Group.Count());
        }

        [Fact]
        public void Test_Channel2Group_Get_No_Channel2Group()
        {
            List<Channel2Group> lsC2G = new List<Channel2Group>();
            for (int i = 0; i < 10; i++)
            {
                lsC2G.Add(new Channel2Group()
                {
                    channelId = 123
                });
            }
            var c2g = lsC2G.AsEnumerable();

            mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.Setup(p => p.GetData<Channel2Group>("http://test.url/presence/Api/CMA/Channel2Groupss")).Returns(lsC2G);

            APIChannel2GroupRepository channelRepo = new APIChannel2GroupRepository(mockHttpClientProvider.Object);

            var resultChannel2Group = channelRepo.GetChannel2Group("http://test.url/", 0);

            Assert.NotNull(resultChannel2Group);

            Assert.Empty(resultChannel2Group);
        }
    }
}
