﻿using Moq;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Repositories;
using West.Presence.CMA.Core.Servies;
using Xunit;

namespace West.Presence.CMA.Core.Repository.Tests
{
    public class DBNewsRepositoryNews
    {
        private Mock<IDatabaseProvider> mockDatabaseProvider;
        private Mock<IDBConnectionService> mockDBConnectionService;
        private Mock<IDefaultUrlService> mockDefaultUrlService;
        private INewsRepository _newsRepository;

        public DBNewsRepositoryNews()
        {
            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDBConnectionService = new Mock<IDBConnectionService>();
            mockDefaultUrlService = new Mock<IDefaultUrlService>();
        }

        //[Fact]
        public void Test_News_Real_DB_Test() {
            //real DB Test
            string baseUrl = "http://presence.kingzad.local/";
            string dbString = "Data Source=.;Initial Catalog=Presence_QA;User Id=sa;Password=P@ssw0rd";
            mockDBConnectionService.Setup(p => p.GetConnection(baseUrl)).Returns(dbString);
            var _databaseProvider = new DatabaseProvider();

            mockDefaultUrlService.Setup(p => p.GetDefaultUrl(1291956, baseUrl, dbString)).Returns("http://sample.url/");

            _newsRepository = new DBNewsRepository(_databaseProvider, mockDBConnectionService.Object, mockDefaultUrlService.Object);
            var news = _newsRepository.GetNews(1291956, baseUrl);

            Assert.NotEmpty(news);
        }


        [Fact]
        public void Test_News_no_connection_string_found_return_null()
        {
            //Arrange
            string baseUrl = "http://presence.kingzad.local/";
            string dbString = "fake_connection_string";
            int serverId = 123;

            mockDBConnectionService.Setup(p => p.GetConnection(baseUrl)).Returns("");
            IDatabaseProvider _databaseProvider = new DatabaseProvider();

            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDatabaseProvider.Setup(p => p.GetCellValue<string>(dbString, "SELECT url FROM click_server_urls where server_id=@serverId and default_p = 1", It.IsAny<object>(), CommandType.Text)).Returns("http://test.ul/");
            mockDatabaseProvider.Setup(p => p.GetData<RawNews>(dbString, "[dbo].[cma_news_get]", It.IsAny<object>(), CommandType.StoredProcedure)).Returns(GetSampleNews(2));
            mockDefaultUrlService.Setup(p => p.GetDefaultUrl(serverId, baseUrl, dbString)).Returns("http://sample.url/");

            //Action
            _newsRepository = new DBNewsRepository(mockDatabaseProvider.Object, mockDBConnectionService.Object, mockDefaultUrlService.Object);
            var news = _newsRepository.GetNews(serverId, baseUrl);

            //Assert
            Assert.Null(news);
        }

        [Fact]
        public void Test_News_no_news_found_return_empty()
        {
            //Arrange
            string baseUrl = "http://presence.kingzad.local/";
            string dbString = "fake_connection_string";
            int serverId = 123;

            mockDBConnectionService.Setup(p => p.GetConnection(baseUrl)).Returns(dbString);
            IDatabaseProvider _databaseProvider = new DatabaseProvider();

            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDatabaseProvider.Setup(p => p.GetCellValue<string>(dbString, "SELECT url FROM click_server_urls where server_id=@serverId and default_p = 1", It.IsAny<object>(), CommandType.Text)).Returns("http://test.ul/");
            mockDatabaseProvider.Setup(p => p.GetData<RawNews>(dbString, "[dbo].[cma_news_get]", It.IsAny<object>(), CommandType.StoredProcedure)).Returns(GetSampleNews(0));

            mockDefaultUrlService.Setup(p => p.GetDefaultUrl(serverId, baseUrl, dbString)).Returns("http://sample.url/");

            //Action
            _newsRepository = new DBNewsRepository(mockDatabaseProvider.Object, mockDBConnectionService.Object, mockDefaultUrlService.Object);
            var news = _newsRepository.GetNews(serverId, baseUrl);

            //Assert
            Assert.Empty(news);
        }

        [Fact]
        public void Test_News_Repository_Okay()
        {
            //Arragne
            string baseUrl = "http://presence.kingzad.local/";
            string dbString = "fake_connection_string";
            int serverId = 123;

            mockDBConnectionService.Setup(p => p.GetConnection(baseUrl)).Returns(dbString);
            IDatabaseProvider _databaseProvider = new DatabaseProvider();

            mockDatabaseProvider = new Mock<IDatabaseProvider>();
            mockDatabaseProvider.Setup(p => p.GetCellValue<string>(dbString, "SELECT url FROM click_server_urls where server_id=@serverId and default_p = 1", It.IsAny<object>(), CommandType.Text)).Returns("http://test.ul/");
            mockDatabaseProvider.Setup(p => p.GetData<RawNews>(dbString, "[dbo].[cma_news_get]", It.IsAny<object>(), CommandType.StoredProcedure)).Returns(GetSampleNews(5));

            mockDefaultUrlService.Setup(p => p.GetDefaultUrl(serverId, baseUrl, dbString)).Returns("http://sample.url/");

            //Action
            _newsRepository = new DBNewsRepository(mockDatabaseProvider.Object, mockDBConnectionService.Object, mockDefaultUrlService.Object);
            var news = _newsRepository.GetNews(serverId, baseUrl);

            //Assert
            Assert.NotEmpty(news);
            Assert.Equal(5, news.Count());
        }

        private IEnumerable<RawNews> GetSampleNews(int count)
        {
            List<RawNews> rawNews = new List<RawNews>();
            for (int i = 0; i < count; i++)
            {
                rawNews.Add(new RawNews()
                {
                    pageId = 1234,
                    xmlData = @"&lt;data&gt;&lt;title&gt;news 1 - no pubdate&lt;/title&gt;&lt;featured_image&gt;&lt;/featured_image&gt;&lt;image_title&gt;&lt;/image_title&gt;&lt;summary&gt;test test setset&lt;/summary&gt;&lt;published_date&gt;1800/01/01&lt;/published_date&gt;&lt;body&gt;sadfsdfsdfsdfsdfsd sdf sdf sdfsdf&amp;lt;br&amp;gt;&amp;lt;br&amp;gt; Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.&amp;lt;br&amp;gt;&lt;/body&gt;&lt;link_of_current_page&gt;/cms/One.aspx?portalId=1292040&amp;amp;pageId=4798858&lt;/link_of_current_page&gt;&lt;pageTitle&gt;Test news data 1&lt;/pageTitle&gt;&lt;pageLastModified&gt;2016/08/08 01:31:00&lt;/pageLastModified&gt;&lt;/data&gt;",
                });
            }
            return rawNews;
        }

    }
}
