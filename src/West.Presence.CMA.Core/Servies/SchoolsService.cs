using System;
using System.Collections.Generic;
using System.Text;
using West.Presence.CMA.Core.Models;

namespace West.Presence.CMA.Core.Servies
{
    public interface ISchoolService
    {
        IEnumerable<School> GetSchools(int districtServerId, string searchKey);
    }

    public class SchoolsService : ISchoolService
    {
        public SchoolsService()
        {

        }

        public IEnumerable<School> GetSchools(int districtServerId, string searchKey)
        {
            List<School> schools = new List<School>();
            return schools;
        }
    }
}
