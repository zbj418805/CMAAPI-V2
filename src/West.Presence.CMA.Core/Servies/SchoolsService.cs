﻿using System;
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
        IEnumerable<School> GetSchools(string baseUrl, string searchKey);
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

        public IEnumerable<School> GetSchools(string baseUrl, string searchKey)
        {
            Uri u = new Uri(baseUrl);

            int cacheDuration = _options.Value.CacheSchoolsDurationInSeconds;
            string cacheKey = $"{_options.Value.CacheSchoolsKey}_{u.Host}";

            IEnumerable<School> schools;

            if (!_cacheProvider.TryGetValue<IEnumerable<School>>(cacheKey, out schools))
            {
                //Get Schools from repo
                schools = _schoolsRepository.GetSchools(baseUrl);
                //Set to Cache
                _cacheProvider.Add(cacheKey, schools, cacheDuration);
            }

            return searchKey == "" ? schools : schools.Where(x => x.Name.Contains(searchKey) || x.Description.Contains(searchKey));
        }
    }
}

