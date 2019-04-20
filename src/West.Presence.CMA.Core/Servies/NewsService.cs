using System;
using System.Collections.Generic;
using System.Text;
using West.Presence.CMA.Core.Models;

namespace West.Presence.CMA.Core.Servies
{
    public interface INewsService
    {
        IEnumerable<News> GetNews(string serverIds, string searchKey);
    }

    public class NewsService : INewsService
    {
        public NewsService()
        {

        }

        public IEnumerable<News> GetNews(string serverIds, string searchKey)
        {
            List<News> news = new List<News>();

            return news;
        }
    }
}
