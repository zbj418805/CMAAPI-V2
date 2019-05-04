using System.Data;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Repositories;
using Moq;
using Xunit;

namespace West.Presence.CMA.Core.Repository.Tests
{
    public class DefaultUrlRepositoryTests
    {
        private Mock<IDatabaseProvider> mockDatabaseProvider;
        private IDefaultUrlRepository defaultUrlRepository;

        public DefaultUrlRepositoryTests()
        {
            mockDatabaseProvider = new Mock<IDatabaseProvider>();
        }

        [Fact]
        public void Test_DefaultUrlReposity_Okay()
        {
            string connectionString = "fake_connection_string";
            string sqlscript_GetDefaultURL = "SELECT TOP 1 url FROM click_server_urls WHERE default_p=1 AND server_id=@serverId";
            string retUrl = "returnUrl";
            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDatabaseProvider.Setup(p => p.GetCellValue<string>(connectionString, sqlscript_GetDefaultURL, It.IsAny<object>(), CommandType.Text)).Returns(retUrl);

            defaultUrlRepository = new DefaultUrlRepository(mockDatabaseProvider.Object);

            var connString = defaultUrlRepository.GetDefaultUrl(1234, connectionString);

            Assert.NotEmpty(connString);
            Assert.Equal(retUrl, connString);
        }

        [Fact]
        public void Test_DefaultUrlReposity_noserverId_return_empty()
        {
            string connectionString = "fake_connection_string";
            string sqlScript = "SELECT TOP 1 url FROM click_server_urls WHERE default_p=1 AND server_id=@serverId";
            string retUrl = "returnUrl";
            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDatabaseProvider.Setup(p => p.GetCellValue<string>(connectionString, sqlScript, It.IsAny<object>(), CommandType.Text)).Returns(retUrl);

            defaultUrlRepository = new DefaultUrlRepository(mockDatabaseProvider.Object);

            var connString = defaultUrlRepository.GetDefaultUrl(0, connectionString);

            Assert.Empty(connString);
        }

        [Fact]
        public void Test_DefaultUrlReposity_no_connection_string_return_empty()
        {
            string connectionString = "fake_connection_string";
            string sqlScript = "SELECT TOP 1 url FROM click_server_urls WHERE default_p=1 AND server_id=@serverId";
            string retUrl = "returnUrl";
            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDatabaseProvider.Setup(p => p.GetCellValue<string>(connectionString, sqlScript, It.IsAny<object>(), CommandType.Text)).Returns(retUrl);

            defaultUrlRepository = new DefaultUrlRepository(mockDatabaseProvider.Object);

            var connString = defaultUrlRepository.GetDefaultUrl(0, connectionString);

            Assert.Empty(connString);
        }
    }
}
