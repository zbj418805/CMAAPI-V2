using System;
using System.Collections.Generic;
using System.Text;
using West.Presence.CMA.Core.Models;

namespace West.Presence.CMA.Core.Servies
{
    public interface IPeopleService
    {
        IEnumerable<Person> GetPeople(string serverIds, string searchKey);
    }

    public class PeopleService : IPeopleService
    {
        public PeopleService()
        {

        }

        public IEnumerable<Person> GetPeople(string serverIds, string searchKey)
        {
            List<Person> people = new List<Person>();

            return people;
        }
    }
}
