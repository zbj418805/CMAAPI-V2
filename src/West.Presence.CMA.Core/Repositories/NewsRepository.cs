using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Servies;

namespace West.Presence.CMA.Core.Repositories
{
    public interface INewsRepository
    {
        IEnumerable<News> GetNews(int serverId, string baseUrl);
    }

    public class DBNewsRepository : INewsRepository
    {
        IDatabaseProvider _databaseProvider;
        IDBConnectionService _dbConnectionService;
        private readonly ILogger _logger = Log.ForContext<DBNewsRepository>();

        public DBNewsRepository(IDatabaseProvider databaseProvider, IDBConnectionService dbConnectionService)
        {
            _databaseProvider = databaseProvider;
            _dbConnectionService = dbConnectionService;
        }

        public IEnumerable<News> GetNews(int serverId, string baseUrl)
        {
            string connectionStr = _dbConnectionService.GetConnection(baseUrl);

            if (string.IsNullOrEmpty(connectionStr))
            {
                _logger.Error($"Get connection string based on {baseUrl} failed.");
                return null;
            }

            string serverUrl = _databaseProvider.GetCellValue<string>(connectionStr, "SELECT url FROM click_server_urls where server_id=@serverId and default_p = 1", new { serverId = serverId }, CommandType.Text);
            if(string.IsNullOrEmpty(serverUrl))
            {
                _logger.Information($"No url found based on server id {serverId}.");
            }

            var rawNews = _databaseProvider.GetData<RawNews>(connectionStr, "[dbo].[cma_news_get]", new { server_id = serverId }, CommandType.StoredProcedure);

            List<News> news = new List<News>();
            foreach(RawNews rn in rawNews)
            {
                News n = LoadXmlData(rn.xmlData);
                if (string.IsNullOrEmpty(n.Body) &&
                        string.IsNullOrEmpty(n.Title) &&
                        string.IsNullOrEmpty(n.Summary))
                {
                    continue;
                }
                n.ServerId = serverId;

                if (n.PublishedDate.Year < 1900)
                    n.PublishedDate = n.PageLastModified;

                n.Body = n.Body.Replace("href=\"/", "href=" + "\"" + serverUrl).Replace("src=\"/", "src=" + "\"" + serverUrl);

                //AG3070 add abolutely url featuerImage and linkofCurrentPage
                if (!string.IsNullOrEmpty(n.LinkOfCurrentPage) && !n.LinkOfCurrentPage.StartsWith("http"))
                    n.LinkOfCurrentPage = serverUrl + n.LinkOfCurrentPage;
                if (!string.IsNullOrEmpty(n.FeaturedImage) && !n.FeaturedImage.StartsWith("http"))
                    n.FeaturedImage = serverUrl + n.FeaturedImage;

                news.Add(n);
            }
            return news;
        }

        private News LoadXmlData(string xml)
        {
            var news = new News();
            news.Title = "";
            news.Body = "";
            news.Summary = "";

            XmlDocument xdata = new XmlDocument();
            try
            {
                xdata.Load(new System.IO.StringReader(HttpUtility.HtmlDecode(xml)));

                foreach (XmlNode cn in xdata.DocumentElement.ChildNodes)
                {
                    if (cn.InnerText.Trim().Length > 0)
                    {
                        switch (cn.Name)
                        {
                            case "title":
                                news.Title = cn.InnerText;
                                break;
                            case "published_date":
                                news.PublishedDate = DateTime.Parse(cn.InnerText);
                                break;
                            case "image_title":
                                news.ImageTitle = cn.InnerText;
                                break;
                            case "body":
                                news.Body = HTML_TrimStyleSection(cn.InnerText);
                                break;
                            case "summary":
                                news.Summary = HTML_TrimStyleSection(cn.InnerText);
                                break;
                            case "link_of_current_page":
                                news.LinkOfCurrentPage = cn.InnerText;
                                break;
                            case "pageTitle":
                                news.PageTitle = cn.InnerText;
                                break;
                            case "pageLastModified":
                                news.PageLastModified = DateTime.Parse(cn.InnerText);
                                break;
                            case "featured_image":
                                news.FeaturedImage = cn.InnerText;
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error("xml transformation error, " + e.Message);
            }

            if (news.Summary.Length == 0)
            {
                news.Summary = HTML_TrimHtmlTag(news.Body);
            }

            return news;
        }

        private string HTML_TrimStyleSection(string htmlText)
        {
            try
            {
                var regex = new Regex("(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)",
                                   RegexOptions.Singleline | RegexOptions.IgnoreCase);
                htmlText = regex.Replace(htmlText, "");
            }
            catch { }

            return htmlText;
        }

        private string HTML_TrimHtmlTag(string htmlText)
        {
            try
            {
                var regex = new Regex("(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)",
                                   RegexOptions.Singleline | RegexOptions.IgnoreCase);
                htmlText = regex.Replace(htmlText, "");
                htmlText = Regex.Replace(htmlText, @"<[^>]*>", String.Empty);
                htmlText = htmlText.Replace("&nbsp;", " ");
                htmlText = HttpUtility.HtmlDecode(htmlText);
            }
            catch { }

            return htmlText;
        }
    }

    public class APINewsRepository : INewsRepository
    {
        private readonly IHttpClientProvider _httpClientProvider;

        public APINewsRepository(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
        }

        public IEnumerable<News> GetNews(int serverId, string baseUrl)
        {
            return _httpClientProvider.GetData<News>($"{baseUrl}webapi/cma/news/{serverId}", "PresenceApi");
        }
    }
}
