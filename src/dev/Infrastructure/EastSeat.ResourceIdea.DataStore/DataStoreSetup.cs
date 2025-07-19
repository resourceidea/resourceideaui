using EastSeat.ResourceIdea.Application.Features.Clients.Contracts;
using EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Contracts;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Contracts;
using EastSeat.ResourceIdea.Application.Features.Tenants.Contracts;
using EastSeat.ResourceIdea.Application.Features.WorkItems.Contracts;
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

    private static readonly string[] _systemRoles = { "Owner", "Administrator", "General User" };
    private static readonly string[] _backendRoles = { "Developer", "Support" };

    /// <summary>
    /// Add the ResourceIdeaDBContext to the service collection
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="sqlServerConnectionString">Sql Server connection string.</param>
    public static IServiceCollection AddDataAccessServices(this IServiceCollection services, string sqlServerConnectionString)
    {
        services.AddTransient<IClientsService, ClientsService>();
        services.AddTransient<IEngagementsService, EngagementsService>();
        services.AddTransient<ISubscriptionsService, SubscriptionsService>();
        services.AddTransient<ISubscriptionServicesService, SubscriptionServicesService>();
        services.AddTransient<ITenantsService, TenantsService>();
        services.AddTransient<IWorkItemsService, WorkItemsService>();

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
                    // CreateSystemRolesTaskType => new Func<ResourceIdeaDBContext, Task>(CreateSystemRolesAsync),
                    // CreateSystemRolesClaimsTaskType => new Func<ResourceIdeaDBContext, Task>(CreateSystemRolesClaimsAsync),
                    _ => new Func<ResourceIdeaDBContext, Task>(LogUnknownStartupTaskType)
                })
                .Select(startupTask => startupTask(dbContext));

            await Task.WhenAll(tasks);
        }

        return app;
    }

    private static Task LogUnknownStartupTaskType(ResourceIdeaDBContext _)
    {
        // TODO: Log unknown startup task type.
        return Task.CompletedTask;
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

    private static List<SubscriptionService> RemoveExistingServiceNames(
        List<SubscriptionService> subscriptionServices,
        List<string> existingServiceNames)
    {
        subscriptionServices = subscriptionServices
            .Where(s => !existingServiceNames.Contains(s.Name.ToLower()))
            .ToList();
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

    // private static async Task CreateSystemRolesAsync(ResourceIdeaDBContext dbContext)
    // {
    //     if (dbContext?.ApplicationRoles == null)
    //     {
    //         return;
    //     }

    //     List<ApplicationRole> rolesToCreate = _systemRoles.Select(role => new ApplicationRole
    //     {
    //         DepartmentId = Guid.NewGuid().ToString(),
    //         Name = role,
    //         NormalizedName = role.ToUpper(),
    //         IsBackendRole = true,
    //         TenantId = TenantId.Empty
    //     }).ToList();

    //     var existingRoles = await GetExistingRoles(dbContext);
    //     rolesToCreate = RemoveExistingRoles(rolesToCreate, existingRoles);
    //     if (rolesToCreate.Count == 0)
    //     {
    //         return;
    //     }

    //     dbContext.ApplicationRoles.AddRange(rolesToCreate);
    //     await dbContext.SaveChangesAsync();
    // }

    // private static List<ApplicationRole> RemoveExistingRoles(List<ApplicationRole> subscriptionServices, List<string> existingServiceNames)
    // {
    //     subscriptionServices = subscriptionServices.Where(s => !existingServiceNames.Contains(s.Name!.ToLower())).ToList();
    //     return subscriptionServices;
    // }

    // private static async Task<List<string>> GetExistingRoles(ResourceIdeaDBContext dbContext)
    // {
    //     if (dbContext?.ApplicationRoles == null)
    //     {
    //         return [];
    //     }

    //     return await dbContext.ApplicationRoles.Where(s => !string.IsNullOrEmpty(s.Name)).Select(s => s.Name!.ToLower()).ToListAsync();
    // }

    // private static async Task CreateSystemRolesClaimsAsync(ResourceIdeaDBContext dbContext)
    // {
    //     if (dbContext?.ApplicationRoles is null || dbContext?.RoleClaims is null)
    //     {
    //         return;
    //     }

    //     var roleClaims = new Dictionary<string, List<string>>
    //     {
    //         { "Owner", new List<string> { "Permission.AccessAll", "Permission.ManageUsers" } },
    //         { "Administrator", new List<string> { "Permission.ManageUsers", "Permission.ViewReports" } },
    //         { "General User", new List<string> { "Permission.AccessBasic" } }
    //     };

    //     var roles = await dbContext.ApplicationRoles
    //         .Where(r => _systemRoles.Contains(r.Name))
    //         .ToListAsync();

    //     foreach (var role in roles)
    //     {
    //         var existingClaims = await dbContext.RoleClaims
    //             .Where(rc => rc.RoleId == role.DepartmentId)
    //             .Select(rc => new { rc.ClaimType, rc.ClaimValue })
    //             .ToListAsync();

    //         if (roleClaims.TryGetValue(role.Name!, out var claims))
    //         {
    //             foreach (var claimValue in claims)
    //             {
    //                 const string claimType = "Permission";

    //                 if (!existingClaims.Any(ec => ec.ClaimType == claimType && ec.ClaimValue == claimValue))
    //                 {
    //                     var roleClaim = new IdentityRoleClaim<string>
    //                     {
    //                         RoleId = role.DepartmentId!,
    //                         ClaimType = claimType,
    //                         ClaimValue = claimValue
    //                     };
    //                     dbContext.RoleClaims.Add(roleClaim);
    //                 }
    //             }
    //         }
    //     }

    //     await dbContext.SaveChangesAsync();
    // }
}
