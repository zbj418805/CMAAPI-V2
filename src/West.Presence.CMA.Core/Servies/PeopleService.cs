using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Repositories;

namespace West.Presence.CMA.Core.Servies
{
    public interface IPeopleService
    {
        IEnumerable<Person> GetPeople(string serverIds, string baseUrl, string searchKey);
    }

    public class PeopleService : IPeopleService
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly IOptions<CMAOptions> _options;
        private readonly IPeopleRepository _peopleRepository;

        public PeopleService(ICacheProvider cacheProvider, IOptions<CMAOptions> options, IPeopleRepository peopleRepository)
        {
            _cacheProvider = cacheProvider;
            _options = options;
            _peopleRepository = peopleRepository;
        }

        public IEnumerable<Person> GetPeople(string serverIds, string baseUrl, string searchKey)
        {
            List<Person> allPeople = new List<Person>();
            int cacheDuration = _options.Value.CachePeopleDurationInSeconds;

            foreach (string serverId in serverIds.Split(','))
            {
                string cacheKey = $"{_options.Value.CachePeopleKey}_{_options.Value.Environment}_{serverId}";
                IEnumerable<Person> people;
                if (!_cacheProvider.TryGetValue<IEnumerable<Person>>(cacheKey, out people))
                {
                    //Get people from repo
                    people = _peopleRepository.GetPeople(int.Parse(serverId), "", "");
                    //set to cache
                    _cacheProvider.Add(cacheKey, people, cacheDuration);
                }
                //Add to collection
                allPeople.AddRange(searchKey == "" ? people : people.Where(p => p.firstName.Contains(searchKey) || p.lastName.Contains(searchKey)));
            }


            //if (searchKey == "")
            //{
            //    foreach (string serverId in serverIds.Split(','))
            //    {
            //        //Set Cachekey
            //        string cacheKey = $"{_options.Value.CachePeopleKey}_{_options.Value.Environment}_{serverId}";
            //        IEnumerable<Person> people;
            //        if (!_cacheProvider.TryGetValue<IEnumerable<Person>>(cacheKey, out people))
            //        {
            //            //Get people from repo
            //            people = _peopleRepository.GetPeople(int.Parse(serverId), baseUrl, "");
            //            //set to cache
            //            _cacheProvider.Add(cacheKey, people, cacheDuration);
            //        }
            //        //Add to collection
            //        allPeople.AddRange(people);
            //    }
            //}
            //else
            //    foreach (string serverId in serverIds.Split(','))
            //        allPeople.AddRange(_peopleRepository.GetPeople(int.Parse(serverId), baseUrl, searchKey));

            return allPeople.OrderBy(x => x.firstName);
        }
    }
}
