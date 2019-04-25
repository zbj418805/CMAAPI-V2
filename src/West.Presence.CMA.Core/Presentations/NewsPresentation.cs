using System.Collections.Generic;
using System.Linq;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Servies;

namespace West.Presence.CMA.Core.Presentations
{
    public interface INewsPresentation
    {
        IEnumerable<News> GetNews(string serverIds, string baseUrl, string searchKey,  int pageIndex, int pageSize);
    }
    public class NewsPresentation : PresentationBase, INewsPresentation
    {
        private readonly INewsService _newsServise;

        public NewsPresentation(INewsService newsServise)
        {
            _newsServise = newsServise;
        }

        public IEnumerable<News> GetNews(string serverIds, string baseUrl, string searchKey, int pageIndex, int pageSize)
        {
            return GetPageItems<News>(_newsServise.GetNews(serverIds, baseUrl, searchKey), pageIndex, pageSize);
        }
    }
}
