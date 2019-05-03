using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Moq;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Repositories;
using West.Presence.CMA.Core.Servies;
using Xunit;

namespace West.Presence.CMA.Core.Services.Tests
{
    public class NewsServiceTests
    {
        private Mock<INewsRepository> mockNewsRepository;
        private Mock<ICacheProvider> mockCacheProvider;
        private Mock<IOptions<CMAOptions>> mockOptions;
        private INewsService newsService;

        public NewsServiceTests()
        {
            CMAOptions option = new CMAOptions
            {
                //Environment = "Dev",
                CacheNewsKey = "CMANewsKey",
                CacheNewsDurationInSeconds = 300
            };

            mockOptions = new Mock<IOptions<CMAOptions>>();
            mockOptions.Setup(p => p.Value).Returns(option);
        }

        [Fact]
        public void Test_News_From_CacheProvider_With_No_Search()
        {
            List<News> lsNews1 = GetSampleNews(10, "Repo");
            var news1 = lsNews1.AsEnumerable();
            List<News> lsNews2 = GetSampleNews(10, "Cached");
            var news2 = lsNews2.AsEnumerable();

            mockNewsRepository = new Mock<INewsRepository>();
            //mockNewsRepository.Setup(p => p.GetNews(1)).Returns(lsNews1);

            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<News>>("CMANewsKey_localhost_1", out news1)).Returns(true);
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<News>>("CMANewsKey_localhost_2", out news2)).Returns(true);

            newsService = new NewsService(mockCacheProvider.Object, mockOptions.Object, mockNewsRepository.Object);
            var resultNews = newsService.GetNews(new List<int>() { 1, 2 }, "http://localhost/", "");

            Assert.NotNull(resultNews);
            Assert.Equal(20, resultNews.Count());
        }

        [Fact]
        public void Test_News_From_Repo_With_No_Search()
        {
            List<News> lsNews1 = GetSampleNews(10, "Repo");
            var news1 = lsNews1.AsEnumerable();
            List<News> lsNews2 = GetSampleNews(10, "Repo");
            var news2 = lsNews2.AsEnumerable();

            mockCacheProvider = new Mock<ICacheProvider>();
            //mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<News>>("CMANewsKey_Dev_1", out news1)).Returns(true);
            //mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<News>>("CMANewsKey_Dev_2", out news2)).Returns(true);

            mockNewsRepository = new Mock<INewsRepository>();
            mockNewsRepository.Setup(p => p.GetNews(1, "http://localhost/")).Returns(lsNews1);
            mockNewsRepository.Setup(p => p.GetNews(2, "http://localhost/")).Returns(lsNews1);

            newsService = new NewsService(mockCacheProvider.Object, mockOptions.Object, mockNewsRepository.Object);
            var resultNews = newsService.GetNews(new List<int>() { 1, 2 }, "http://localhost/", "");

            Assert.NotNull(resultNews);
            Assert.Equal(20, resultNews.Count());
        }

        [Fact]
        public void Test_News_From_CacheProvider_And_Repo_With_No_Search()
        {
            List<News> lsNews1 = GetSampleNews(10, "Cached");
            var news1 = lsNews1.AsEnumerable();
            List<News> lsNews2 = GetSampleNews(10, "Repo");
            var news2 = lsNews2.AsEnumerable();

            mockNewsRepository = new Mock<INewsRepository>();
            mockNewsRepository.Setup(p => p.GetNews(1, "http://localhost/")).Returns(lsNews1);

            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<News>>("CMANewsKey_localhost_2", out news2)).Returns(true);

            newsService = new NewsService(mockCacheProvider.Object, mockOptions.Object, mockNewsRepository.Object);
            var resultNews = newsService.GetNews(new List<int>() { 1, 2 }, "http://localhost/", "");

            Assert.NotNull(resultNews);
            Assert.Equal(20, resultNews.Count());
        }

        [Fact]
        public void Test_News_From_CacheProvider_And_Repo_With_Search()
        {
            List<News> lsNews1 = GetSampleNews(10, "Cached");
            var news1 = lsNews1.AsEnumerable();
            List<News> lsNews2 = GetSampleNews(10, "Repo");
            var news2 = lsNews2.AsEnumerable();

            mockNewsRepository = new Mock<INewsRepository>();
            mockNewsRepository.Setup(p => p.GetNews(1, "http://localhost/")).Returns(lsNews1);

            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<News>>("CMANewsKey_localhost_2", out news2)).Returns(true);

            newsService = new NewsService(mockCacheProvider.Object, mockOptions.Object, mockNewsRepository.Object);
            var resultNews = newsService.GetNews(new List<int>() { 1, 2 }, "http://localhost/", "1");

            Assert.NotNull(resultNews);
            Assert.Equal(2, resultNews.Count());
        }

        private List<News> GetSampleNews(int count, string title)
        {
            List<News> lsNews = new List<News>();
            for(int i =0; i < count; i++)
            {
                lsNews.Add(new News()
                {
                    Title = $"{title} News {i}",
                    Body = $"Happened at {i} ...",
                    Summary = "asdfas"
                });
            }
            return lsNews;
        }
    }
}
