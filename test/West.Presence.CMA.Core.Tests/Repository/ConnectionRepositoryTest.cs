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
    public class ConnectionRepositoryTest
    {
        private Mock<IHttpClientProvider> mockHttpClientProvider;

        public ConnectionRepositoryTest()
        {

        }

        [Fact]
        public void Test_GetConnection_Ok() {
            string connectionStr = "testString";

            mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.Setup(p => p.GetSingleData<string>("/cma/appsettings", "CentralServerApi")).Returns(connectionStr);

            APIConectionRepository channelRepo = new APIConectionRepository(mockHttpClientProvider.Object);

            string connStr = channelRepo.GetConnection("/cma/appsettings");

            Assert.Equal(connectionStr, connStr);

        }

        [Fact]
        public void Test_GetConnection_Get_Empty()
        {
            //string connectionStr = "testString";

            mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.Setup(p => p.GetSingleData<string>("/cma/appsettings", "CentralServerApi")).Returns("");

            APIConectionRepository channelRepo = new APIConectionRepository(mockHttpClientProvider.Object);

            string connStr = channelRepo.GetConnection("/cma/appsettings");

            Assert.Empty(connStr);

        }
    }
}
