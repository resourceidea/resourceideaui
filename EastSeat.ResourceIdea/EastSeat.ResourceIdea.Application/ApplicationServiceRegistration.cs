using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace EastSeat.ResourceIdea.Application;

/// <summary>
/// Application services registration class.
/// </summary>
public static class ApplicationServiceRegistration
{
    /// <summary>
    /// Add application services to the service collection.
    /// </summary>
    /// <param name="services">Services collection.</param>
    /// <returns>Service collection.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}
