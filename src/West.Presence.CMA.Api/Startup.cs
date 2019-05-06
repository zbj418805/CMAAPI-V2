using System.IO.Compression;
using System.Net.Http.Headers;
using System;
using GlobalExceptionHandler.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.CloudFoundry.Connector.Redis;
using System.Diagnostics.CodeAnalysis;
using West.Presence.CMA.Api.Infrastructure;
using West.Presence.CMA.Api.Utilities;
using West.Presence.CMA.Core.Helper;
using West.Presence.CMA.Core.Servies;
using West.Presence.CMA.Core.Presentations;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Repositories;
using Microsoft.AspNetCore.ResponseCompression;


namespace West.Presence.CMA.Api
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        private readonly ILogger _logger;
        //private readonly CMAOptions _options;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {

            // Configuration was initialized with WebHostBuilder class with pre-configured defaults.
            // https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.webhost.createdefaultbuilder?view=aspnetcore-2.1

            // User secrets are enabled - For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
            // You can edit your file directly at %APPDATA%\microsoft\UserSecrets\West.Presence.GoogleUpdater.WebApi\secrets.json
            Configuration = configuration;
            _env = env;
            _logger = Log.ForContext<Startup>();
            _logger.Information($"Starting: {Utility.ApplicationName()}");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Config Logging
            LogConfig();

            //Config Mvc
            ConfigMvc(services);

            // Add Response Compression Middleware
            ConfigureResponseCompression(services);

            // Add Swagger
            AddSwagger(services);

            // Adds IConfiguration and IConfigurationRoot to service container
            ConfigureOptionsAndConfigurations(services);

            // Add our own DI components using .NET Core DI syntax
            ConfigApplicationServices(services);

            // Add Distributed Cache Service.
            ConfigureDistributedCache(services);

            // Add HttpClient Service
            ConfigureHttpclient(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            // Ensure any buffered events are sent at shutdown
            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
            
            app.UseGlobalExceptionHandler(x =>
            {
                x.ContentType = "application/json";
                x.ResponseBody((exception, context) => JsonConvert.SerializeObject(new
                {
                    Message = $"An error occured while processing your request. RequestId: {context.TraceIdentifier}"
                }));
            });

            // app.UseMiddleware<SerilogMiddleware>();
            app.UseDefaultFiles();

            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }
            // else
            // {
            //     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //     app.UseHsts();
            // }

            app.UseStaticFiles();
            //Enable ResponseCompression
            app.UseResponseCompression();

            //Enable Https
            //app.UseHttpsRedirection();
            app.UseMvc();

            UseSwagger(app);
        }

        [ExcludeFromCodeCoverage]
        private void UseSwagger(IApplicationBuilder app)
        {
            if (_env.IsEnvironment("IntegrationTests")) return;

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(SwaggerConfig.SetupSwaggerUiOptions());
        }

        [ExcludeFromCodeCoverage]
        private void AddSwagger(IServiceCollection services)
        {
            if (_env.IsEnvironment("IntegrationTests")) return;
            // Add Swagger
            services.AddSwaggerGen(SwaggerConfig.SetupSwaggerGenOptions());
        }

        private void LogConfig()
        {
            //var loggingOptions = Configuration.GetSection("LoggingOptions").Get<LoggingOptions>();
            //_logger.Debug($"LoggingOptions:RollingFile:Enabled = {loggingOptions.RollingFile.Enabled}");
            //if (loggingOptions.RollingFile.Enabled)
            //_logger.Debug($"LoggingOptions:RollingFile:Filepath = {loggingOptions.RollingFile.FilePath}");

            //var configServerClientSettingsOptions = Configuration.GetSection("spring:cloud:config").Get<ConfigServerClientSettingsOptions>();
            //_logger.Debug($"spring:cloud:config:name = {configServerClientSettingsOptions.Name}");
            //_logger.Debug($"spring:cloud:config:env = {configServerClientSettingsOptions.Env}");
            //_logger.Debug($"spring:cloud:config:enabled = {configServerClientSettingsOptions.Enabled}");
            //_logger.Debug($"spring:cloud:config:failFast = {configServerClientSettingsOptions.FailFast}");
            //_logger.Debug($"spring:cloud:config:uri = {configServerClientSettingsOptions.Uri}");
        }

        private static void ConfigMvc(IServiceCollection services)
        {
            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    });
        }

        //Config Compression
        private static void ConfigureResponseCompression(IServiceCollection services)
        {
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });
        }

        //Add ConfigrationAndOptions
        private void ConfigureOptionsAndConfigurations(IServiceCollection services)
        {
            //services.AddConfiguration(Configuration);
            services.AddOptions();
            // Adds CloudFoundryApplicationOptions and CloudFoundryServicesOptions to service container
            services.ConfigureCloudFoundryOptions(Configuration);
            // Adds ConfigServerClientOptions to service container
            //services.ConfigureConfigServerClientOptions(Configuration);
            services.Configure<CMAOptions>(Configuration.GetSection("CMAOptions"));
            //services.Configure<RabbitMqOptions>(Configuration.GetSection("RabbitMqOptions"));
        }

        //Dependency Injection for applciation services
        protected virtual void ConfigApplicationServices(IServiceCollection services)
        {
            services.AddSingleton<ICacheProvider, CacheProvider>();
            services.AddSingleton<IDatabaseProvider, DatabaseProvider>();
            services.AddSingleton<IHttpClientProvider, HttpClientProvider>();

            //Add Presentation Layer
            services.AddSingleton<IEventsPresentation, EventsPresentation>();
            services.AddSingleton<INewsPresentation, NewsPresentation>();
            services.AddSingleton<IPeoplePresentation, PeoplePresentation>();
            services.AddSingleton<ISchoolsPresentation, SchoolsPresentation>();
            
            //Add Service Layer
            services.AddTransient<IEventsService, EventsService>();
            services.AddSingleton<INewsService, NewsService>();
            services.AddSingleton<IPeopleService, PeopleService>();
            services.AddSingleton<ISchoolsService, SchoolsService>();
            
            //Add DB Service Layer
            services.AddSingleton<IDBConnectionService, DBConnectionService>();
            services.AddSingleton<IPeopleSettingsService, PeopleSettingsService>();
            services.AddSingleton<IDefaultUrlService, DefaultUrlService>();

            //Add API Repository Layer
            //services.AddSingleton<ISchoolsRepository, APISchoolsRepository>();
            //services.AddSingleton<IEventsRepository, APIEventsRepository>();
            //services.AddSingleton<INewsRepository, APINewsRepository>();
            //services.AddSingleton<IPeopleRepository, APIPeopleRepository>();
            //services.AddSingleton<IChannel2GroupRepository, APIChannel2GroupRepository>();

            //Add DBRepository Layer
            services.AddSingleton<ISchoolsRepository, DBSchoolsRepository>();
            services.AddSingleton<IEventsRepository, APIEventsRepository>();
            services.AddSingleton<INewsRepository, DBNewsRepository>();
            services.AddSingleton<IPeopleRepository, DBPeopleRepository>();
            services.AddSingleton<IChannel2GroupRepository, DBChannel2GroupRepository>();
            services.AddSingleton<IConnectionRepository, APIConectionRepository>();
            services.AddSingleton<IPeopleSettingsRepository, PeopleSettingsRepository>();
            services.AddSingleton<IDefaultUrlRepository, DefaultUrlRepository>();

        }

        private void ConfigureDistributedCache(IServiceCollection services)
        {
            if (Utility.IsPcf())
            {
                services.AddDistributedRedisCache(Configuration);
            }
            else
            {
                if (_env.IsEnvironment("IntegrationTests") || _env.IsEnvironment("Development"))
                    services.AddDistributedMemoryCache();
                else
                    services.AddDistributedRedisCache(option =>
                    {
                        option.Configuration = "localhost";
                        option.InstanceName = "CMAAPI";
                    });
            }
        }

        private void ConfigureHttpclient(IServiceCollection services)
        {
            var cmaOptions = Configuration.GetSection("CMAOptions").Get<CMAOptions>();

            services.AddHttpClient("PresenceApi", c =>
            {
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cmaOptions.PresenceAccessToken);
            });

            services.AddHttpClient("CentralServiceApi", c =>
            {
                c.BaseAddress = new Uri(cmaOptions.CentralServiceUrl);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }

        private void ConfigureCloudFoundryServicesAndActuators(IServiceCollection services)
        {
            //services.AddDistributedTracing(Configuration);
            //services.addCloudFoundryActuators(Configuration);
            //services.AddRefreshActuator(Configuration);
            //if (!_env.IsEnvironment("IntegrationTests"))
            //    services.AddConfigurationDiscoveryClient(Configuration);
        }
    }
}
