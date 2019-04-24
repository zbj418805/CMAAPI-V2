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
            mockHttpClientProvider.Setup(p => p.GetData<Channel2Group>("http://test.url//presence/Api/CMA/Channel2Groups")).Returns(lsC2G);

            APIChannel2GroupRepository channelRepo = new APIChannel2GroupRepository(mockHttpClientProvider.Object);

            var resultChannel2Group = channelRepo.GetChannel2Group("http://test.url/");

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
            mockHttpClientProvider.Setup(p => p.GetData<Channel2Group>("http://test.url//presence/Api/CMA/Channel2Groupss")).Returns(lsC2G);

            APIChannel2GroupRepository channelRepo = new APIChannel2GroupRepository(mockHttpClientProvider.Object);

            var resultChannel2Group = channelRepo.GetChannel2Group("http://test.url/");

            Assert.NotNull(resultChannel2Group);

            Assert.Empty(resultChannel2Group);
        }

        [Fact]
        public void Test_Channel2Group_Set_Channel2Group()
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
            mockHttpClientProvider.Setup(p => p.PostData<Channel2Group>("http://test.url//presence/Api/CMA/Channel2Groups", null)).Returns(c2g);

            APIChannel2GroupRepository channelRepo = new APIChannel2GroupRepository(mockHttpClientProvider.Object);

            var resultChannel2Group = channelRepo.SetChannel2Group("http://test.url/", null);

            Assert.NotNull(resultChannel2Group);

            Assert.Equal(10, resultChannel2Group.Count());
        }


        [Fact]
        public void Test_Channel2Group_Delete_Channel2Group_Return_True()
        {
            mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.Setup(p => p.DeletetData("http://test.url//presence/Api/CMA/Channel2Groups")).Returns(true);


            APIChannel2GroupRepository channelRepo = new APIChannel2GroupRepository(mockHttpClientProvider.Object);

            var resultChannel2Group = channelRepo.DeleteChannel2Group("http://test.url/");

            Assert.True(resultChannel2Group);

            //Assert.Equal(10, resultChannel2Group.Count());
        }


        [Fact]
        public void Test_Channel2Group_Delete_Channel2Group_Return_False()
        {
            mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.Setup(p => p.DeletetData("http://test.url//presence/Api/CMA/Channel2Groups")).Returns(false);


            APIChannel2GroupRepository channelRepo = new APIChannel2GroupRepository(mockHttpClientProvider.Object);

            var resultChannel2Group = channelRepo.DeleteChannel2Group("http://test.url//presence/Api/CMA/Channel2Groups");

            Assert.False(resultChannel2Group);
        }
    }
}
