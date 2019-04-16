using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;


namespace West.Presence.CMA.Api.Infrastructure
{
	[ExcludeFromCodeCoverage]
	public class SwaggerConfig
	{
		internal static Action<SwaggerGenOptions> SetupSwaggerGenOptions()
		{
			return options =>
			{
				options.SwaggerDoc("v1", new Info
				{
					Version = "v1",
					Title = "Presence GoogleUpdater API"
				});

				// determine base path fo the application
				var basePath = AppContext.BaseDirectory;

				// set the comments path for the Swagger json and ui
				var xmlPath = Path.Combine(basePath, "West.Presence.GoogleUpdater.WebApi.xml");
				options.IncludeXmlComments(xmlPath);

				options.EnableAnnotations();
				options.DocumentFilter<SecurityRequirementsDocumentFilter>();
			};
		}

		internal static Action<SwaggerUIOptions> SetupSwaggerUiOptions()
		{
			return options =>
			{
				options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
			};
		}
	}
}