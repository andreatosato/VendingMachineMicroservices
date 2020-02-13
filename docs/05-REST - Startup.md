# Lo startup applicativo
Il Program.cs questo sconosciuto
```cs
 public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

// Serilog
// https://github.com/serilog/serilog-aspnetcore
public static int Main(string[] args)
{
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(Configuration)
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .CreateLogger();

    try
    {
        Log.Information("Starting web host");
        CreateHostBuilder(args).Build().Run();
        return 0;
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "Host terminated unexpectedly");
        return 1;
    }
    finally
    {
        Log.CloseAndFlush();
    }
}

public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder
            .ConfigureKestrel((ctx, options) =>
            {
                options.Listen(IPAddress.Any, 4010, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                    listenOptions.UseHttps();
                });
            })
            .UseStartup<Startup>()
            .UseSerilog();
        });
```

Il ConfigureService
```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
    {
        builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
    }));
    services.AddHttpContextAccessor()
        .AddMachineEntityFramework(Configuration, env)
        //.AddMachineHealthChecks(Configuration)
        .AddControllers(options =>
        {
            // Add ModelBinderProvider
            options.ModelBinderProviders.Insert(0, new MachineModelBinderProvider(options.InputFormatters));

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
    services.AddCustomAuthentication(Configuration)
        .AddMachineInfrastructure()
        .AddMachineQueries(Configuration.GetConnectionString("MachineDatabase"))
        .AddMediatR(typeof(MachineRequestsHandler))
        .AddDistributedMemoryCache()
        .AddMachineSwagger(env);

    services.AddGrpc(c => c.EnableDetailedErrors = true);
}
```

Il Configure
```cs
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    app.UseCors("MyPolicy");

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
        endpoints.MapGrpcService<ServiceCommunications.Services.MachineItemService>();
    });
}
```

Le estensioni
```cs
public static class CustomExtensions
{
    public static IServiceCollection AddMachineSwagger(this IServiceCollection services, IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
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
        return services;
    }

    public static IServiceCollection AddMachineHealthChecks(this IServiceCollection services, IConfiguration Configuration)
    {
        services
            .AddHealthChecks()
            .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy())
            .AddSqlServer(Configuration.GetConnectionString("MachineDatabase"),
                tags: new[] { "machine" },
                name: "machine-db-check");

        services.AddHealthChecksUI(setupSettings: settings =>
        {
            settings.AddHealthCheckEndpoint("api", $"{Configuration.GetValue<string>("PathBase")}/health-data-api");
        });
        return services;
    }

    public static IServiceCollection AddMachineEntityFramework(this IServiceCollection services, IConfiguration Configuration, IHostEnvironment env)
    {
        // Ogni ambiente avrà la corretta connectionstring in base al file appsettings --> appsettings.Development, appsettings.Production, appsettings.Staging
        if (env.IsDevelopment())
            services.AddMachineEntityFrameworkDev(Configuration.GetConnectionString("MachineDatabase"));
        else
            services.AddMachineEntityFrameworkProd(Configuration.GetConnectionString("MachineDatabase"));
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
```


## Log
Serilog: https://github.com/serilog/serilog-aspnetcore

## Mediatr
https://github.com/jbogard/MediatR/wiki

## HealthCheck
https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks