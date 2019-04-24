using System.Collections.Generic;
using System.Linq;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Servies;
using Xunit;
using Moq;
using West.Presence.CMA.Core.Repositories;
using West.Presence.CMA.Core.Helper;
using Microsoft.Extensions.Options;

namespace West.Presence.CMA.Core.Tests.Services
{
    public class SchoolsServiceTests
    {
        private Mock<ISchoolsRepository> mockSchoolsRepository;
        private Mock<ICacheProvider> mockCacheProvider;
        private Mock<IOptions<CMAOptions>> mockOptions;
        private ISchoolsService schoolsService;

        public SchoolsServiceTests()
        {
            CMAOptions option = new CMAOptions
            {
                Environment = "Dev",
                CacheSchoolsKey = "CMASchoolKey",
                CacheSchoolsDurationInSeconds = 300
            };

            mockOptions = new Mock<IOptions<CMAOptions>>();
            mockOptions.Setup(p => p.Value).Returns(option);
        }

        [Fact]
        public void Test_Schools_From_CacheProvider_With_No_Search()
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

            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<School>>("CMASchoolKey_Dev_1", out schools)).Returns(true);

            mockSchoolsRepository = new Mock<ISchoolsRepository>();
            //mockSchoolsRepository.Setup(p => p.GetSchools(1)).Returns(new List<School>());

            schoolsService = new SchoolsService(mockCacheProvider.Object, mockOptions.Object, mockSchoolsRepository.Object);
            var resultSchools = schoolsService.GetSchools(1, "", "");

            Assert.NotNull(resultSchools);
            Assert.Equal(10, resultSchools.Count());
        }

        [Fact]
        public void Test_Schools_From_Repository_With_No_Search()
        {
            IEnumerable<School> sch;

            mockCacheProvider = new Mock<ICacheProvider>();
            //mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<School>>("CMASchoolKey_Dev_1", out sch)).Returns(false);

            List<School> schools = new List<School>();

            for (int i = 0; i < 10; i++)
            {
                schools.Add(new School()
                {
                    serverName = $"School Name {i}",
                    serverDescription = $"Description {i} ..."
                });
            }

            mockSchoolsRepository = new Mock<ISchoolsRepository>();
            mockSchoolsRepository.Setup(p => p.GetSchools(1, "")).Returns(schools);

            schoolsService = new SchoolsService(mockCacheProvider.Object, mockOptions.Object, mockSchoolsRepository.Object);
            var resultSchools = schoolsService.GetSchools(1, "", "");

            Assert.NotNull(resultSchools);
            Assert.Equal(10, resultSchools.Count());
        }

        [Fact]
        public void Test_Schools_From_CacheProvider_With_Search()
        {
            List<School> lsSchools = new List<School>();

            for (int i = 0; i < 10; i++)
            {
                lsSchools.Add(new School()
                {
                    serverName = $"School Name {i}",
                    serverDescription = $"Description {i} ..."
                });
            }

            var schools = lsSchools.AsEnumerable();

            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<School>>("CMASchoolKey_Dev_1", out schools)).Returns(true);

            mockSchoolsRepository = new Mock<ISchoolsRepository>();
            mockSchoolsRepository.Setup(p => p.GetSchools(1, "")).Returns(new List<School>());

            schoolsService = new SchoolsService(mockCacheProvider.Object, mockOptions.Object, mockSchoolsRepository.Object);
            var resultSchools = schoolsService.GetSchools(1, "", "1");

            Assert.NotNull(resultSchools);
            Assert.Single(resultSchools);
        }

        [Fact]
        public void Test_Schools_From_Repository_With_Search()
        {
            //IEnumerable<School> sch;

            mockCacheProvider = new Mock<ICacheProvider>();
            //mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<School>>("CMASchoolKey_Dev_1", out sch)).Returns(false);

            List<School> schools = new List<School>();

            for (int i = 0; i < 10; i++)
            {
                schools.Add(new School()
                {
                    serverName = $"School Name {i}",
                    serverDescription = $"Description {i} ..."
                });
            }

            mockSchoolsRepository = new Mock<ISchoolsRepository>();
            mockSchoolsRepository.Setup(p => p.GetSchools(1, "")).Returns(schools);

            schoolsService = new SchoolsService(mockCacheProvider.Object, mockOptions.Object, mockSchoolsRepository.Object);
            var resultSchools = schoolsService.GetSchools(1, "", "1");

            Assert.NotNull(resultSchools);
            Assert.Single(resultSchools);
        }
    }
}