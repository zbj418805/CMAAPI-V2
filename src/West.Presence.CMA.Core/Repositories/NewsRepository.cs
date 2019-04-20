using System;
using System.Collections.Generic;
using West.Presence.CMA.Core.Models;


namespace West.Presence.CMA.Core.Repositories
{
    public interface INewsRepository
    {
        IEnumerable<News> GetNews(int serverId, string searchKey);
    }

    public class NewsRepository : INewsRepository
    {
        public NewsRepository()
        {

        }

        public IEnumerable<News> GetNews(int serverId, string searchKey)
        {
            throw new NotImplementedException();
        }
    }
}
