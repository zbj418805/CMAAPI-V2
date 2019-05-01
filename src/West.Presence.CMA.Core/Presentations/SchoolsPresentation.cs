using System.Collections.Generic;
using System.Linq;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Servies;

namespace West.Presence.CMA.Core.Presentations
{
    public interface ISchoolsPresentation
    {
        IEnumerable<School> GetSchools(string baseUrl, string searchKey, int pageIndex, int pageSize, out int total);
    }
    public class SchoolsPresentation : PresentationBase, ISchoolsPresentation
    {
        private readonly ISchoolsService _schoolService;

        public SchoolsPresentation(ISchoolsService schoolService)
        {
            _schoolService = schoolService;
        }

        public IEnumerable<School> GetSchools(string baseUrl, string searchKey, int pageIndex, int pageSize, out int total)
        {
            var schools = _schoolService.GetSchools(baseUrl, searchKey);
            total = schools == null ? 0 : schools.Count();
            return GetPageItems<School>(schools, pageIndex, pageSize);
        }
    }
}
