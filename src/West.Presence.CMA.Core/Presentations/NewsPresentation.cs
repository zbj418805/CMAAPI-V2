using System.Collections.Generic;
using System.Linq;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Servies;

namespace West.Presence.CMA.Core.Presentations
{
    public interface INewsPresentation
    {
        IEnumerable<News> GetNews(List<int> serverIds, string baseUrl, string searchKey,  int pageIndex, int pageSize, out int total);
    }
    public class NewsPresentation : PresentationBase, INewsPresentation
    {
        private readonly INewsService _newsServise;

        public NewsPresentation(INewsService newsServise)
        {
            _newsServise = newsServise;
        }

        public IEnumerable<News> GetNews(List<int> serverIds, string baseUrl, string searchKey, int pageIndex, int pageSize, out int total)
        {
            var news = _newsServise.GetNews(serverIds, baseUrl, searchKey);
            total = news.Count();

            return GetPageItems<News>(news, pageIndex, pageSize);
        }
    }
}
