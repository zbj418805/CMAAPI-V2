using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace West.Presence.CMA.Api.Integ.Tests.Controllers
{
    public class PeopleControllerTests : IClassFixture<TestWebApplicationFactory<Startup>>
    {

        private readonly TestWebApplicationFactory<Startup> _factory;

        public PeopleControllerTests(TestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Test_Get_People()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("cmaapi/1/resources/school-messenger.people");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
