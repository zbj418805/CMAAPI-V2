using System;
using System.Collections.Generic;
using West.Presence.CMA.Core.Models;


namespace West.Presence.CMA.Core.Repositories
{
    public interface INewsRepository
    {
        IEnumerable<News> GetNews(int serverId, string baseUrl);
    }

    public class DBNewsRepository : INewsRepository
    {
        public DBNewsRepository()
        {

        }

        public IEnumerable<News> GetNews(int serverId, string baseUrl)
        {
            throw new NotImplementedException();
        }
    }

    public class APINewsRepository : INewsRepository
    {
        public APINewsRepository()
        {

        }

        public IEnumerable<News> GetNews(int serverId, string baseUrl)
        {
            throw new NotImplementedException();
        }
    }
}
