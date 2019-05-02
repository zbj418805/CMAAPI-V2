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

        public void Test_GetSimplePeople_Okay()
        {
            string baseUrl = "http://presence.kingzad.local/";
            mockDBConnectionService.Setup(p => p.GetConnection(baseUrl)).Returns("");
            IDatabaseProvider _databaseProvider = new DatabaseProvider();

            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDatabaseProvider.Setup(p => p.GetData<School>("", "[dbo].[cma_server_get_v2]", new { district_server_id = 10 }, CommandType.StoredProcedure)).Returns(schools)
        }
    }
}
