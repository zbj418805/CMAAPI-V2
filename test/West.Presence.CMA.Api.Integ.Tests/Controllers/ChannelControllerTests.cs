using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace West.Presence.CMA.Api.Integ.Tests.Controllers
{
    public class ChannelControllerTests : IClassFixture<TestWebApplicationFactory<Startup>>
    {
        private readonly TestWebApplicationFactory<Startup> _factory;

        public ChannelControllerTests(TestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Test_Channel_Get()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("cmaapi/1/channels");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}