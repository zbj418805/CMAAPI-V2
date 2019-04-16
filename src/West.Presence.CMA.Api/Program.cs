using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pivotal.Extensions.Configuration.ConfigServer;
using Serilog;
using Serilog.Formatting.Json;
using Steeltoe.Extensions.Logging;

namespace West.Presence.CMA.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Initializes a new instance of the WebHostBuilder class with pre-configured defaults.
        /// https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.webhost.createdefaultbuilder?view=aspnetcore-2.0
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                //.UseCloudFoundryHosting()
                .ConfigureAppConfiguration((hostingContext, config)=>{
                    config.Sources.Clear();
                    var env = hostingContext.HostingEnvironment;
                    config.AddYamlFile("appsettings.yml", optional: true, reloadOnChange: true)
			               .AddYamlFile($"appsettings.{env.EnvironmentName}.yml", optional: true, reloadOnChange: true);

                    if (env.IsDevelopment())
		            {
			             var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
			             if (appAssembly != null)
			             {
				            // JSON format
				            config.AddUserSecrets(appAssembly, optional: true);
			            }
		            }

                    config.AddEnvironmentVariables();
                    //config.AddConfigServer(env);
                    //config.AddEnvironmentVariables();
                })
                .UseStartup<Startup>()
                .ConfigureLogging((hostingContext, logging)=> {
                    //var loggingOptions = hostingContext.Configuration.GetSection("LoggingOptions").Get<LoggingOptions>();

                    var loggerConfiguration = new Serilog.LoggerConfiguration()
                        .ReadFrom.ConfigurationSection(hostingContext.Configuration.GetSection("LoggingLevels"))
                        .Enrich.FromLogContext()
                        .Enrich.WithProperty("ApplicationName", "CMAAPI") //Utility.ApplicationName()
                        .Enrich.WithMachineName();

                    // check to see if deployed to PCF
                    var isPcf =  false; //Core.Utilities.Utility.IsPcf();

                    if (isPcf)
                        loggerConfiguration.WriteTo.Console(new JsonFormatter(renderMessage: true));
                    else
                        loggerConfiguration.WriteTo.ColoredConsole();
                    
                });
        }
    }
}
