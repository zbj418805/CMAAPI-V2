using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Net.Security;
using System.Security.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
//using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using GlobalExceptionHandler.WebApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Pivotal.Extensions.Configuration.ConfigServer;
using Serilog;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Extensions.Configuration.ConfigServer;
using West.Presence.CMA.Api.Infrastructure;

namespace West.Presence.CMA.Api
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        private readonly ILogger _logger;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {

            // Configuration was initialized with WebHostBuilder class with pre-configured defaults.
            // https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.webhost.createdefaultbuilder?view=aspnetcore-2.1

            // User secrets are enabled - For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
            // You can edit your file directly at %APPDATA%\microsoft\UserSecrets\West.Presence.GoogleUpdater.WebApi\secrets.json
            Configuration = configuration;
            _env = env;
            _logger = Log.ForContext<Startup>();
            _logger.Information($"Starting: CMAAPI"); // Utility.ApplicationName()
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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
            //app.UseResponseCompression();

            //Enable Https
            //app.UseHttpsRedirection();
            app.UseMvc();

            UseSwagger(app);
        }

        [ExcludeFromCodeCoverage]
        private void UseSwagger(IApplicationBuilder app)
        {
            //if (_env == null || _env.IsEnvironment("IntegrationTests")) return;

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(SwaggerConfig.SetupSwaggerUiOptions());
        }

        [ExcludeFromCodeCoverage]
        private void AddSwagger(IServiceCollection services)
        {
            //if (_env == null ||  _env.IsEnvironment("IntegrationTests")) return;
            // Add Swagger
            services.AddSwaggerGen(SwaggerConfig.SetupSwaggerGenOptions());
        }

        private void LogConfig()
        {
            // var loggingOptions = Configuration.GetSection("LoggingOptions").Get<LoggingOptions>();
            // _logger.Debug($"LoggingOptions:RollingFile:Enabled = {loggingOptions.RollingFile.Enabled}");
            // if (loggingOptions.RollingFile.Enabled)
            //     _logger.Debug($"LoggingOptions:RollingFile:Filepath = {loggingOptions.RollingFile.FilePath}");

            // var configServerClientSettingsOptions = Configuration.GetSection("spring:cloud:config").Get<ConfigServerClientSettingsOptions>();
            // _logger.Debug($"spring:cloud:config:name = {configServerClientSettingsOptions.Name}");
            // _logger.Debug($"spring:cloud:config:env = {configServerClientSettingsOptions.Env}");
            // _logger.Debug($"spring:cloud:config:enabled = {configServerClientSettingsOptions.Enabled}");
            // _logger.Debug($"spring:cloud:config:failFast = {configServerClientSettingsOptions.FailFast}");
            // _logger.Debug($"spring:cloud:config:uri = {configServerClientSettingsOptions.Uri}");
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
            //services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
            // services.AddResponseCompression(options =>
            // {
            //     options.Providers.Add<GzipCompressionProvider>();
            // });
        }

        //Add ConfigrationAndOptions
        private void ConfigureOptionsAndConfigurations(IServiceCollection services)
        {
            services.AddConfiguration(Configuration);
            services.AddOptions();
            // Adds CloudFoundryApplicationOptions and CloudFoundryServicesOptions to service container
            services.ConfigureCloudFoundryOptions(Configuration);
            // Adds ConfigServerClientOptions to service container
            services.ConfigureConfigServerClientOptions(Configuration);
            //services.Configure<GoogleUpdaterOptions>(Configuration.GetSection("GoogleUpdaterOptions"));
            //services.Configure<RabbitMqOptions>(Configuration.GetSection("RabbitMqOptions"));
        }

        //Dependency Injection for applciation services
        protected virtual void ConfigApplicationServices(IServiceCollection services)
        {
            // services.AddTransient<IGoogleUpdaterRepository, GoogleUpdaterRepository>();
            // services.AddTransient<IDatabaseConnectionFactory, SqlConnectionFactory>();
            // //services.AddTransient<IQueueService, QueueService>();
            // services.AddTransient<INotificationRepository, NotificationRepository>();
            // services.AddTransient<ISsoClientRepository, SsoClientRepository>();
            // services.AddTransient<IWatchRequestRepository, WatchRequestRepository>();
            // services.AddSingleton<IMassTransitService, MassTransitService>();
        }

        private void ConfigureDistributedCache(IServiceCollection services)
        {
            services.AddDistributedRedisCache(option => {
                option.Configuration = "localhost";
            });


            //if (_env == null || _env.IsEnvironment("IntegrationTests"))
            //    services.AddDistributedMemoryCache();
            //else
            //{
            //    services.AddDistributedRedisCache(options =>
            //    {
            //        options.Configuration = "localhost";
            //        options.InstanceName = "CMAAPI";
            //    });
            //}
        }
    }
}
