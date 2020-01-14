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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VendingMachine.Service.Machines.Configuration;
using System.Threading.Tasks;
using VendingMachine.Service.Shared.Authentication;

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
            services.AddCors(options =>
            {
                options.AddPolicy("AnyOrigin", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod();
                });
            });
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


            services.AddControllers(options =>
            {
                // Add ModelBinderProvider
                IHttpRequestStreamReaderFactory readerFactory = services.BuildServiceProvider().GetRequiredService<IHttpRequestStreamReaderFactory>();
                options.ModelBinderProviders.Insert(0, new MachineModelBinderProvider(options.InputFormatters, readerFactory));

                // Apply Auth filter
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(VendingMachineClaimTypes.ApiClaim, VendingMachineClaimValues.MachineApi)
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<AddCoinsValidation>();
                fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false; // Remove default ASP .NET Core Validations
            });

            services.AddCustomAuthentication(Configuration);

            if (env.IsDevelopment())
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Machine API", Version = "v1" });
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

            app.UseCors("AnyOrigin");
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
