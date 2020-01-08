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
            services.AddControllers()
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<AddCoinsValidation>();
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false; // Remove default ASP .NET Core Validations
                });
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

            if (env.IsDevelopment())
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Machine API", Version = "v1" });
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
