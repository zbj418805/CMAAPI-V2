using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Moq;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Repositories;
using West.Presence.CMA.Core.Servies;
using Xunit;


namespace West.Presence.CMA.Core.Tests.Services
{
    public class DBConnectionServiceTest
    {
        private Mock<IConnectionRepository> mockConnectionRepository;
        private Mock<ICacheProvider> mockCacheProvider;
        private Mock<IOptions<CMAOptions>> mockOptions;
        private IDBConnectionService dbConnectionService;

        public DBConnectionServiceTest()
        {
            CMAOptions option = new CMAOptions
            {
                CacheConnKey = "CMAConnections"
            };

            mockOptions = new Mock<IOptions<CMAOptions>>();
            mockOptions.Setup(p => p.Value).Returns(option);
        }

        [Fact]
        public void Test_GetConnection_From_Cached()
        {
            string conn = "asdfa";

            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<string>("CMAConnections_localhost", out conn)).Returns(true);

            mockConnectionRepository = new Mock<IConnectionRepository>();
            //mockConnectionRepository.Setup(p => p.GetConnection("1111")).Returns("conn_111");

            dbConnectionService = new DBConnectionService(mockCacheProvider.Object, mockOptions.Object, mockConnectionRepository.Object);
            string connStr = dbConnectionService.GetConnection("http://localhost/");

            Assert.Equal("asdfa", connStr);
        }

        [Fact]
        public void Test_GetConnection_From_Repository()
        {
            string conn = "asdfa";

            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<string>("CMAConnections_localhost", out conn)).Returns(false);

            mockConnectionRepository = new Mock<IConnectionRepository>();
            mockConnectionRepository.Setup(p => p.GetConnection("http://localhost/")).Returns("conn_111");

            dbConnectionService = new DBConnectionService(mockCacheProvider.Object, mockOptions.Object, mockConnectionRepository.Object);
            string connStr = dbConnectionService.GetConnection("http://localhost/");

            Assert.Equal("conn_111", connStr);
        }
    }
}
