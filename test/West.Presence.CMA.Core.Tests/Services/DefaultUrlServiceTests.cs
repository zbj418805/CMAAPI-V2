using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Repositories;
using West.Presence.CMA.Core.Servies;
using Xunit;
using Moq;

namespace West.Presence.CMA.Core.Services.Tests
{
    public class DefaultUrlServiceTests
    {
        private Mock<IDefaultUrlRepository> mockDefaultUrlRepository;
        private Mock<ICacheProvider> mockCacheProvider;
        private Mock<IOptions<CMAOptions>> mockOptions;
        private IDefaultUrlService defaultUrlService;

        public DefaultUrlServiceTests()
        {
            CMAOptions option = new CMAOptions
            {
                CacheDefaultUrlsKey = "CMADefaultUrl",
                CacheDefaultUrlsDurationInSceconds = 800
            };

            mockOptions = new Mock<IOptions<CMAOptions>>();
            mockOptions.Setup(p => p.Value).Returns(option);
        }

        [Fact]
        public void Test_DefaultUrl_From_Cache()
        {
            var baseUrl = "http://localhost/";
            var connectionStr = "fake_connection_string";
            List<ServerDefaultUrl> lsUrls = new List<ServerDefaultUrl>();
            lsUrls.Add(new ServerDefaultUrl() { defaultUrl = "http://test.url/", serverId = 1 });
            var retUrlSet = lsUrls.AsEnumerable();

            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<ServerDefaultUrl>>("CMADefaultUrl_localhost", out retUrlSet)).Returns(true);

            mockDefaultUrlRepository = new Mock<IDefaultUrlRepository>();
            //mockPeopleSettingsRepository.Setup(p => p.GetPeopleSettings(1, connectionStr)).Returns(new PeopleSettings() { SelectGroups = "1234", ExcludedUser = "11", HiddenAttributres = "dks,da", serverId=1 });

            defaultUrlService = new DefaultUrlService(mockCacheProvider.Object, mockOptions.Object, mockDefaultUrlRepository.Object);

            var url = defaultUrlService.GetDefaultUrl(1, baseUrl, connectionStr);

            Assert.NotNull(url);
            Assert.Equal("http://test.url/", url);

        }

        [Fact]
        public void Test_DefaultUrl_From_Cache_Need_AddItem()
        {
            var baseUrl = "http://localhost/";
            var connectionStr = "fake_connection_string";
            List<ServerDefaultUrl> lsUrls = new List<ServerDefaultUrl>();
            lsUrls.Add(new ServerDefaultUrl() { defaultUrl = "http://test.url/", serverId = 1 });
            var retUrlSet = lsUrls.AsEnumerable();

            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<ServerDefaultUrl>>("CMADefaultUrl_localhost", out retUrlSet)).Returns(true);

            mockDefaultUrlRepository = new Mock<IDefaultUrlRepository>();
            mockDefaultUrlRepository.Setup(p => p.GetDefaultUrl(2, connectionStr)).Returns("http://test2.url");

            defaultUrlService = new DefaultUrlService(mockCacheProvider.Object, mockOptions.Object, mockDefaultUrlRepository.Object);

            var url = defaultUrlService.GetDefaultUrl(2, baseUrl, connectionStr);

            Assert.NotNull(url);
            Assert.Equal("http://test2.url", url);
        }

        [Fact]
        public void Test_DefaultUrl_From_Repo()
        {
            var baseUrl = "http://localhost/";
            var connectionStr = "fake_connection_string";
            List<ServerDefaultUrl> lsUrls = new List<ServerDefaultUrl>();
            //lsUrls.Add(new ServerDefaultUrl() { defaultUrl = "http://test.url/", serverId = 1 });
            var retUrlSet = lsUrls.AsEnumerable();

            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<ServerDefaultUrl>>("CMADefaultUrl_localhost", out retUrlSet)).Returns(false);

            mockDefaultUrlRepository = new Mock<IDefaultUrlRepository>();
            mockDefaultUrlRepository.Setup(p => p.GetDefaultUrl(2, connectionStr)).Returns("http://test2.url");

            defaultUrlService = new DefaultUrlService(mockCacheProvider.Object, mockOptions.Object, mockDefaultUrlRepository.Object);

            var url = defaultUrlService.GetDefaultUrl(2, baseUrl, connectionStr);

            Assert.NotNull(url);
            Assert.Equal("http://test2.url", url);
        }

        [Fact]
        public void Test_DefaultUrl_Cahce_Not_Exist_From_Repo_Return_Empty()
        {
            var baseUrl = "http://localhost/";
            var connectionStr = "fake_connection_string";
            List<ServerDefaultUrl> lsUrls = new List<ServerDefaultUrl>();
            //lsUrls.Add(new ServerDefaultUrl() { defaultUrl = "http://test.url/", serverId = 1 });
            var retUrlSet = lsUrls.AsEnumerable();

            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<ServerDefaultUrl>>("CMADefaultUrl_localhost", out retUrlSet)).Returns(false);

            mockDefaultUrlRepository = new Mock<IDefaultUrlRepository>();
            mockDefaultUrlRepository.Setup(p => p.GetDefaultUrl(2, connectionStr)).Returns("");

            defaultUrlService = new DefaultUrlService(mockCacheProvider.Object, mockOptions.Object, mockDefaultUrlRepository.Object);

            var url = defaultUrlService.GetDefaultUrl(2, baseUrl, connectionStr);

            Assert.NotNull(url);
            Assert.Empty(url);
        }

        [Fact]
        public void Test_DefaultUrl_Cahce_From_Repo_Return_Empty()
        {
            var baseUrl = "http://localhost/";
            var connectionStr = "fake_connection_string";
            List<ServerDefaultUrl> lsUrls = new List<ServerDefaultUrl>();
            //lsUrls.Add(new ServerDefaultUrl() { defaultUrl = "http://test.url/", serverId = 1 });
            var retUrlSet = lsUrls.AsEnumerable();

            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<ServerDefaultUrl>>("CMADefaultUrl_localhost", out retUrlSet)).Returns(true);

            mockDefaultUrlRepository = new Mock<IDefaultUrlRepository>();
            mockDefaultUrlRepository.Setup(p => p.GetDefaultUrl(2, connectionStr)).Returns("");

            defaultUrlService = new DefaultUrlService(mockCacheProvider.Object, mockOptions.Object, mockDefaultUrlRepository.Object);

            var url = defaultUrlService.GetDefaultUrl(2, baseUrl, connectionStr);

            Assert.NotNull(url);
            Assert.Empty(url);
        }
    }
}
