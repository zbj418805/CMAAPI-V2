using Moq;
using System.Collections.Generic;
using System.Linq;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Presentations;
using West.Presence.CMA.Core.Servies;
using Xunit;


namespace West.Presence.CMA.Core.Tests.Presentation
{
    public class SchoolPresentationTests
    {
        private Mock<ISchoolService> mockSchoolService;
        private ISchoolPresentation schoolPresentation;

        public SchoolPresentationTests()
        {
            List<School> schools = new List<School>();

            for (int i = 0; i < 10; i++)
            {
                schools.Add(new School()
                {
                    name = $"School Name {i}",
                    description = $"Description {1} ..."
                });
            }

            mockSchoolService = new Mock<ISchoolService>();
            mockSchoolService.Setup(p => p.GetSchools(1, "")).Returns(schools);
            schoolPresentation = new SchoolsPresentation(mockSchoolService.Object);
        }

        [Fact]
        public void Test_Events_First_Page()
        {
            IEnumerable<School> sampleSchools = schoolPresentation.GetSchools(1, "", 0, 2);

            Assert.NotNull(sampleSchools);
            Assert.Equal(2, sampleSchools.Count());
        }

        [Fact]
        public void Test_Events_Second_Page()
        {
            IEnumerable<School> sampleSchools = schoolPresentation.GetSchools(1, "", 1, 3);

            Assert.NotNull(sampleSchools);
            Assert.Equal(3, sampleSchools.Count());
            Assert.Equal("School Name 3", sampleSchools.FirstOrDefault().name);
            Assert.Equal("School Name 4", sampleSchools.Skip(1).Take(1).FirstOrDefault().name);
            Assert.Equal("School Name 5", sampleSchools.LastOrDefault().name);
        }


        [Fact]
        public void Test_Events_Page_with_no_items()
        {
            IEnumerable<School> sampleSchools = schoolPresentation.GetSchools(1, "", 5, 3);
            
            Assert.NotNull(sampleSchools);
            Assert.Empty(sampleSchools);
        }
    }
}
