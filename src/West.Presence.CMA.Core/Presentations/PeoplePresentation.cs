using System.Collections.Generic;
using System.Linq;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Servies;

namespace West.Presence.CMA.Core.Presentations
{
    public interface IPeoplePresentation
    {
        IEnumerable<Person> GetPeople(List<int> serverIds, string searchKey,string baseUrl, int pageIndex, int pageSize, out int total);
    }

    public class PeoplePresentation: PresentationBase, IPeoplePresentation
    {
        private readonly IPeopleService _peopleService; 

        public PeoplePresentation(IPeopleService peopleService)
        {
            _peopleService = peopleService;
        }

        public IEnumerable<Person> GetPeople(List<int> serverIds, string searchKey, string baseUrl, int pageIndex, int pageSize, out int total)
        {
            var people = _peopleService.GetPeople(serverIds, searchKey, baseUrl);
            total = people.Count();

            return GetPageItems<Person>(people, pageIndex, pageSize);
        }
    }
}
