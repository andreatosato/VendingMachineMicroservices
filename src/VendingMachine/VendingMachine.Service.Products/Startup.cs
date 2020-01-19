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
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using VendingMachine.Service.Products.Application.Validations.Products;
using VendingMachine.Service.Products.Infrastructure.Handlers;
using VendingMachine.Service.Shared.Authentication;

namespace VendingMachine.Service.Products
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
                .AddProductEntityFramework(Configuration, env)
                .AddProductHealthChecks(Configuration, env)
                .AddControllers(options =>
                {
                    // Apply Auth filter
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireClaim(VendingMachineClaimTypes.ApiClaim, VendingMachineClaimValues.ProductApi)
                        .Build();
                    options.Filters.Add(new AuthorizeFilter(policy));
                })
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<ProductValidation>();
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false; // Remove default ASP .NET Core Validations
                });

            services.AddCustomAuthentication(Configuration)
               .AddProductInfrastructure()
               .AddProductQueries(Configuration.GetConnectionString("ProductDatabase"))
               .AddMediatR(typeof(ProductHandler))
               .AddDistributedMemoryCache()
               .AddProductSwagger(env);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API");
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

    public static class CustomExceptions
    {
        public static IServiceCollection AddProductSwagger(this IServiceCollection services, IHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product API", Version = "v1" });
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

        public static IServiceCollection AddProductHealthChecks(this IServiceCollection services, IConfiguration Configuration, IHostEnvironment env)
        {
            if (env.IsProduction())
            {
                services
                               .AddHealthChecks()
                               .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy())
                               .AddSqlServer(Configuration.GetConnectionString("ProductDatabase"),
                                    tags: new[] { "product" },
                                    name: "product-db-check");

                services.AddHealthChecksUI(setupSettings: settings =>
                {
                    settings.AddHealthCheckEndpoint("api", $"{Configuration.GetValue<string>("PathBase")}/health-data-api");
                });
            }
            return services;
        }

        public static IServiceCollection AddProductEntityFramework(this IServiceCollection services, IConfiguration Configuration, IHostEnvironment env)
        {
            // Ogni ambiente avrà la corretta connectionstring in base al file appsettings --> appsettings.Development, appsettings.Production, appsettings.Staging
            if (env.IsDevelopment())
                services.AddProductEntityFrameworkDev(Configuration.GetConnectionString("ProductDatabase"));
            else
                services.AddProductEntityFrameworkProd(Configuration.GetConnectionString("ProductDatabase"));
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
    }
}
