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
    public class PeopleSettingsServiceTests
    {
        private Mock<IPeopleSettingsRepository> mockPeopleSettingsRepository;
        private Mock<ICacheProvider> mockCacheProvider;
        private Mock<IOptions<CMAOptions>> mockOptions;
        private IPeopleSettingsService peopleSettingsService;

        public PeopleSettingsServiceTests()
        {
            CMAOptions option = new CMAOptions
            {
                CachePeopleSettingsKey = "CMAPeopleSetting"
            };

            mockOptions = new Mock<IOptions<CMAOptions>>();
            mockOptions.Setup(p => p.Value).Returns(option);
        }

        [Fact]
        public void Test_PeopleSetting_From_Cache()
        {
            var baseUrl = "http://localhost/";
            var connectionStr = "fake_connection_string";
            List<PeopleSettings> lsSettings = new List<PeopleSettings>();
            lsSettings.Add(new PeopleSettings() { SelectGroups = "1234", ExcludedUser = "11", HiddenAttributres = "dks,da", serverId = 1 });
            var retSettings = lsSettings.AsEnumerable();

            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<PeopleSettings>>("CMAPeopleSetting_localhost", out retSettings)).Returns(true);

            mockPeopleSettingsRepository = new Mock<IPeopleSettingsRepository>();
            //mockPeopleSettingsRepository.Setup(p => p.GetPeopleSettings(1, connectionStr)).Returns(new PeopleSettings() { SelectGroups = "1234", ExcludedUser = "11", HiddenAttributres = "dks,da", serverId=1 });

            peopleSettingsService = new PeopleSettingsService(mockCacheProvider.Object, mockOptions.Object, mockPeopleSettingsRepository.Object);

            var settings = peopleSettingsService.GetPeopleSettings(1, baseUrl, connectionStr);

            Assert.NotNull(settings);

        }

        [Fact]
        public void Test_PeopleSetting_From_Cache_Need_AddItem()
        {
            var baseUrl = "http://localhost/";
            var connectionStr = "fake_connection_string";
            List<PeopleSettings> lsSettings = new List<PeopleSettings>();
            lsSettings.Add(new PeopleSettings() { SelectGroups = "1234", ExcludedUser = "11", HiddenAttributres = "dks,da", serverId = 1 });
            var retSettings = lsSettings.AsEnumerable();

            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<PeopleSettings>>("CMAPeopleSetting_localhost", out retSettings)).Returns(true);

            mockPeopleSettingsRepository = new Mock<IPeopleSettingsRepository>();
            mockPeopleSettingsRepository.Setup(p => p.GetPeopleSettings(2, connectionStr)).Returns(new PeopleSettings() { SelectGroups = "1234", ExcludedUser = "11", HiddenAttributres = "dks,da", serverId=2 });

            peopleSettingsService = new PeopleSettingsService(mockCacheProvider.Object, mockOptions.Object, mockPeopleSettingsRepository.Object);

            var settings = peopleSettingsService.GetPeopleSettings(2, baseUrl, connectionStr);

            Assert.NotNull(settings);

        }

        [Fact]
        public void Test_PeopleSetting_From_Repo()
        {
            var baseUrl = "http://localhost/";
            var connectionStr = "fake_connection_string";
            List<PeopleSettings> lsSettings = new List<PeopleSettings>();
            //lsSettings.Add(new PeopleSettings() { SelectGroups = "1234", ExcludedUser = "11", HiddenAttributres = "dks,da", serverId = 1 });
            var retSettings = lsSettings.AsEnumerable();

            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<PeopleSettings>>("CMAPeopleSetting_localhost", out retSettings)).Returns(false);

            mockPeopleSettingsRepository = new Mock<IPeopleSettingsRepository>();
            mockPeopleSettingsRepository.Setup(p => p.GetPeopleSettings(1, connectionStr)).Returns(new PeopleSettings() { SelectGroups = "1234", ExcludedUser = "11", HiddenAttributres = "dks,da", serverId = 1 });

            peopleSettingsService = new PeopleSettingsService(mockCacheProvider.Object, mockOptions.Object, mockPeopleSettingsRepository.Object);

            var settings = peopleSettingsService.GetPeopleSettings(1, baseUrl, connectionStr);

            Assert.NotNull(settings);

        }
    }
}
