using System.Collections.Generic;
using System.Linq;
using Moq;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Repositories;
using Xunit;

namespace West.Presence.CMA.Core.Tests.Repository
{
    public class SchoolRepositoryTests
    {
        private Mock<IHttpClientProvider> mockHttpClientProvider;


        public SchoolRepositoryTests()
        {

        }

        [Fact]
        public void Test_School_Reposity_Get_Schools() {
            List<School> lsSchools = new List<School>();
            for (int i = 0; i < 10; i++)
            {
                lsSchools.Add(new School()
                {
                    serverName = $"School Name {i}",
                    serverDescription = $"Description {1} ..."
                });
            }
            var schools = lsSchools.AsEnumerable();


            mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.Setup(p => p.GetData<School>("http://test.url/" + "/presence/Api/CMA/Schools/" + "1234")).Returns(schools);

            APISchoolsRepository schoolRepo = new APISchoolsRepository(mockHttpClientProvider.Object);

            var resultSchools = schoolRepo.GetSchools(1234, "http://test.url/");

            Assert.NotNull(resultSchools);

            Assert.Equal(10, resultSchools.Count());
        }

        [Fact]
        public void Test_School_Reposity_Get_No_Schools()
        {
            List<School> lsSchools = new List<School>();
            for (int i = 0; i < 10; i++)
            {
                lsSchools.Add(new School()
                {
                    serverName = $"School Name {i}",
                    serverDescription = $"Description {1} ..."
                });
            }
            var schools = lsSchools.AsEnumerable();


            mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.Setup(p => p.GetData<School>("http://test.url/" + "/presence/Api/CMA/Schools/" + "21234")).Returns(schools);

            APISchoolsRepository schoolRepo = new APISchoolsRepository(mockHttpClientProvider.Object);

            var resultSchools = schoolRepo.GetSchools(1234, "http://test.url/");

            Assert.NotNull(resultSchools);

            Assert.Empty(resultSchools);
        }
    }
}
