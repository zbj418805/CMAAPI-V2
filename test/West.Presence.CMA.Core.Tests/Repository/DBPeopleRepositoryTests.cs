using Moq;
using System.Collections.Generic;
using System.Linq;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Servies;
using Moq;
using Xunit;
using West.Presence.CMA.Core.Repositories;
using System.Data;
using West.Presence.CMA.Core.Models;
using System.Threading.Tasks;

namespace West.Presence.CMA.Core.Repository.Tests
{
    public class DBPeopleRepositoryTests
    {
        private Mock<IDatabaseProvider> mockDatabaseProvider;
        private Mock<IDBConnectionService> mockDBConnectionService;
        private Mock<IPeopleSettingsService> mockPeopleSettingServce;
        private IPeopleRepository _peopleRepository;

        public DBPeopleRepositoryTests()
        {
            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDBConnectionService = new Mock<IDBConnectionService>();
            mockPeopleSettingServce = new Mock<IPeopleSettingsService>();
        }

        //[Fact]
        public void Test_RealDB_GetSimplePeople_Test()
        {
            string baseUrl = "http://presence.kingzad.local/";
            string dbString = "Data Source=.;Initial Catalog=Presence_QA;User Id=sa;Password=P@ssw0rd";
            mockDBConnectionService.Setup(p => p.GetConnection(baseUrl)).Returns(dbString);
            var _databaseProvider = new DatabaseProvider();
            mockPeopleSettingServce.Setup(p => p.GetPeopleSettings(1291956, baseUrl, dbString)).Returns(new PeopleSettings());


            _peopleRepository = new DBPeopleRepository(_databaseProvider, mockDBConnectionService.Object, mockPeopleSettingServce.Object);
            var people = _peopleRepository.GetPeople(1291956, baseUrl);

            Assert.NotEmpty(people);
        }

        [Fact]
        public void Test_GetSimplePeople_Okay()
        {
            string baseUrl = "http://localhost/";
            string connectionString = "fake_connection_string";
            mockDBConnectionService.Setup(p => p.GetConnection(baseUrl)).Returns(connectionString);

            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDatabaseProvider.Setup(p => p.GetData<Person>(connectionString, "[dbo].[staff_directory_get_basic_users_info_by_groups]", It.IsAny<object>(), CommandType.StoredProcedure)).Returns(GetSamplePeople(5));

            
            mockPeopleSettingServce = new Mock<IPeopleSettingsService>();
            mockPeopleSettingServce.Setup(p => p.GetPeopleSettings(1234, baseUrl, connectionString)).Returns(new PeopleSettings() {
                SelectGroups= "1291957"
            });

            //var _databaseProvider = new DatabaseProvider();
            _peopleRepository = new DBPeopleRepository(mockDatabaseProvider.Object, mockDBConnectionService.Object, mockPeopleSettingServce.Object);

            var people = _peopleRepository.GetPeople(1234, baseUrl);

            Assert.Equal(5, people.Count());
        }

        [Fact]
        public void Test_GetSimplePeople_connection_string_empty_returns_nulll()
        {
            string baseUrl = "http://localhost/";
            string connectionString = "fake_connection_string";
            mockDBConnectionService.Setup(p => p.GetConnection(baseUrl)).Returns("");

            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDatabaseProvider.Setup(p => p.GetData<Person>("", "[dbo].[staff_directory_get_basic_users_info_by_groups]", It.IsAny<object>(), CommandType.StoredProcedure)).Returns(GetSamplePeople(5));

            mockPeopleSettingServce = new Mock<IPeopleSettingsService>();
            mockPeopleSettingServce.Setup(p => p.GetPeopleSettings(1291956, baseUrl, connectionString)).Returns(new PeopleSettings());

            _peopleRepository = new DBPeopleRepository(mockDatabaseProvider.Object, mockDBConnectionService.Object, mockPeopleSettingServce.Object);

            var people = _peopleRepository.GetPeople(1234, baseUrl);

            Assert.Null(people);
        }

        //[Fact]
        public void Test_GetSimplePeople_PortletSettings_null_returns_nulll()
        {
            string baseUrl = "http://localhost/";
            string connectionString = "fake_connection_string";
            mockDBConnectionService.Setup(p => p.GetConnection(baseUrl)).Returns(connectionString);

            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDatabaseProvider.Setup(p => p.GetData<Person>(connectionString, "[dbo].[staff_directory_get_basic_users_info_by_groups]", It.IsAny<object>(), CommandType.StoredProcedure)).Returns(GetSamplePeople(5));

            mockPeopleSettingServce = new Mock<IPeopleSettingsService>();
            mockPeopleSettingServce.Setup(p => p.GetPeopleSettings(1291956, baseUrl, connectionString)).Returns((PeopleSettings)null);


            _peopleRepository = new DBPeopleRepository(mockDatabaseProvider.Object, mockDBConnectionService.Object, mockPeopleSettingServce.Object);

            var people = _peopleRepository.GetPeople(1234, baseUrl);

            Assert.Null(people);
        }

        [Fact]
        public void Test_GetSimplePeople_selectGroups_empty_returns_empty()
        {
            string baseUrl = "http://localhost/";
            string connectionString = "fake_connection_string";
            mockDBConnectionService.Setup(p => p.GetConnection(baseUrl)).Returns(connectionString);

            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDatabaseProvider.Setup(p => p.GetData<Person>(connectionString, "[dbo].[staff_directory_get_basic_users_info_by_groups]", It.IsAny<object>(), CommandType.StoredProcedure)).Returns(GetSamplePeople(5));

            mockPeopleSettingServce = new Mock<IPeopleSettingsService>();
            mockPeopleSettingServce.Setup(p => p.GetPeopleSettings(1234, baseUrl, connectionString)).Returns( new PeopleSettings() { SelectGroups="", ExcludedUser="" });


            _peopleRepository = new DBPeopleRepository(mockDatabaseProvider.Object, mockDBConnectionService.Object, mockPeopleSettingServce.Object);

            var people = _peopleRepository.GetPeople(1234, baseUrl);

            Assert.Empty(people);
        }

        private List<Person> GetSamplePeople(int count)
        {
            List<Person> people = new List<Person>();
            for (int i = 0; i < count; i++)
            {
                people.Add(new Person()
                {
                    userId = i,
                    firstName = "First",
                    lastName = "Last",
                    jobTitle = "jobs"
                });
            }
            return people;
        }
    }
}
