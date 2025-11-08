using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using EastSeat.ResourceIdea.Client.Services;

namespace EastSeat.ResourceIdea.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");


        var apiBase = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:5001"; // default dev server
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBase) });

        builder.Services.AddScoped<TokenStorage>();
        builder.Services.AddScoped<AuthService>();

        await builder.Build().RunAsync();
    }
}
