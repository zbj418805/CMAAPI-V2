using Moq;
using System.Collections.Generic;
using System.Linq;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Presentations;
using West.Presence.CMA.Core.Servies;
using Xunit;

namespace West.Presence.CMA.Core.Tests.Presentation
{

    public class NewsPresentationTests
    {
        private Mock<INewsService> mockNewsService;
        private INewsPresentation newsPresentation;

        public NewsPresentationTests()
        {
            List<News> news = new List<News>();

            for (int i = 0; i < 10; i++)
            {
                news.Add(new News()
                {
                    title = $"Title {i}",
                    body = $"Body {i} ..."
                });
            }

            mockNewsService = new Mock<INewsService>();
            mockNewsService.Setup(p => p.GetNews("1,2", "")).Returns(news);
            newsPresentation = new NewsPresentation(mockNewsService.Object);
        }

        [Fact]
        public void Test_News_First_Page()
        {
            IEnumerable<News> sampleEvents = newsPresentation.GetNews("1,2", "", 0, 2);

            Assert.NotNull(sampleEvents);
            Assert.Equal(2, sampleEvents.Count());
        }

        [Fact]
        public void Test_News_Second_Page()
        {
            IEnumerable<News> sampleNews = newsPresentation.GetNews("1,2", "", 1,3);

            Assert.NotNull(sampleNews);
            Assert.Equal(3, sampleNews.Count());
            Assert.Equal("Title 3", sampleNews.FirstOrDefault().title);
            Assert.Equal("Title 4", sampleNews.Skip(1).Take(1).FirstOrDefault().title);
            Assert.Equal("Title 5", sampleNews.LastOrDefault().title);
        }


        [Fact]
        public void Test_News_Page_with_no_items()
        {
            IEnumerable<News> sampleNews = newsPresentation.GetNews("1,2", "", 5, 3);

            Assert.NotNull(sampleNews);
            Assert.Empty(sampleNews);
        }
    }
}
