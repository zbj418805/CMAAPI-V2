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
        IEnumerable<Person> GetPeople(string serverIds, string searchKey);
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

        public IEnumerable<Person> GetPeople(string serverIds, string searchKey)
        {
            List<Person> allPeople = new List<Person>();
            int cacheDuration = _options.Value.CachePeopleDurationInSeconds;
            if (searchKey == "")
            {
                foreach (string serverId in serverIds.Split(','))
                {
                    string cacheKey = $"{_options.Value.CachePeopleKey}_{serverId}";
                    IEnumerable<Person> people;
                    if (_cacheProvider.TryGetValue<IEnumerable<Person>>(cacheKey, out people))
                    {
                        allPeople.AddRange(people);
                    }
                    else
                    {
                        people = _peopleRepository.GetPeople(int.Parse(serverId), "");
                        allPeople.AddRange(people);
                        _cacheProvider.Add(cacheKey, people, cacheDuration);
                    }
                }
            }
            else
            {
                foreach (string serverId in serverIds.Split(','))
                {
                    allPeople.AddRange(_peopleRepository.GetPeople(int.Parse(serverId), searchKey));
                }
            }

            return allPeople.OrderBy(x => x.firstName);
        }
    }
}
