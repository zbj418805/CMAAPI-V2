using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace West.Presence.CMA.Api.Integ.Tests.Controllers
{
    public class CategoriesControllerTests : IClassFixture<TestWebApplicationFactory<Startup>>
    {
        private readonly TestWebApplicationFactory<Startup> _factory;

        public CategoriesControllerTests(TestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Test_Categories_Get()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("cmaapi/1/categories");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
