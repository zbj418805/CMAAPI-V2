using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace West.Presence.CMA.Api.Integ.Tests.Controllers
{
    public class SchemaControllerTests : IClassFixture<TestWebApplicationFactory<Startup>>
    {
        private readonly TestWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public SchemaControllerTests(TestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Test_Schema_GetAll()
        {
            var response = await _client.GetAsync("cmaapi/1/schemas");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Test_Schema_GetSchoolSchema()
        {
            var response = await _client.GetAsync("cmaapi/1/schemas/school-messenger.schools");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Test_Schema_GetNewsSchema()
        {
            var response = await _client.GetAsync("cmaapi/1/schemas/school-messenger.news");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Test_Schema_GetEventsSchema()
        {
            var response = await _client.GetAsync("cmaapi/1/schemas/school-messenger.events");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Test_Schema_GetPeopleSchema()
        {
            var response = await _client.GetAsync("cmaapi/1/schemas/school-messenger.people");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
