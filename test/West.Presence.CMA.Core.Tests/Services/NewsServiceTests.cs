using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Moq;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Repositories;
using West.Presence.CMA.Core.Servies;
using Xunit;

namespace West.Presence.CMA.Core.Tests.Services
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
                Environment = "Dev",
                CacheNewsKey = "CMANewsKey",
                CacheNewsDurationInSeconds = 300
            };

            mockOptions = new Mock<IOptions<CMAOptions>>();
            mockOptions.Setup(p => p.Value).Returns(option);
        }

        [Fact]
        public void Test_News_From_CacheProvider_With_No_Search()
        {
            List<News> lsNews1 = new List<News>();
            for (int i = 0; i < 10; i++)
            {
                lsNews1.Add(new News()
                {
                    title = $"Cached News 1-{i}",
                    body = $"Happened at {1} ..."
                });
            }
            var news1 = lsNews1.AsEnumerable();

            List<News> lsNews2 = new List<News>();
            for (int i = 0; i < 10; i++)
            {
                lsNews2.Add(new News()
                {
                    title = $"Cached News 2-{i}",
                    body = $"Happened at {1} ..."
                });
            }
            var news2 = lsNews2.AsEnumerable();

            mockNewsRepository = new Mock<INewsRepository>();
            //mockNewsRepository.Setup(p => p.GetNews(1)).Returns(lsNews1);

            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<News>>("CMANewsKey_Dev_1", out news1)).Returns(true);
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<News>>("CMANewsKey_Dev_2", out news2)).Returns(true);

            newsService = new NewsService(mockCacheProvider.Object, mockOptions.Object, mockNewsRepository.Object);
            var resultNews = newsService.GetNews("1,2", "");

            Assert.NotNull(resultNews);
            Assert.Equal(20, resultNews.Count());
        }

        [Fact]
        public void Test_News_From_Repo_With_No_Search()
        {
            List<News> lsNews1 = new List<News>();
            for (int i = 0; i < 10; i++)
            {
                lsNews1.Add(new News()
                {
                    title = $"Repo News 1-{i}",
                    body = $"Happened at {1} ..."
                });
            }
            var news1 = lsNews1.AsEnumerable();

            List<News> lsNews2 = new List<News>();
            for (int i = 0; i < 10; i++)
            {
                lsNews2.Add(new News()
                {
                    title = $"Repo News 2-{i}",
                    body = $"Happened at {1} ..."
                });
            }
            var news2 = lsNews2.AsEnumerable();

            mockCacheProvider = new Mock<ICacheProvider>();
            //mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<News>>("CMANewsKey_Dev_1", out news1)).Returns(true);
            //mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<News>>("CMANewsKey_Dev_2", out news2)).Returns(true);

            mockNewsRepository = new Mock<INewsRepository>();
            mockNewsRepository.Setup(p => p.GetNews(1)).Returns(lsNews1);
            mockNewsRepository.Setup(p => p.GetNews(2)).Returns(lsNews1);

            newsService = new NewsService(mockCacheProvider.Object, mockOptions.Object, mockNewsRepository.Object);
            var resultNews = newsService.GetNews("1,2", "");

            Assert.NotNull(resultNews);
            Assert.Equal(20, resultNews.Count());
        }

        [Fact]
        public void Test_News_From_CacheProvider_And_Repo_With_No_Search()
        {
            List<News> lsNews1 = new List<News>();
            for (int i = 0; i < 10; i++)
            {
                lsNews1.Add(new News()
                {
                    title = $"Cached News {i}",
                    body = $"Happened at {1} ..."
                });
            }
            var news1 = lsNews1.AsEnumerable();

            List<News> lsNews2 = new List<News>();
            for (int i = 0; i < 10; i++)
            {
                lsNews2.Add(new News()
                {
                    title = $"Repo News {i}",
                    body = $"Happened at {1} ..."
                });
            }
            var news2 = lsNews2.AsEnumerable();

            mockNewsRepository = new Mock<INewsRepository>();
            mockNewsRepository.Setup(p => p.GetNews(1)).Returns(lsNews1);

            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<News>>("CMANewsKey_Dev_2", out news2)).Returns(true);

            newsService = new NewsService(mockCacheProvider.Object, mockOptions.Object, mockNewsRepository.Object);
            var resultNews = newsService.GetNews("1,2", "");

            Assert.NotNull(resultNews);
            Assert.Equal(20, resultNews.Count());
        }

        [Fact]
        public void Test_News_From_CacheProvider_And_Repo_With_Search()
        {
            List<News> lsNews1 = new List<News>();
            for (int i = 0; i < 10; i++)
            {
                lsNews1.Add(new News()
                {
                    title = $"Cached News {i}",
                    body = $"Happened at {1} ..."
                });
            }
            var news1 = lsNews1.AsEnumerable();

            List<News> lsNews2 = new List<News>();
            for (int i = 0; i < 10; i++)
            {
                lsNews2.Add(new News()
                {
                    title = $"Repo News {i}",
                    body = $"Happened at {1} ..."
                });
            }
            var news2 = lsNews2.AsEnumerable();

            mockNewsRepository = new Mock<INewsRepository>();
            mockNewsRepository.Setup(p => p.GetNews(1)).Returns(lsNews1);

            mockCacheProvider = new Mock<ICacheProvider>();
            mockCacheProvider.Setup(p => p.TryGetValue<IEnumerable<News>>("CMANewsKey_Dev_2", out news2)).Returns(true);

            newsService = new NewsService(mockCacheProvider.Object, mockOptions.Object, mockNewsRepository.Object);
            var resultNews = newsService.GetNews("1,2", "1");

            Assert.NotNull(resultNews);
            Assert.Equal(2, resultNews.Count());
        }
    }
}
