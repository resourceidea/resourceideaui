using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ResourceIdeaUI.Web.Services;

namespace ResourceIdeaUI.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services
                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<ILocalStorageService, LocalStorageService>()
                .AddScoped<IHttpService, HttpService>()
                .AddScoped<IDepartmentService, DepartmentService>()
                .AddScoped<NewDepartmentNotifierService>();

            builder.Services
                .AddHttpClient<IHttpService, HttpService>(client => client.BaseAddress = new Uri("http://127.0.0.1:8000/"));

            var host = builder.Build();

            var authenticationService = host.Services.GetRequiredService<IAuthenticationService>();
            await authenticationService.Initialize();

            await host.RunAsync();
        }
    }
}
