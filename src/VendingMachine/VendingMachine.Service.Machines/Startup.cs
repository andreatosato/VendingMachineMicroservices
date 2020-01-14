using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.OpenApi.Models;
using VendingMachine.Service.Machines.Infrastructure.Handlers;
using FluentValidation.AspNetCore;
using VendingMachine.Service.Machines.Application.Validations.Coins;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using VendingMachine.Service.Machines.Binders;

namespace VendingMachine.Service.Machines
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
            string machineDatabaseConnectionString = string.Empty;
            if (env.IsDevelopment())
            {
                machineDatabaseConnectionString = "Server=(localdb)\\mssqllocaldb;Database=VendingMachine-Machines;Trusted_Connection=True;MultipleActiveResultSets=true";
                services.AddMachineEntityFrameworkDev(machineDatabaseConnectionString);
            }
            else
            {
                machineDatabaseConnectionString = Configuration.GetConnectionString("ConnectionStrings:MachineDatabase");
                services.AddMachineEntityFrameworkProd(machineDatabaseConnectionString);
            }
            services.AddHttpContextAccessor();
            
            services
               .AddHealthChecks()
               .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy())
               .AddSqlServer(machineDatabaseConnectionString,
                    tags: new[] { "machine" },
                    name: "machine-db-check");

            services.AddHealthChecksUI(setupSettings: settings => 
            {
                settings.AddHealthCheckEndpoint("api", $"{Configuration.GetValue<string>("PathBase")}/health-data-api");
            });

            services.AddCustomAuthentication(Configuration);

            services.AddControllers()
                .AddMvcOptions(options =>
                {
                    IHttpRequestStreamReaderFactory readerFactory = services.BuildServiceProvider().GetRequiredService<IHttpRequestStreamReaderFactory>();
                    options.ModelBinderProviders.Insert(0, new MachineModelBinderProvider(options.InputFormatters, readerFactory));
                })
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<AddCoinsValidation>();
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false; // Remove default ASP .NET Core Validations
                });


            if (env.IsDevelopment())
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Machine API", Version = "v1" });
                    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows()
                        {
                            Password = new OpenApiOAuthFlow()
                            {
                                AuthorizationUrl = new Uri($"{Configuration.GetValue<string>("IdentityUrlExternal")}/connect/authorize"),
                                TokenUrl = new Uri($"{Configuration.GetValue<string>("IdentityUrlExternal")}/connect/token"),
                                RefreshUrl = new Uri($"{Configuration.GetValue<string>("IdentityUrlExternal")}/connect/refresh"),
                                Scopes = new Dictionary<string, string>()
                                {
                                    { "machines", "Machine API" }
                                }
                            }
                        }
                    });
                });
            }

            services.AddInfrastructure()
                    .AddQueries(machineDatabaseConnectionString);
            services.AddMediatR(typeof(MachineRequestsHandler));
            services.AddDistributedMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Machine API");
                });
            }
            
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
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
                
                endpoints.MapControllers();
            });
        }


        
    }

    public static class CustomExceptions
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // prevent from mapping "sub" claim to nameidentifier.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            var identityUrl = configuration.GetValue<string>("IdentityUrl");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = identityUrl;
                options.RequireHttpsMetadata = false;
                options.Audience = "machines";
            });

            return services;
        }
    }
}
