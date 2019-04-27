﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using West.Presence.CMA.Api.Controllers;
using West.Presence.CMA.Api.Model;
using West.Presence.CMA.Core.Presentations;
using West.Presence.CMA.Core.Servies;
using Xunit;

namespace West.Presence.CMA.Api.Tests.Controllers
{
    public class EventsControllerTests : BaseControllerTest
    {
        private EventsController _sut;
        private readonly Mock<ISchoolsService> mockSchoolsService;
        private readonly Mock<IEventsPresentation> mockEventsPresentation;

        public EventsControllerTests()
        {
            mockSchoolsService = new Mock<ISchoolsService>();
            mockEventsPresentation = new Mock<IEventsPresentation>();

            mockSchoolsService.Setup(p => p.GetSchools("http://localhost/", "")).Returns(GetSampleSchools(10, 10));
            mockSchoolsService.Setup(p => p.GetSchools("http://noschools/", "")).Returns(GetSampleSchools(0, 10));
        }

        [Fact]
        public void Test_EventsEndpoint_Returns_OK()
        {
            // Arrange
            int total = 0;
            var serverList = new List<int>() { 0, 1, 2, 3, 4 };

            mockEventsPresentation.Setup(p => p.GetEvents(serverList, "http://localhost", "", System.DateTime.Today, System.DateTime.Today.AddMonths(12), 0, 20, out total)).Returns(GetSampleEvents(10));

            _sut = new EventsController(mockSchoolsService.Object, mockEventsPresentation.Object);
            // Act
            var result = _sut.GetEvents(new QueryPagination() { Limit = 20, Offset = 0 },
                new QueryFilter() { Categories = 3, ChannelServerIds = serverList }, "", "http://localhost/");

            // Assert
            Assert.IsType<NoContentResult>(result);
            //Assert.IsType<OkObjectResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_EventlEndpoint_NoSchools_Returns_NoContent()
        {
            // Arrange
            int total = 0;
            var serverList = new List<int>() { 0, 1, 2, 3, 4 };

            mockEventsPresentation.Setup(p => p.GetEvents(serverList, "http://noschools/", "", System.DateTime.Today, System.DateTime.Today.AddMonths(12), 0, 20, out total)).Returns(GetSampleEvents(10));

            _sut = new EventsController(mockSchoolsService.Object, mockEventsPresentation.Object);
            // Act
            var result = _sut.GetEvents(new QueryPagination() { Limit = 20, Offset = 0 },
                new QueryFilter() { Categories = 3, ChannelServerIds = serverList }, "", "http://noschools/");

            // Assert
            Assert.IsType<NoContentResult>(result);
            //Assert.IsType<OkObjectResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_EventlEndpoint_MissCategory_Returns_NoContent()
        {
            // Arrange
            int total = 0;
            var serverList = new List<int>() { 0, 1, 2, 3, 4 };

            mockEventsPresentation.Setup(p => p.GetEvents(serverList, "http://localhost/", "", System.DateTime.Today, System.DateTime.Today.AddMonths(12), 0, 20, out total)).Returns(GetSampleEvents(10));

            _sut = new EventsController(mockSchoolsService.Object, mockEventsPresentation.Object);
            // Act
            var result = _sut.GetEvents(new QueryPagination() { Limit = 20, Offset = 0 },
                new QueryFilter() { Categories = 0, ChannelServerIds = serverList }, "", "http://localhost/");

            // Assert
            Assert.IsType<NoContentResult>(result);
            //Assert.IsType<OkObjectResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_EventsEndpoint_NoEvents_Returns_Content()
        {
            // Arrange
            int total = 0;
            var serverList = new List<int>() { 0, 1, 2, 3, 4 };

            mockEventsPresentation.Setup(p => p.GetEvents(serverList, "http://localhost", "", System.DateTime.Today, System.DateTime.Today.AddMonths(12), 0, 20, out total)).Returns(GetSampleEvents(0));

            _sut = new EventsController(mockSchoolsService.Object, mockEventsPresentation.Object);
            // Act
            var result = _sut.GetEvents(new QueryPagination() { Limit = 20, Offset = 0 },
                new QueryFilter() { Categories = 3, ChannelServerIds = serverList }, "", "http://localhost/");

            // Assert
            Assert.IsType<NoContentResult>(result);
            //Assert.IsType<OkObjectResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }

        [Fact]
        public void Test_EventsEndpoint_Nourl_Returns_Content()
        {
            // Arrange
            int total = 0;
            var serverList = new List<int>() { 0, 1, 2, 3, 4 };

            mockEventsPresentation.Setup(p => p.GetEvents(serverList, "http://localhost", "", System.DateTime.Today, System.DateTime.Today.AddMonths(12), 0, 20, out total)).Returns(GetSampleEvents(10));

            _sut = new EventsController(mockSchoolsService.Object, mockEventsPresentation.Object);
            // Act
            var result = _sut.GetEvents(new QueryPagination() { Limit = 20, Offset = 0 },
                new QueryFilter() { Categories = 3, ChannelServerIds = serverList }, "", "");

            // Assert
            Assert.IsType<NoContentResult>(result);
            //Assert.IsType<OkObjectResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }
    }
}
