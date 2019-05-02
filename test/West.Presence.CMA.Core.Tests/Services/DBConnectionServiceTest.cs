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
            List<Connection> conns = new List<Connection>();
            conns.Add(new Connection() { baseUrl = "1111", connectionString = "conn_111"});
            conns.Add(new Connection() { baseUrl = "2222", connectionString = "conn_222"});
            IEnumerable<Connection> connE = conns.AsEnumerable();

            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<Connection>>("CMAConnections", out connE)).Returns(true);

            mockConnectionRepository = new Mock<IConnectionRepository>();
            //mockConnectionRepository.Setup(p => p.GetConnection("1111")).Returns("conn_111");

            dbConnectionService = new DBConnectionService(mockCacheProvider.Object, mockOptions.Object, mockConnectionRepository.Object);
            string connStr = dbConnectionService.GetConnection("1111");

            Assert.Equal("conn_111", connStr);
        }

        [Fact]
        public void Test_GetConnection_From_Repository()
        {
            List<Connection> conns = new List<Connection>();
            //conns.Add(new Connection() { baseUrl = "1111", connectionString = "conn_111" });
            //conns.Add(new Connection() { baseUrl = "2222", connectionString = "conn_222" });
            IEnumerable<Connection> connE = conns.AsEnumerable();

            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<Connection>>("CMAConnections", out connE)).Returns(false);

            mockConnectionRepository = new Mock<IConnectionRepository>();
            mockConnectionRepository.Setup(p => p.GetConnection("1111")).Returns("conn_111");

            dbConnectionService = new DBConnectionService(mockCacheProvider.Object, mockOptions.Object, mockConnectionRepository.Object);
            string connStr = dbConnectionService.GetConnection("1111");

            Assert.Equal("conn_111", connStr);
        }

        [Fact]
        public void Test_GetConnection_CacheNotExist_Then_GET_Repository()
        {
            List<Connection> conns = new List<Connection>();
            conns.Add(new Connection() { baseUrl = "1111", connectionString = "conn_111" });
            conns.Add(new Connection() { baseUrl = "2222", connectionString = "conn_222" });
            IEnumerable<Connection> connE = conns.AsEnumerable();

            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<Connection>>("CMAConnections", out connE)).Returns(true);

            mockConnectionRepository = new Mock<IConnectionRepository>();
            mockConnectionRepository.Setup(p => p.GetConnection("3333")).Returns("conn_333");

            dbConnectionService = new DBConnectionService(mockCacheProvider.Object, mockOptions.Object, mockConnectionRepository.Object);
            string connStr = dbConnectionService.GetConnection("3333");

            Assert.Equal("conn_333", connStr);
        }
    }
}
