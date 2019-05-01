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

            AppSettings setting = new AppSettings()
            {
                Channel2Groups = lsC2G,
                appId = 12,
                endpoint = "asd",
                sessionId = "asdfasdf"
            };

            mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.Setup(p => p.GetSingleData<AppSettings>("http://test.url/webapi/cma/appsettings")).Returns(setting);

            APIChannel2GroupRepository channelRepo = new APIChannel2GroupRepository(mockHttpClientProvider.Object);

            var resultChannel2Group = channelRepo.GetChannel2Group("http://test.url/", 0);

            Assert.NotNull(resultChannel2Group);

            Assert.Equal(10, resultChannel2Group.Count());
        }

        [Fact]
        public void Test_Channel2Group_Get_No_Channel2Group()
        {
            AppSettings setting = new AppSettings()
            {
                Channel2Groups = null,
                appId = 12,
                endpoint = "asd",
                sessionId = "asdfasdf"
            };

            mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.Setup(p => p.GetSingleData<AppSettings>("http://test.url/webapi/cma/appsettings")).Returns(setting);

            APIChannel2GroupRepository channelRepo = new APIChannel2GroupRepository(mockHttpClientProvider.Object);

            var resultChannel2Group = channelRepo.GetChannel2Group("http://test.url/", 0);

            Assert.Null(resultChannel2Group);

            //Assert.Empty(resultChannel2Group);
        }
    }
}
