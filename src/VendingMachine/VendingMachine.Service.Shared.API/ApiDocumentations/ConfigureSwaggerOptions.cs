using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace VendingMachine.Service.Shared.API
{
    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;
        readonly OpenApiBasicInformation openApiBasicInformation;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, OpenApiBasicInformation openApiBasicInformation)
        {
            this.provider = provider;
            this.openApiBasicInformation = openApiBasicInformation;
        }

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, openApiBasicInformation));
            }
        }

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description, OpenApiBasicInformation openApiBasicInformation)
        {
            var info = new OpenApiInfo()
            {
                Title = openApiBasicInformation.Title,
                Version = description.ApiVersion.ToString(),
                Description = openApiBasicInformation.Description,
                Contact = new OpenApiContact() { Name = "Andrea Tosato", Email = "andrea.tosato@4ward.it" },
                License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }

    public class OpenApiBasicInformation
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
