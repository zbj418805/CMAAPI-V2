using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using West.Presence.CMA.Api.Controllers;
using West.Presence.CMA.Api.Model;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Presentations;
using West.Presence.CMA.Core.Servies;
using Xunit;

namespace West.Presence.CMA.Api.Tests.Controllers
{
    public class NewsControllerTests : BaseControllerTest
    {
        private NewsController _sut;
        private readonly Mock<ISchoolsService> mockSchoolsService;
        private readonly Mock<INewsPresentation> mocknewsPresentation;

        public NewsControllerTests()
        {
            mockSchoolsService = new Mock<ISchoolsService>();
            mocknewsPresentation = new Mock<INewsPresentation>();

            mockSchoolsService.Setup(p => p.GetSchools("http://localhost/", "")).Returns(GetSampleSchools(10,10));

            mockSchoolsService.Setup(p => p.GetSchools("http://noschools/", "")).Returns(GetSampleSchools(0, 10));
        }

        [Fact]
        public void Test_NewsEndpoint_Returns_OK()
        {
            // Arrange
            int total = 0;

            var serverList = new List<int>() { 0, 1, 2, 3, 4 };

            mocknewsPresentation.Setup(p => p.GetNews(serverList, "", "http://localhost/", 0, 20, out total)).Returns(GetSampleNews(10));

            _sut = new NewsController(mockSchoolsService.Object, mocknewsPresentation.Object);
            // Act
            var result = _sut.GetNews(new QueryPagination() { Limit = 20, Offset = 0 },
                new QueryFilter() { Categories = 1, ChannelServerIds = serverList }, "", "http://localhost/");

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_NewsEndpoint_wrong_category_Returns_nocontent()
        {
            // Arrange
            int total = 0;

            var serverList = new List<int>() { 0, 1, 2, 3, 4 };

            mocknewsPresentation.Setup(p => p.GetNews(serverList, "", "http://localhost/", 0, 20, out total)).Returns(GetSampleNews(10));

            _sut = new NewsController(mockSchoolsService.Object, mocknewsPresentation.Object);
            // Act
            var result = _sut.GetNews(new QueryPagination() { Limit = 20, Offset = 0 },
                new QueryFilter() { Categories = 0, ChannelServerIds = serverList }, "", "http://localhost/");

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_NewsEndpoint_nonews_Returns_nocontent()
        {
            // Arrange
            int total = 0;

            var serverList = new List<int>() { 0, 1, 2, 3, 4 };

            mocknewsPresentation.Setup(p => p.GetNews(serverList, "", "http://localhost/", 0, 20, out total)).Returns(GetSampleNews(0));

            _sut = new NewsController(mockSchoolsService.Object, mocknewsPresentation.Object);
            // Act
            var result = _sut.GetNews(new QueryPagination() { Limit = 20, Offset = 0 },
                new QueryFilter() { Categories = 0, ChannelServerIds = serverList }, "", "http://localhost/");

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_NewsEndpoint_noschools_Returns_nocontent()
        {
            // Arrange
            int total = 0;

            var serverList = new List<int>();

            mocknewsPresentation.Setup(p => p.GetNews(serverList, "", "http://noschools/", 0, 20, out total)).Returns(GetSampleNews(0));

            _sut = new NewsController(mockSchoolsService.Object, mocknewsPresentation.Object);
            // Act
            var result = _sut.GetNews(new QueryPagination() { Limit = 20, Offset = 0 },
                new QueryFilter() { Categories = 0, ChannelServerIds = serverList }, "", "http://noschools/");

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_NewsEndpoint_nourl_Returns_nocontent()
        {
            // Arrange
            int total = 0;

            var serverList = new List<int>();

            mocknewsPresentation.Setup(p => p.GetNews(serverList, "", "http://localhost/", 0, 20, out total)).Returns(GetSampleNews(0));

            _sut = new NewsController(mockSchoolsService.Object, mocknewsPresentation.Object);
            // Act
            var result = _sut.GetNews(new QueryPagination() { Limit = 20, Offset = 0 },
                new QueryFilter() { Categories = 0, ChannelServerIds = serverList }, "", "");

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }
    }
}
