﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using West.Presence.CMA.Api.Controllers;
using Xunit;

namespace West.Presence.CMA.Api.Controllers.Tests
{
    public class HealthControllerTests
    {
        private HealthController _sut;

        [Fact]
        public void Test_HealthEndpoint_Returns_OK()
        {
            // Arrange
            _sut = new HealthController();

            // Act
            var result = _sut.Ping();

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }
    }
}
