using EastSeat.ResourceIdea.Application.Features.Clients.Contracts;
using EastSeat.ResourceIdea.Application.Features.Departments.Contracts;
using EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Contracts;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Contracts;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Contracts;
using EastSeat.ResourceIdea.Application.Features.Tenants.Contracts;
using EastSeat.ResourceIdea.DataStore;
using EastSeat.ResourceIdea.DataStore.Services;

using Microsoft.EntityFrameworkCore;

using System.Diagnostics;

namespace EastSeat.ResourceIdea.Web
{
    public static class AppStartupConfiguration
    {
        /// <summary>
        /// Configure the DbContext for the application.
        /// </summary>
        /// <param name="services">Services Collection</param>
        /// <param name="configuration"></param>
        public static void AddResourceIdeaDbContext(this IServiceCollection services)
        {
            string sqlServerConnectionString = GetDbContextConnectionString();
            services.AddDbContext<ResourceIdeaDBContext>(options => options.UseSqlServer(sqlServerConnectionString));
        }

        public static void AddResourceIdeaServices(this IServiceCollection services)
        {
            services.AddScoped<ITenantsService, TenantsService>();
            services.AddScoped<IClientsService, ClientsService>();
            services.AddScoped<IDepartmentsService, DepartmentsService>();
            services.AddScoped<IEngagementsService, EngagementsService>();
            services.AddScoped<IEngagementTasksService, EngagementTasksService>();
            services.AddScoped<ISubscriptionServicesService, SubscriptionServicesService>();
            services.AddScoped<ISubscriptionsService, SubscriptionsService>();
        }

        private static string GetDbContextConnectionString()
        {
            string sqlServerConnectionString;

            // TODO: Setup getting connection string from Azure App Configuration.
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                var serverInstance = Environment.GetEnvironmentVariable("RESOURCEIDEA_DB_HOST");
                var database = Environment.GetEnvironmentVariable("RESOURCEIDEA_DB");
                var userId = Environment.GetEnvironmentVariable("RESOURCEIDEA_DB_USER");
                var password = Environment.GetEnvironmentVariable("RESOURCEIDEA_DB_PASSWORD");

                if (string.IsNullOrWhiteSpace(serverInstance) ||
                    string.IsNullOrWhiteSpace(database) ||
                    string.IsNullOrWhiteSpace(userId) ||
                    string.IsNullOrWhiteSpace(password))
                {
                    throw new InvalidOperationException($"Invalid SQL Server connection string. -- instance: {serverInstance} database: {database} userId: {userId}");
                }

                sqlServerConnectionString = $"Server={serverInstance};Database={database};User Id={userId};Password={password};";
            }
            else
            {
                // TODO: Implement getting connection string from Azure App Configuration.
                sqlServerConnectionString = string.Empty;
            }

            Debug.WriteLine($"ResourceIdea :: SQL Server Connection String: {sqlServerConnectionString}");
            return sqlServerConnectionString;
        }
    }
}

