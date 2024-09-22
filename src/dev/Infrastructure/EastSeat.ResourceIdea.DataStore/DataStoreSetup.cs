using EastSeat.ResourceIdea.Application.Features.Clients.Contracts;
using EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Contracts;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Contracts;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Contracts;
using EastSeat.ResourceIdea.Application.Features.Tenants.Contracts;
using EastSeat.ResourceIdea.DataStore.Configuration.DatabaseStartup;
using EastSeat.ResourceIdea.DataStore.Services;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.ValueObjects;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EastSeat.ResourceIdea.DataStore;

public static class DataStoreSetup
{
    private const string ApplyMigrationTaskType = "EastSeat.ResourceIdea.Web.StartupTasks.ApplyMigrations";
    private const string CreateSubscriptionServicesTaskType = "EastSeat.ResourceIdea.Web.StartupTasks.CreateSubscriptionServices";
    private const string CreateSystemRolesTaskType = "EastSeat.ResourceIdea.Web.StartupTasks.CreateSystemRoles";
    private const string CreateSystemRolesClaimsTaskType = "EastSeat.ResourceIdea.Web.StartupTasks.CreateSystemRolesClaims";

    /// <summary>
    /// Add the ResourceIdeaDBContext to the service collection
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="sqlServerConnectionString">Sql Server connection string.</param>
    public static IServiceCollection AddDataStoreServices(this IServiceCollection services, string sqlServerConnectionString)
    {
        services.AddTransient<IClientsService, ClientsService>();
        services.AddTransient<IEngagementsService, EngagementsService>();
        services.AddTransient<IEngagementTasksService, EngagementTasksService>();
        services.AddTransient<ISubscriptionsService, SubscriptionsService>();
        services.AddTransient<ISubscriptionServicesService, SubscriptionServicesService>();
        services.AddTransient<ITenantsService, TenantsService>();

        return services;
    }

    /// <summary>
    /// Configure the application to use the ResourceIdea data store.
    /// </summary>
    /// <param name="app">The application builder.</param>
    public static async Task<IApplicationBuilder> RunDatabaseStartupTasks(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ResourceIdeaDBContext>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var startupTasksConfig = configuration.GetSection("DatabaseStartupTasks").Get<DatabaseStartupTasksConfig>();

        if (startupTasksConfig?.Enabled is true)
        {
            var tasks = startupTasksConfig.Tasks
                .Where(task => task.Enabled)
                .Select(task => task.Type switch
                {
                    ApplyMigrationTaskType => new Func<ResourceIdeaDBContext, Task>(context => context.Database.MigrateAsync()),
                    CreateSubscriptionServicesTaskType => new Func<ResourceIdeaDBContext, Task>(CreateSubscriptionServicesAsync),
                    CreateSystemRolesTaskType => new Func<ResourceIdeaDBContext, Task>(CreateSystemRolesAsync),
                    CreateSystemRolesClaimsTaskType => new Func<ResourceIdeaDBContext, Task>(CreateSystemRolesClaimsAsync),
                    _ => throw new InvalidOperationException($"Unknown startup task type: {task.Type}")
                })
                .Select(startupTask => startupTask(dbContext));

            await Task.WhenAll(tasks);
        }

        return app;
    }

    private static async Task CreateSubscriptionServicesAsync(ResourceIdeaDBContext dbContext)
    {
        if (dbContext?.SubscriptionServices is null)
        {
            return;
        }

        List<SubscriptionService> serviceNamesToCreate =
        [
            new SubscriptionService { Id = SubscriptionServiceId.Create(Guid.NewGuid()), Name = "Resource Planner" }
        ];

        var existingServiceNames = await GetExistingServiceNames(dbContext);
        serviceNamesToCreate = RemoveExistingServiceNames(serviceNamesToCreate, existingServiceNames);
        if (serviceNamesToCreate.Count == 0)
        {
            return;
        }

        dbContext.SubscriptionServices.AddRange(serviceNamesToCreate);
        await dbContext.SaveChangesAsync();
    }

    private static List<SubscriptionService> RemoveExistingServiceNames(List<SubscriptionService> subscriptionServices, List<string> existingServiceNames)
    {
        subscriptionServices = subscriptionServices.Where(s => !existingServiceNames.Contains(s.Name.ToLower())).ToList();
        return subscriptionServices;
    }

    private static async Task<List<string>> GetExistingServiceNames(ResourceIdeaDBContext dbContext)
    {
        if (dbContext?.SubscriptionServices is null)
        {
            return [];
        }

        return await dbContext.SubscriptionServices.Select(s => s.Name.ToLower()).ToListAsync();
    }

    private static Task CreateSystemRolesAsync(ResourceIdeaDBContext dbContext)
    {
        //TODO: Add logic for CreateSystemRoles
        return Task.CompletedTask;
    }

    private static Task CreateSystemRolesClaimsAsync(ResourceIdeaDBContext dbContext)
    {
        //TODO: Add logic for CreateSystemRolesClaims
        return Task.CompletedTask;
    }
}
