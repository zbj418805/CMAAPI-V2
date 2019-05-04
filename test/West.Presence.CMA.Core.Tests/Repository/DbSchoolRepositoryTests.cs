using Moq;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Repositories;
using West.Presence.CMA.Core.Servies;
using Xunit;

namespace West.Presence.CMA.Core.Repository.Tests
{
    public class DBSchoolRepositoryTests
    {
        private Mock<IDatabaseProvider> mockDatabaseProvider;
        private Mock<IDBConnectionService> mockDBConnectionService;
        private ISchoolsRepository _schoolRepository;

        public DBSchoolRepositoryTests()
        {
            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDBConnectionService = new Mock<IDBConnectionService>();
        }

        //[Fact]
        public void Test_RealDB_Test()
        {
            //real DB Test
            string baseUrl = "http://presence.kingzad.local/";
            string dbString = "Data Source=.;Initial Catalog=Presence_QA;User Id=sa;Password=P@ssw0rd";
            mockDBConnectionService.Setup(p => p.GetConnection(baseUrl)).Returns(dbString);
            var _databaseProvider = new DatabaseProvider();
            _schoolRepository = new DBSchoolsRepository(_databaseProvider, mockDBConnectionService.Object);
            var schools = _schoolRepository.GetSchools(baseUrl);
            Assert.NotEmpty(schools);
        }

        [Fact]
        public void Test_School_Reposity_Get_Schools()
        {
            string baseUrl = "http://presence.kingzad.local/";
            string connectionString = "fake_connection_string";
            //arrange
            mockDBConnectionService.Setup(p => p.GetConnection(baseUrl)).Returns(connectionString);
            IDatabaseProvider _databaseProvider = new DatabaseProvider();
            //
            List<School> schools = GetSampleSchools(5);
            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDatabaseProvider.Setup(p => p.GetCellValue<int>(connectionString, "SELECT TOP 1 server_id FROM click_server_urls WHERE url = @url", It.IsAny<object>(), CommandType.Text)).Returns(10);
            mockDatabaseProvider.Setup(p => p.GetData<School>(connectionString, "[dbo].[cma_server_get_v2]", It.IsAny<object>(), CommandType.StoredProcedure)).Returns(schools);
            mockDatabaseProvider.Setup(p => p.GetData<MAttribute>(connectionString, "[dbo].[cma_server_attributes.get]", It.IsAny<object>(), CommandType.StoredProcedure)).Returns(GetSampleAttributes());
            _schoolRepository = new DBSchoolsRepository(mockDatabaseProvider.Object, mockDBConnectionService.Object);
            var resultSchools = _schoolRepository.GetSchools(baseUrl);

            //Assert
            Assert.Equal(4, resultSchools.Count());
            
        }

        [Fact]
        public void Test_School_Reposity_can_not_find_serverId_Return_null()
        {
            string baseUrl = "http://presence.kingzad.local/";
            string connectionString = "fake_connection_string";
            //arrange
            mockDBConnectionService.Setup(p => p.GetConnection(baseUrl)).Returns(connectionString);
            IDatabaseProvider _databaseProvider = new DatabaseProvider();

            
            List<School> schools = GetSampleSchools(0);
            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDatabaseProvider.Setup(p => p.GetCellValue<int>(connectionString, "SELECT TOP 1 server_id FROM click_server_urls WHERE url = @url", It.IsAny<object>(), CommandType.Text)).Returns(0);
            mockDatabaseProvider.Setup(p => p.GetData<School>(connectionString, "[dbo].[cma_server_get_v2]", It.IsAny<object>(), CommandType.StoredProcedure)).Returns(schools);
            mockDatabaseProvider.Setup(p => p.GetData<MAttribute>(connectionString, "[dbo].[cma_server_attributes.get]", It.IsAny<object>(), CommandType.StoredProcedure)).Returns(GetSampleAttributes());

            _schoolRepository = new DBSchoolsRepository(mockDatabaseProvider.Object, mockDBConnectionService.Object);

            var resultSchools = _schoolRepository.GetSchools(baseUrl);

            //Assert
            Assert.Null(resultSchools);
        }

        [Fact]
        public void Test_School_Reposity_can_not_find_schools_Return_empty()
        {
            string baseUrl = "http://presence.kingzad.local/";
            string connectionString = "fake_connection_string";
            //arrange
            mockDBConnectionService.Setup(p => p.GetConnection(baseUrl)).Returns(connectionString);
            IDatabaseProvider _databaseProvider = new DatabaseProvider();


            List<School> schools = GetSampleSchools(0);
            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDatabaseProvider.Setup(p => p.GetCellValue<int>(connectionString, "SELECT TOP 1 server_id FROM click_server_urls WHERE url = @url", It.IsAny<object>(), CommandType.Text)).Returns(10);
            mockDatabaseProvider.Setup(p => p.GetData<School>(connectionString, "[dbo].[cma_server_get_v2]", It.IsAny<object>(), CommandType.StoredProcedure)).Returns(schools);
            mockDatabaseProvider.Setup(p => p.GetData<MAttribute>(connectionString, "[dbo].[cma_server_attributes.get]", It.IsAny<object>(), CommandType.StoredProcedure)).Returns(GetSampleAttributes());

            _schoolRepository = new DBSchoolsRepository(mockDatabaseProvider.Object, mockDBConnectionService.Object);

            var resultSchools = _schoolRepository.GetSchools(baseUrl);

            //Assert
            Assert.Empty(resultSchools);
        }


        [Fact]
        public void Test_School_Reposity_Url_Not_Exist_No_Schools_Return()
        {
            //arrange
            string baseUrl = "http://presence.kingzad.local/";
            //string connectionString = "fake_connection_string";
            
            mockDBConnectionService.Setup(p => p.GetConnection(baseUrl)).Returns("");
            IDatabaseProvider _databaseProvider = new DatabaseProvider();

            //
            List<School> schools = GetSampleSchools(0);
            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDatabaseProvider.Setup(p => p.GetCellValue<int>("", "SELECT TOP 1 server_id FROM click_server_urls WHERE url = @url", new { url = baseUrl }, CommandType.Text)).Returns(10);
            mockDatabaseProvider.Setup(p => p.GetData<School>("", "[dbo].[cma_server_get_v2]", new { district_server_id = 10 }, CommandType.StoredProcedure)).Returns(schools);
            
            _schoolRepository = new DBSchoolsRepository(mockDatabaseProvider.Object, mockDBConnectionService.Object);

            var resultSchools = _schoolRepository.GetSchools(baseUrl);

            //Assert
            Assert.Null(resultSchools);
        }

        private List<School> GetSampleSchools(int count)
        {
            List<School> schools = new List<School>();
            for (int i = 1; i < count; i++)
            {
                schools.Add(new School()
                {
                    ServerId = i,
                    DistrictServerId = 0,
                    Name = $"School {i}",
                    Description = $"School Details {i}"
                });
            }

            return schools;
        }

        private List<MAttribute> GetSampleAttributes() {
            List<MAttribute> mAttributes = new List<MAttribute>();
            mAttributes.Add(new MAttribute() { attributeName = "org_address1", attributeValue = "123 akdasdf " });
            mAttributes.Add(new MAttribute() { attributeName = "org_address2", attributeValue = "123 akdasdf " });
            mAttributes.Add(new MAttribute() { attributeName = "org_city", attributeValue = "123 akdasdf " });
            mAttributes.Add(new MAttribute() { attributeName = "org_country", attributeValue = "123 akdasdf " });
            mAttributes.Add(new MAttribute() { attributeName = "org_postal", attributeValue = "123 akdasdf " });
            mAttributes.Add(new MAttribute() { attributeName = "org_province", attributeValue = "123 akdasdf " });
            mAttributes.Add(new MAttribute() { attributeName = "org_phone", attributeValue = "123 akdasdf " });
            mAttributes.Add(new MAttribute() { attributeName = "org_slogan", attributeValue = "123 akdasdf " });
            mAttributes.Add(new MAttribute() { attributeName = "org_fax", attributeValue = "123 akdasdf " });
            mAttributes.Add(new MAttribute() { attributeName = "org_facebook_website", attributeValue = "123 akdasdf " });
            mAttributes.Add(new MAttribute() { attributeName = "org_twitter_website", attributeValue = "123 akdasdf " });
            mAttributes.Add(new MAttribute() { attributeName = "org_youtube_channel", attributeValue = "123 akdasdf " });
            mAttributes.Add(new MAttribute() { attributeName = "org_email_address", attributeValue = "123 akdasdf " });

            return mAttributes;
        }
    }
}
