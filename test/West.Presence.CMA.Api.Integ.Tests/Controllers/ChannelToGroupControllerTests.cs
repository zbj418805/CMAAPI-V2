using System.Net;
using System.Threading.Tasks;
using Xunit;
using West.Presence.CMA.Api.Integ.Tests;

namespace West.Presence.CMA.Api.Controllers.Integ.Tests
{
    public class ChannelToGroupControllerTests : IClassFixture<TestWebApplicationFactory<Startup>>
    {
        private readonly TestWebApplicationFactory<Startup> _factory;

        public ChannelToGroupControllerTests(TestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Test_ChannelToGroup()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("cmaapi/1/shoutem/integration/5/groups?baseurl=http://localhost/");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
