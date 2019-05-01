using System;
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
        IEnumerable<News> GetNews(List<int> serverIds, string baseUrl, string searchKey);
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

        public IEnumerable<News> GetNews(List<int> serverIds, string baseUrl, string searchKey)
        {
            List<News> allNews = new List<News>();
            int cacheDuration = _options.Value.CacheNewsDurationInSeconds;

            Uri u = new Uri(baseUrl);

            foreach (int serverId in serverIds)
            {
                //Set Cache Key
                string cacheKey = $"{_options.Value.CacheNewsKey}_{u.Host}_{serverId}";
                IEnumerable<News> news;
                if (!_cacheProvider.TryGetValue<IEnumerable<News>>(cacheKey, out news))
                {
                    //Get News From Repo
                    news = _newsRepository.GetNews(serverId, baseUrl);
                    //Add to cache
                    _cacheProvider.Add(cacheKey, news, cacheDuration);
                }

                //Add to news collection
                allNews.AddRange(string.IsNullOrEmpty(searchKey) ? news : news.Where(n => n.Title.ToLower().Contains(searchKey) ||
                                                                                            n.Summary.ToLower().Contains(searchKey) ||
                                                                                              n.Body.ToLower().Contains(searchKey)));
            }

            return allNews.OrderByDescending(x => x.PublishedDate);
        }
    }
}
