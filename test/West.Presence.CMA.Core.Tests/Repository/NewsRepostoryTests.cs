using System.Collections.Generic;
using System.Linq;
using Moq;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Repositories;
using Xunit;

namespace West.Presence.CMA.Core.Tests.Repository
{
    public class NewsRepostoryTests
    {
        private Mock<IHttpClientProvider> mockHttpClientProvider;

        public NewsRepostoryTests()
        {

        }

        [Fact]
        public void Test_News_Repository_Get_News()
        {
            List<News> lsnews = new List<News>();
            for (int i = 0; i < 10; i++)
            {
                lsnews.Add(new News()
                {
                    title = $"MyFirstName_{i}",
                    body = $"LastName--{i}"
                });
            }
            var news = lsnews.AsEnumerable();

            mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.Setup(p => p.GetData<News>("http://test.url/" + "/presence/Api/CMA/News/" + "1234")).Returns(news);

            APINewsRepository newsRepo = new APINewsRepository(mockHttpClientProvider.Object);

            var resultNews = newsRepo.GetNews(1234, "http://test.url/");

            Assert.NotNull(resultNews);

            Assert.Equal(10, resultNews.Count());
        }

        [Fact]
        public void Test_News_Repository_Get_No_News()
        {
            List<News> lsnews = new List<News>();
            for (int i = 0; i < 10; i++)
            {
                lsnews.Add(new News()
                {
                    title = $"MyFirstName_{i}",
                    body = $"LastName--{i}"
                });
            }
            var news = lsnews.AsEnumerable();

            mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.Setup(p => p.GetData<News>("http://test.url/" + "/presence/Api/CMA/News/" + "12344")).Returns(news);

            APINewsRepository newsRepo = new APINewsRepository(mockHttpClientProvider.Object);

            var resultNews = newsRepo.GetNews(1234, "http://test.url/");

            Assert.NotNull(resultNews);

            Assert.Empty(resultNews);
        }
    }
}
