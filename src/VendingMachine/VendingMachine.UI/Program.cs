using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using VendingMachine.Service.Gateway.RefitModels.Auth;
using Microsoft.Extensions.Configuration;
using Blazor.Extensions.Storage;
using AspNetMonsters.Blazor.Geolocation;
using VendingMachine.UI.Authentication;

namespace VendingMachine.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.Services.AddServices();
            builder.RootComponents.Add<App>("app");

            await builder.Build().RunAsync();
        }

        
    }

    public static class Startup
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            //var serviceReference = new ServicesReference();
            //configuration.Bind(nameof(ServicesReference), serviceReference);
            //services.AddSingleton<ServicesReference>(serviceReference);

            services.AddSingleton<ServicesReference>((sp ) => new ServicesReference { 
                AuthService = "https://localhost:44330/",
                GatewayBackendService = "https://localhost:4444/"
            });

            services.AddSingleton<ILoginStore, LoginStore>();
            services.AddSingleton<IAccessTokenReader, AccessTokenReader>();

            //https://github.com/AspNetMonsters/Blazor.Geolocation
            services.AddSingleton<LocationService>();

            //https://github.com/BlazorExtensions/Storage
            services.AddStorage();

            services.AddRefitClients();
            return services;
        }
    }
}
