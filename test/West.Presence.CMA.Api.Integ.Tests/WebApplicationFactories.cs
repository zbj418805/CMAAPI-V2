using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;
using West.Presence.CMA.Api;

namespace West.Presence.CMA.Api.Integ.Tests
{
    public class TestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public TestWebApplicationFactory()
        {
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseStartup<CMAAPIStartup>();
        }

        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
            builder.UseEnvironment("IntegrationTests");
            return base.CreateServer(builder);
        }
    }

    public class CMAAPIStartup : Startup
    {
        public CMAAPIStartup(IConfiguration configuration, IHostingEnvironment env, ILogger<Startup> logger)
            : base(configuration, env)
        {
        }

        protected override void ConfigApplicationServices(IServiceCollection services)
        {
            base.ConfigApplicationServices(services);

            ModifyIFileStorageWithStubbedImplementaion(services);
        }

        private static void ModifyIFileStorageWithStubbedImplementaion(IServiceCollection services)
        {
            //services.Remove(services.FirstOrDefault(descriptor => descriptor.ImplementationType == typeof(SmbFileStorage)));
            //services.AddTransient<IFileStorage, SmbStorageIntegrationStub>();
        }

    }
}
