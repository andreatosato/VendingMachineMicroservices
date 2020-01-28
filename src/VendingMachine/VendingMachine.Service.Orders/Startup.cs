using System;
using System.Text;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using VendingMachine.Service.Orders.Application.Validations;
using VendingMachine.Service.Orders.Infrastructure;
using VendingMachine.Service.Orders.Infrastructure.Handlers;
using VendingMachine.Service.Orders.Read;
using VendingMachine.Service.Shared.API;
using VendingMachine.Service.Shared.Authentication;

namespace VendingMachine.Service.Orders
{
    public class Startup
    {
        private readonly IWebHostEnvironment env;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor()
                .AddOrderEntityFramework(Configuration, env)
                .AddOrderHealthChecks(Configuration, env)
                .AddControllers(options =>
                {
                    //Apply Auth filter
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireClaim(VendingMachineClaimTypes.ApiClaim, VendingMachineClaimValues.OrderApi)
                        .Build();
                    options.Filters.Add(new AuthorizeFilter(policy));
                })
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<OrderValidation>();
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false; // Remove default ASP .NET Core Validations
                });

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            })
            .AddVersionedApiExplorer(options => {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = false;
            })
            .AddCustomAuthentication(Configuration)
            .AddOrderInfrastructure()            
            .AddOrderQueries(Configuration.GetConnectionString("OrderDatabase"))
            .AddMediatR(typeof(OrderHandler))
            .AddDistributedMemoryCache()
            .AddGrpcClients(Configuration)
            .AddOrderSwagger(env);

            services.AddGrpc(c => c.EnableDetailedErrors = true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("AnyOrigin");
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
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                if (env.IsProduction())
                {
                    endpoints.MapHealthChecks("/health-data-api", new HealthCheckOptions()
                    {
                        Predicate = _ => true,
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                    });

                    endpoints.MapHealthChecksUI(setupOptions: setup =>
                    {
                        setup.UIPath = "/show-health-ui"; // this is ui path in your browser
                        setup.ApiPath = "/health-ui-api"; // the UI ( spa app )  use this path to get information from the store ( this is NOT the health-data-api path, is internal ui api )
                    });
                }
                endpoints.MapControllers();
            });
        }
    }

    public static class CustomExtensions
    {
        public static IServiceCollection AddOrderSwagger(this IServiceCollection services, IHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
                        .AddSingleton<OpenApiBasicInformation>((serviceProvider) => new OpenApiBasicInformation
                        {
                            Title = "Order API",
                            Description = "Order API for buy products in machine"
                        });
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

        public static IServiceCollection AddOrderHealthChecks(this IServiceCollection services, IConfiguration Configuration, IHostEnvironment env)
        {
            if (env.IsProduction())
            {
                services
                    .AddHealthChecks()
                    .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy())
                    .AddSqlServer(Configuration.GetConnectionString("OrderDatabase"),
                        tags: new[] { "order" },
                        name: "order-db-check");

                services.AddHealthChecksUI(setupSettings: settings =>
                {
                    settings.AddHealthCheckEndpoint("api", $"{Configuration.GetValue<string>("PathBase")}/health-data-api");
                });
            }
            return services;
        }

        public static IServiceCollection AddOrderEntityFramework(this IServiceCollection services, IConfiguration Configuration, IHostEnvironment env)
        {
            // Ogni ambiente avrà la corretta connectionstring in base al file appsettings --> appsettings.Development, appsettings.Production, appsettings.Staging
            if (env.IsDevelopment())
                services.AddOrderEntityFrameworkDev(Configuration.GetConnectionString("OrderDatabase"));
            else
                services.AddOrderEntityFrameworkProd(Configuration.GetConnectionString("OrderDatabase"));
            return services;
        }

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

        public static IServiceCollection AddGrpcClients(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceReference = new ServicesReference();
            configuration.Bind(nameof(ServicesReference), serviceReference);
            services.AddSingleton<ServicesReference>(serviceReference);

            services.AddProductClient();
            services.AddGrpcClient<Products.ServiceCommunications.ProductItems.ProductItemsClient>((serviceProvider, o) =>
            {
                var serviceReference = serviceProvider.GetRequiredService<ServicesReference>();
                o.Address = new Uri(serviceReference.ProductItemsService);
            });

            services.AddGrpcClient<Products.ServiceCommunications.Products.ProductsClient>((serviceProvider, o) =>
            {
                var serviceReference = serviceProvider.GetRequiredService<ServicesReference>();
                o.Address = new Uri(serviceReference.ProductsService);
            });

            services.AddMachineClient();
            services.AddGrpcClient<Machines.ServiceCommunications.MachineItems.MachineItemsClient>((serviceProvider, o) =>
            {
                var serviceReference = serviceProvider.GetRequiredService<ServicesReference>();
                o.Address = new Uri(serviceReference.MachineItemService);
            });

            return services;
        }
    }
}
