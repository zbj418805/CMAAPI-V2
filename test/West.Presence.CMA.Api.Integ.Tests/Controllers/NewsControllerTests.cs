using System.Net;
using System.Threading.Tasks;
using Xunit;


namespace West.Presence.CMA.Api.Integ.Tests.Controllers
{
    public class NewsControllerTests : IClassFixture<TestWebApplicationFactory<Startup>>
    {
        private readonly TestWebApplicationFactory<Startup> _factory;

        public NewsControllerTests(TestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Test_Get_News()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("cmaapi/1/resources/school-messenger.news");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
