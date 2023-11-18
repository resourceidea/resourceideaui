﻿using EastSeat.ResourceIdea.Application;
using EastSeat.ResourceIdea.Persistence;
using EastSeat.ResourceIdea.Persistence.Services;
using EastSeat.ResourceIdea.Web.Data;
using EastSeat.ResourceIdea.Web.Services;

using Microsoft.AspNetCore.Components.Authorization;

namespace EastSeat.ResourceIdea.Web;

public static class StartupExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddApplicationServices();
        builder.Services.AddWebPersistentServices(builder.Configuration);

        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        //builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>(); stock scaffolding

        builder.Services.AddScoped<ResourceIdeaAuthenticationService>();
        builder.Services.AddScoped<ResourceIdeaAuthenticationStateProvider>();
        builder.Services.AddScoped<WebUserService>();
        builder.Services.AddScoped<AuthenticationStateProvider>(
            authenticationStateProvider => authenticationStateProvider.GetRequiredService<ResourceIdeaAuthenticationStateProvider>());
        builder.Services.AddAuthorizationCore();
    }
}
