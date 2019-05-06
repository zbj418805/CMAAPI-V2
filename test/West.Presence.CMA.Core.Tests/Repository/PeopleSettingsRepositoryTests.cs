using System.Data;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Repositories;
using Moq;
using Xunit;
using West.Presence.CMA.Core.Models;
using System.Collections.Generic;

namespace West.Presence.CMA.Core.Repository.Tests
{
    public class PeopleSettingsRepositoryTests
    {
        private Mock<IDatabaseProvider> mockDatabaseProvider;
        private IPeopleSettingsRepository peopleSettingsRepository;

        public PeopleSettingsRepositoryTests()
        {
            mockDatabaseProvider = new Mock<IDatabaseProvider>();
        }

        [Fact]
        public void Test_PeopleSettingsReposity_Okay()
        {
            string connectionString = "fake_connection_string";
            string sqlscript = "[dbo].[cma_people_settings]";
            PeopleSettingsXML settings = new PeopleSettingsXML();
            settings.SelectGroupsXML = "";
            settings.ExcludedUsersXML = "";
            settings.AttributesXML = "";
            List<PeopleSettingsXML> retSettings = new List<PeopleSettingsXML>();
            retSettings.Add(settings);
            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDatabaseProvider.Setup(p => p.GetData<PeopleSettingsXML>(connectionString, sqlscript, It.IsAny<object>(), CommandType.StoredProcedure)).Returns(retSettings);

            peopleSettingsRepository = new PeopleSettingsRepository(mockDatabaseProvider.Object);

            var retSet = peopleSettingsRepository.GetPeopleSettings(1234, connectionString);

            Assert.NotNull(retSet);

        }

        [Fact]
        public void Test_PeopleSettingsReposity_NoServerId_return_null()
        {
            string connectionString = "fake_connection_string";
            string sqlscript = "[dbo].[cma_people_settings]";
            PeopleSettingsXML settings = new PeopleSettingsXML();
            settings.SelectGroupsXML = "";
            settings.ExcludedUsersXML = "";
            settings.AttributesXML = "";
            List<PeopleSettingsXML> retSettings = new List<PeopleSettingsXML>();
            retSettings.Add(settings);
            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDatabaseProvider.Setup(p => p.GetData<PeopleSettingsXML>(connectionString, sqlscript, It.IsAny<object>(), CommandType.StoredProcedure)).Returns(retSettings);

            peopleSettingsRepository = new PeopleSettingsRepository(mockDatabaseProvider.Object);

            var retSet = peopleSettingsRepository.GetPeopleSettings(0, connectionString);

            Assert.Null(retSet);

        }

        [Fact]
        public void Test_PeopleSettingsReposity_NoConnectionString_return_null()
        {
            string connectionString = "fake_connection_string";
            string sqlscript = "[dbo].[cma_people_settings]";
            PeopleSettingsXML settings = new PeopleSettingsXML();
            settings.SelectGroupsXML = "";
            settings.ExcludedUsersXML = "";
            settings.AttributesXML = "";
            List<PeopleSettingsXML> retSettings = new List<PeopleSettingsXML>();
            retSettings.Add(settings);
            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDatabaseProvider.Setup(p => p.GetData<PeopleSettingsXML>(connectionString, sqlscript, It.IsAny<object>(), CommandType.StoredProcedure)).Returns(retSettings);

            peopleSettingsRepository = new PeopleSettingsRepository(mockDatabaseProvider.Object);

            var retSet = peopleSettingsRepository.GetPeopleSettings(1234, "");

            Assert.Null(retSet);

        }

        //TODO:
        //Add some more test for testing XML content
    }
}
