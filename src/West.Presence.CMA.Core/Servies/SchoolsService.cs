using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Repositories;


namespace West.Presence.CMA.Core.Servies
{
    public interface ISchoolsService
    {
        IEnumerable<School> GetSchools(int districtServerId, string searchKey);
    }

    public class SchoolsService : ISchoolsService
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly IOptions<CMAOptions> _options;
        private readonly ISchoolsRepository _schoolsRepository;

        public SchoolsService(ICacheProvider cacheProvider, IOptions<CMAOptions> options, ISchoolsRepository schoolsRepository)
        {
            _cacheProvider = cacheProvider;
            _options = options;
            _schoolsRepository = schoolsRepository;
        }

        public IEnumerable<School> GetSchools(int districtServerId, string searchKey)
        {
            int cacheDuration = _options.Value.CacheSchoolsDurationInSeconds;
            string cacheKey = $"{_options.Value.CacheSchoolsKey}_{_options.Value.Environment}_{districtServerId}";

            IEnumerable<School> schools;

            if (!_cacheProvider.TryGetValue<IEnumerable<School>>(cacheKey, out schools))
            {
                schools = _schoolsRepository.GetSchools(districtServerId);
                _cacheProvider.Add(cacheKey, schools, cacheDuration);
            }

            if (searchKey == "")
                return schools;
            else
                return schools.Where(x => x.name.Contains(searchKey));
        }
    }
}
