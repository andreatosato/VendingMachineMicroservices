using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using VendingMachine.Service.Gateway.RefitModels.Auth;
using Microsoft.Extensions.Configuration;
using Blazor.Extensions.Storage;

namespace VendingMachine.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            AddServices(builder.Services, builder.Configuration.Build());
            builder.RootComponents.Add<App>("app");

            await builder.Build().RunAsync();
        }

        public static IServiceCollection AddServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            var serviceReference = new ServicesReference();
            configuration.Bind(nameof(ServicesReference), serviceReference);
            services.AddSingleton<ServicesReference>(serviceReference);

            https://github.com/BlazorExtensions/Storage
            services.AddStorage();

            services.AddRefitClients();
            return services;
        }
    }
}
