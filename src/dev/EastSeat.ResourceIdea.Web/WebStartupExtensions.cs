using EastSeat.ResourceIdea.Application;
using EastSeat.ResourceIdea.Persistence;
using EastSeat.ResourceIdea.Persistence.Models;
using EastSeat.ResourceIdea.Web.Areas.Identity;

using Microsoft.AspNetCore.Components.Authorization;

namespace EastSeat.ResourceIdea.Web;

public static class WebStartupExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddApplicationServices();
        builder.Services.AddWebPersistentServices(builder.Configuration);

        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();
    }
}
