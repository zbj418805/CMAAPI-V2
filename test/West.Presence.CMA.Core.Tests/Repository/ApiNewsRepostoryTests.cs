using System.Collections.Generic;
using System.Linq;
using Moq;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Repositories;
using Xunit;

namespace West.Presence.CMA.Core.Repository.Tests
{
    public class ApiNewsRepostoryTests
    {
        private Mock<IHttpClientProvider> mockHttpClientProvider;

        public ApiNewsRepostoryTests()
        {

        }

        [Fact]
        public void Test_News_Repository_Get_News()
        {
            var news = GetSampleNews(10).AsEnumerable();

            mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.Setup(p => p.GetData<News>("http://test.url/webapi/cma/news/1234", "PresenceApi")).Returns(news);

            APINewsRepository newsRepo = new APINewsRepository(mockHttpClientProvider.Object);

            var resultNews = newsRepo.GetNews(1234, "http://test.url/");

            Assert.NotNull(resultNews);

            Assert.Equal(10, resultNews.Count());
        }

        [Fact]
        public void Test_News_Repository_Get_No_News()
        {
            var news = GetSampleNews(10).AsEnumerable();

            mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.Setup(p => p.GetData<News>("http://test.url/presence/Api/CMA/News/12344", "PresenceApi")).Returns(news);

            APINewsRepository newsRepo = new APINewsRepository(mockHttpClientProvider.Object);

            var resultNews = newsRepo.GetNews(1234, "http://test.url/");

            Assert.NotNull(resultNews);

            Assert.Empty(resultNews);
        }

        private List<News> GetSampleNews(int count)
        {
            List<News> lsnews = new List<News>();
            for (int i = 0; i < count; i++)
            {
                lsnews.Add(new News()
                {
                    Title = $"MyFirstName_{i}",
                    Body = $"LastName--{i}"
                });
            }

            return lsnews;
        }
    }
}
