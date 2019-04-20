using System.Collections.Generic;
using System.Linq;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Servies;

namespace West.Presence.CMA.Core.Presentations
{
    public interface IPeoplePresentation
    {
        IEnumerable<Person> GetPeople(string serverIds, string searchKey,string baseUrl, int pageIndex, int pageSize);
    }

    public class PeoplePresentation: PresentationBase, IPeoplePresentation
    {
        private readonly IPeopleService _peopleService; 

        public PeoplePresentation(IPeopleService peopleService)
        {
            _peopleService = peopleService;
        }

        public IEnumerable<Person> GetPeople(string serverIds, string searchKey, string baseUrl, int pageIndex, int pageSize)
        {
            return GetPageItems<Person>(_peopleService.GetPeople(serverIds, searchKey, baseUrl), pageIndex, pageSize);
        }
    }
}
