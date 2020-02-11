using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace VendingMachine.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            //AddServices(builder.Services);
            builder.RootComponents.Add<App>("app");
            await builder.Build().RunAsync();
        }

        public static IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddRefitClients();
            return services;
        }
    }
}
