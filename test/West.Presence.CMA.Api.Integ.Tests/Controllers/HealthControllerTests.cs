﻿using System.Net;
using System.Threading.Tasks;
using Xunit;
using West.Presence.CMA.Api.Integ.Tests;

namespace West.Presence.CMA.Api.Controllers.Integ.Tests
{
    public class HealthControllerTests : IClassFixture<TestWebApplicationFactory<Startup>>
    {
        private readonly TestWebApplicationFactory<Startup> _factory;

        public HealthControllerTests(TestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Test_Health_Checker()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("api/health/ping");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
