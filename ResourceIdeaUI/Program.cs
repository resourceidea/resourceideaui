using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ResourceIdeaUI.Services;
using System.Text.Json;

namespace ResourceIdeaUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services
                .AddScoped<ILocalStorageService, LocalStorageService>()
                .AddScoped<IAuthenticationService, AuthenticationService>();

            builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>(client => client.BaseAddress = new Uri("https://dev-resourceideaapi.azurewebsites.net/"));

            var host = builder.Build();

            var authenticationService = host.Services.GetRequiredService<IAuthenticationService>();
            await authenticationService.Initialize();

            await host.RunAsync();
        }
    }
}
