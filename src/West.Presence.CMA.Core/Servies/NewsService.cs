using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Repositories;

namespace West.Presence.CMA.Core.Servies
{
    public interface INewsService
    {
        IEnumerable<News> GetNews(string serverIds, string searchKey);
    }

    public class NewsService : INewsService
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly IOptions<CMAOptions> _options;
        private readonly INewsRepository _newsRepository;

        public NewsService(ICacheProvider cacheProvider, IOptions<CMAOptions> options, INewsRepository newsRepository)
        {
            _cacheProvider = cacheProvider;
            _options = options;
            _newsRepository = newsRepository;
        }

        public IEnumerable<News> GetNews(string serverIds, string searchKey)
        {
            List<News> allNews = new List<News>();
            int cacheDuration = _options.Value.CacheNewsDurationInSeconds;
            if (searchKey == "")
            {
                foreach (string serverId in serverIds.Split(','))
                {
                    string cacheKey = $"{_options.Value.CacheNewsKey}_{serverId}";
                    
                    IEnumerable<News> news;
                    if (_cacheProvider.TryGetValue<IEnumerable<News>>(cacheKey, out news))
                    {
                        allNews.AddRange(news);
                    }
                    else
                    {
                        news = _newsRepository.GetNews(int.Parse(serverId), "");
                        allNews.AddRange(news);
                        _cacheProvider.Add(cacheKey, news, cacheDuration);
                    }
                }
            }
            else
            {
                foreach (string serverId in serverIds.Split(','))
                {
                    allNews.AddRange(_newsRepository.GetNews(int.Parse(serverId), searchKey));
                }
            }

            return allNews.OrderByDescending(x => x.publishDate);
        }
    }
}
