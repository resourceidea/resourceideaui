using EastSeat.ResourceIdea.Application.Features.ApplicationUsers.Contracts;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.DataStore.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EastSeat.ResourceIdea.DataStore;

public static class DataStoreSetup
{
    /// <summary>
    /// Add the ResourceIdeaDBContext to the service collection
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="sqlServerConnectionString">Sql Server connection string.</param>
    public static IServiceCollection AddResourceIdeaDataStore(this IServiceCollection services, string sqlServerConnectionString)
    {
        services.AddDbContext<ResourceIdeaDBContext>(options => options.UseSqlServer(sqlServerConnectionString));

        services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ResourceIdeaDBContext>()
                .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
            options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        }).AddIdentityCookies();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
        });

        services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();

        return services;
    }

    /// <summary>
    /// Configure the application to use the ResourceIdea data store.
    /// </summary>
    /// <param name="app">The application builder.</param>
    public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ResourceIdeaDBContext>();
            dbContext.Database.Migrate();
        }

        return app;
    }
}
