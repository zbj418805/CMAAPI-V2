using System.Collections.Generic;
using System.Linq;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Servies;

namespace West.Presence.CMA.Core.Presentations
{
    public interface ISchoolPresentation
    {
        IEnumerable<School> GetSchools(int distrctServerId, string searchKey, int pageIndex, int pageSize);
    }
    public class SchoolsPresentation : PresentationBase, ISchoolPresentation
    {
        private readonly ISchoolService _schoolService;

        public SchoolsPresentation(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        public IEnumerable<School> GetSchools(int distrctServerId, string searchKey, int pageIndex, int pageSize)
        {
            return GetPageItems<School>(_schoolService.GetSchools(distrctServerId, searchKey), pageIndex, pageSize);
        }
    }
}
