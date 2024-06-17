using EastSeat.ResourceIdea.DataStore.Identity.Entities;

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
                .AddEntityFrameworkStores<ResourceIdeaDBContext>();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
        });

        return services;
    }
}
