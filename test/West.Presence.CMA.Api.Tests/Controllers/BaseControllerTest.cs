using System;
using System.Collections.Generic;
using System.Text;
using West.Presence.CMA.Core.Models;

namespace West.Presence.CMA.Api.Tests.Controllers
{
    public class BaseControllerTest
    {
        protected List<School> GetSampleSchools(int count, int distructServerid)
        {
            List<School> school = new List<School>();
            for(int i = 0; i< count; i++)
            {
                school.Add(new School() {
                    Name = $"School Name {i}",
                    Description = $"Description {1} ...",
                    DistrictServerId = 10,
                    ServerId = distructServerid
                });
            }

            return school;
        }

        protected List<Person> GetSampleSimplePerson(int count)
        {
            List<Person> people = new List<Person>();
            for (int i = 0; i < count; i++)
            {
                people.Add(new Person()
                {
                    userId = i,
                    firstName = $"First_{i}",
                    lastName = $"Last_{i}",
                    serverId = 1
                });
            }

            return people;
        }

        protected List<PersonInfo> GetSamplePersonInfo(int count)
        {
            List<PersonInfo> peopleinfo = new List<PersonInfo>();
            for (int i = 0; i < count; i++)
            {
                peopleinfo.Add(new PersonInfo()
                {
                    userId = i,
                    firstName = $"First_{i}",
                    lastName = $"Last_{i}"
                });
            }

            return peopleinfo;
        }

        protected List<News> GetSampleNews(int count)
        {
            List<News> news = new List<News>();
            for (int i = 0; i < count; i++)
            {
                news.Add(new News()
                {
                    Title = $"Title {i}",
                    Body = $"Body {i} ..."
                });
            }

            return news;
        }

        protected List<Event> GetSampleEvents(int count)
        {
            List<Event> events = new List<Event>();
            for (int i = 0; i < count; i++)
            {
                events.Add(new Event()
                {
                    Name = $"Events 1-{i}",
                    StartTime = DateTime.UtcNow.AddDays(i),
                    EndTime = DateTime.UtcNow.AddDays(i)
                });
            }

            return events;
        }

        protected List<Channel2Group> GetSamepleC2Gs(int count)
        {
            List<Channel2Group> c2gs = new List<Channel2Group>();

            for (int i = 0; i < count; i++)
            {
                c2gs.Add(new Channel2Group()
                {
                    channelId = i + 10,
                    groupId = i
                });
            }

            return c2gs;
        }
    }
}
