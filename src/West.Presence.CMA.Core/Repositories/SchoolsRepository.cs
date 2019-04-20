using System;
using System.Collections.Generic;
using West.Presence.CMA.Core.Models;

namespace West.Presence.CMA.Core.Repositories
{
    public interface ISchoolsRepository
    {
        IEnumerable<School> GetSchools(int districtServerId);
    }

    public class SchoolsRepository : ISchoolsRepository
    {
        public SchoolsRepository()
        {

        }

        public IEnumerable<School> GetSchools(int districtServerId)
        {
            List<School> sch = new List<School>();
            return sch;
            //throw new NotImplementedException();
        }
    }
}