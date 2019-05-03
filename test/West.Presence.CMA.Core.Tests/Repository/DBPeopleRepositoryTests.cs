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

namespace West.Presence.CMA.Core.Tests.Repository
{
    public class DBPeopleRepositoryTests
    {
        private Mock<IDatabaseProvider> mockDatabaseProvider;
        private Mock<IDBConnectionService> mockDBConnectionService;
        private IPeopleRepository _peopleRepository;

        public DBPeopleRepositoryTests()
        {
            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDBConnectionService = new Mock<IDBConnectionService>();
        }

        //[Fact]
        public void Test_RealDB_GetSimplePeople_Test()
        {
            string baseUrl = "http://presence.kingzad.local/";
            string dbString = "Data Source=.;Initial Catalog=Presence_QA;User Id=sa;Password=P@ssw0rd";
            mockDBConnectionService.Setup(p => p.GetConnection(baseUrl)).Returns(dbString);
            var _databaseProvider = new DatabaseProvider();
            _peopleRepository = new DBPeopleRepository(_databaseProvider, mockDBConnectionService.Object);
            var people = _peopleRepository.GetPeople(1291956, baseUrl);
        }

        [Fact]
        public void Test_GetSimplePeople_portletsetting_null_returns_null()
        {
            string baseUrl = "http://localhost/";
            mockDBConnectionService.Setup(p => p.GetConnection(baseUrl)).Returns("");

            List<PortletSettings> settings = new List<PortletSettings>();
            //settings.Add(new PortletSettings() { SelectGroups = "", ExcludedUsers = "" });

            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDatabaseProvider.Setup(p => p.GetData<PortletSettings>("", "[dbo].[staff_directory_get_settings_v2]", new { server_id = 1234 }, CommandType.StoredProcedure)).Returns(settings);
            mockDatabaseProvider.Setup(p => p.GetData<Person>("", "[dbo].[staff_directory_get_basic_users_info_by_groups]", new { group_ids = "1234" }, CommandType.StoredProcedure)).Returns(GetSamplePeople(5));

            _peopleRepository = new DBPeopleRepository(mockDatabaseProvider.Object, mockDBConnectionService.Object);

            var people = _peopleRepository.GetPeople(1234, baseUrl);

            Assert.Null(people);
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
