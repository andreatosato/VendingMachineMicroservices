using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using VendingMachine.Service.Products.ServiceCommunications;
using VendingMachine.Service.Shared.API;
using VendingMachine.Service.Shared.Authentication;

namespace VendingMachine.Service.Aggregators.Web.API
{
    public class Startup
    {
        private readonly IHostEnvironment environment;

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            this.environment = environment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IServicesReference>(Configuration);

            services
                .AddCustomAuthentication(Configuration)
                .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
                .AddProductSwagger(environment)
                .AddGrpcServices()
                .AddControllers();                
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (env.IsDevelopment())
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();
                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(
                   options =>
                   {
                       // build a swagger endpoint for each discovered API version
                       foreach (var description in provider.ApiVersionDescriptions)
                       {
                           options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                               description.GroupName.ToUpperInvariant());
                       }
                   });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public static class CustomExceptions
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection(nameof(JwtSettings));
            var jwtSettings = jwtSection.Get<JwtSettings>();
            services.Configure<JwtSettings>(jwtSection);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.RequireAuthenticatedSignIn = true;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudience = jwtSettings.Audience,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(jwtSettings.SecurityKey)),
                    LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters) => DateTime.UtcNow < expires.GetValueOrDefault(),
                };
            });

            return services;
        }

        public static IServiceCollection AddGrpcServices(this IServiceCollection services)
        {
            services.AddGrpcClient<ProductItems.ProductItemsClient>((serviceProvider, o) =>
            {
                var serviceReference = serviceProvider.GetRequiredService<IServicesReference>();
                o.Address = new Uri(serviceReference.ProductItemsService);
            })
            //.AddInterceptor(() => new LoggingInterceptor())
            //.ConfigureChannel(o =>
            //{
            //    o.Credentials = new CustomCredentials();
            //})
            ;

            services.AddGrpcClient<Products.ServiceCommunications.Products.ProductsClient>((serviceProvider, o) =>
            {
                var serviceReference = serviceProvider.GetRequiredService<IServicesReference>();
                o.Address = new Uri(serviceReference.ProductsService);
            });


            services.AddGrpcClient<Machines.ServiceCommunications.MachineItems.MachineItemsClient>((serviceProvider, o) =>
            {
                var serviceReference = serviceProvider.GetRequiredService<IServicesReference>();
                o.Address = new Uri(serviceReference.MachineItemService);
            });

            return services;
        }

        public static IServiceCollection AddProductSwagger(this IServiceCollection services, IHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                services.AddSwaggerGen(c =>
                {
                    c.OperationFilter<SwaggerDefaultValues>();

                    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                        new OpenApiSecurityScheme
                        {
                            In = ParameterLocation.Header,
                            Description = "Insert JWT token with the \"Bearer \" prefix",
                            Name = "Authorization",
                            Type = SecuritySchemeType.ApiKey
                        });
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = JwtBearerDefaults.AuthenticationScheme }
                            },
                            new string[0]
                        }
                    });
                });
            }
            return services;
        }
    }
}
