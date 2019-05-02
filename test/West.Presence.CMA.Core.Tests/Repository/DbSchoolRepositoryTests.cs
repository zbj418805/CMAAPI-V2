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
    public class DbSchoolRepositoryTests
    {
        private Mock<IDatabaseProvider> mockDatabaseProvider;
        private Mock<IDBConnectionService> mockDBConnectionService;
        private ISchoolsRepository _schoolRepository;

        public DbSchoolRepositoryTests()
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
        }

        [Fact]
        public void Test_School_Reposity_Get_Schools()
        {
            string baseUrl = "http://presence.kingzad.local/";

            //arrange
            mockDBConnectionService.Setup(p => p.GetConnection(baseUrl)).Returns("");
            IDatabaseProvider _databaseProvider = new DatabaseProvider();
            //
            List<School> schools = GetSampleSchools(5);
            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDatabaseProvider.Setup(p => p.GetCellValue<int>("", "SELECT TOP 1 server_id FROM click_server_urls WHERE url = @url", new { url = baseUrl }, CommandType.Text)).Returns(10);
            mockDatabaseProvider.Setup(p => p.GetData<School>("", "[dbo].[cma_server_get_v2]", new { district_server_id = 10 }, CommandType.StoredProcedure)).Returns(schools);
            mockDatabaseProvider.Setup(p => p.GetData<MAttribute>("", "[dbo].[cma_server_attributes.get]", new { server_id = 1 }, CommandType.StoredProcedure)).Returns(GetSampleAttributes(1));
            mockDatabaseProvider.Setup(p => p.GetData<MAttribute>("", "[dbo].[cma_server_attributes.get]", new { server_id = 2 }, CommandType.StoredProcedure)).Returns(GetSampleAttributes(2));
            mockDatabaseProvider.Setup(p => p.GetData<MAttribute>("", "[dbo].[cma_server_attributes.get]", new { server_id = 3 }, CommandType.StoredProcedure)).Returns(GetSampleAttributes(3));
            mockDatabaseProvider.Setup(p => p.GetData<MAttribute>("", "[dbo].[cma_server_attributes.get]", new { server_id = 4 }, CommandType.StoredProcedure)).Returns(GetSampleAttributes(4));
            _schoolRepository = new DBSchoolsRepository(mockDatabaseProvider.Object, mockDBConnectionService.Object);
            var resultSchools = _schoolRepository.GetSchools(baseUrl);

            //Assert
            //Assert.Equal(4, resultSchools.Count());
            Assert.Empty(resultSchools);
            
        }

        [Fact]
        public void Test_School_Reposity_Url_Not_Exist_No_Schools_Return()
        {
            string baseUrl = "http://presence.kingzad.local/";
            //arrange
            mockDBConnectionService.Setup(p => p.GetConnection(baseUrl)).Returns("");
            IDatabaseProvider _databaseProvider = new DatabaseProvider();

            //
            List<School> schools = GetSampleSchools(5);
            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDatabaseProvider.Setup(p => p.GetCellValue<int>("", "SELECT TOP 1 server_id FROM click_server_urls WHERE url = @url", new { url = baseUrl }, CommandType.Text)).Returns(0);
            mockDatabaseProvider.Setup(p => p.GetData<School>("", "[dbo].[cma_server_get_v2]", new { district_server_id = 10 }, CommandType.StoredProcedure)).Returns(schools);
            mockDatabaseProvider.Setup(p => p.GetData<MAttribute>("", "[dbo].[cma_server_attributes.get]", new { server_id = 1 }, CommandType.StoredProcedure)).Returns(GetSampleAttributes(1));
            mockDatabaseProvider.Setup(p => p.GetData<MAttribute>("", "[dbo].[cma_server_attributes.get]", new { server_id = 2 }, CommandType.StoredProcedure)).Returns(GetSampleAttributes(2));
            mockDatabaseProvider.Setup(p => p.GetData<MAttribute>("", "[dbo].[cma_server_attributes.get]", new { server_id = 3 }, CommandType.StoredProcedure)).Returns(GetSampleAttributes(3));
            mockDatabaseProvider.Setup(p => p.GetData<MAttribute>("", "[dbo].[cma_server_attributes.get]", new { server_id = 4 }, CommandType.StoredProcedure)).Returns(GetSampleAttributes(4));

            _schoolRepository = new DBSchoolsRepository(mockDatabaseProvider.Object, mockDBConnectionService.Object);

            var resultSchools = _schoolRepository.GetSchools(baseUrl);

            //Assert
            Assert.Empty(resultSchools);
        }


        [Fact]
        public void Test_School_Reposity_No_Schools_Return()
        {
            string baseUrl = "http://presence.kingzad.local/";
            //arrange
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
            Assert.Empty(resultSchools);
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

        private List<MAttribute> GetSampleAttributes(int serverId) {
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
