using Moq;
using System.Collections.Generic;
using System.Linq;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Presentations;
using West.Presence.CMA.Core.Servies;
using Xunit;


namespace West.Presence.CMA.Core.Presentation.Tests
{
    public class SchoolsPresentationTests
    {
        private Mock<ISchoolsService> mockSchoolsService;
        private ISchoolsPresentation schoolsPresentation;

        public SchoolsPresentationTests()
        {
            List<School> schools = new List<School>();

            for (int i = 0; i < 10; i++)
            {
                schools.Add(new School()
                {
                    Name = $"School Name {i}",
                    Description = $"Description {1} ..."
                });
            }

            mockSchoolsService = new Mock<ISchoolsService>();
            mockSchoolsService.Setup(p => p.GetSchools("", "")).Returns(schools);
            schoolsPresentation = new SchoolsPresentation(mockSchoolsService.Object);
        }

        [Fact]
        public void Test_Events_First_Page()
        {
            int total;
            IEnumerable<School> sampleSchools = schoolsPresentation.GetSchools("", "", 0, 2, out total);

            Assert.NotNull(sampleSchools);
            Assert.Equal(2, sampleSchools.Count());
        }

        [Fact]
        public void Test_Events_Second_Page()
        {
            int total;
            IEnumerable<School> sampleSchools = schoolsPresentation.GetSchools("", "", 1, 3, out total);

            Assert.NotNull(sampleSchools);
            Assert.Equal(3, sampleSchools.Count());
            Assert.Equal("School Name 3", sampleSchools.FirstOrDefault().Name);
            Assert.Equal("School Name 4", sampleSchools.Skip(1).Take(1).FirstOrDefault().Name);
            Assert.Equal("School Name 5", sampleSchools.LastOrDefault().Name);
        }


        [Fact]
        public void Test_Events_Page_with_no_items()
        {
            int total;
            IEnumerable<School> sampleSchools = schoolsPresentation.GetSchools("", "", 5, 3, out total);
            
            Assert.NotNull(sampleSchools);
            Assert.Empty(sampleSchools);
        }
    }
}
