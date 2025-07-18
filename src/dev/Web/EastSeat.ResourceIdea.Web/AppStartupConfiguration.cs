using EastSeat.ResourceIdea.Application.Features.Clients.Contracts;
using EastSeat.ResourceIdea.Application.Features.Departments.Contracts;
using EastSeat.ResourceIdea.Application.Features.Employees.Contracts;
using EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Contracts;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Contracts;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Contracts;
using EastSeat.ResourceIdea.Application.Features.Tenants.Contracts;
using EastSeat.ResourceIdea.Application.Features.WorkItems.Contracts;
using EastSeat.ResourceIdea.DataStore;
using EastSeat.ResourceIdea.DataStore.Services;
using EastSeat.ResourceIdea.Domain.Exceptions;
using EastSeat.ResourceIdea.Web.Services;
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
            services.AddScoped<ISubscriptionServicesService, SubscriptionServicesService>();
            services.AddScoped<ISubscriptionsService, SubscriptionsService>();
            services.AddScoped<IJobPositionService, JobPositionsService>();
            services.AddScoped<IEmployeeService, EmployeesService>();
            services.AddScoped<IWorkItemsService, WorkItemsService>();
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<IWorkItemsService, WorkItemsService>();

            services.AddScoped<NotificationService>();
        }

        private static string GetDbContextConnectionString()
        {
            string sqlServerConnectionString;

            // TODO: Setup getting connection string from Azure App Configuration.
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                var serverInstance = GetUserEnvironmentVariable("RESOURCEIDEA_DB_HOST");
                var database = GetUserEnvironmentVariable("RESOURCEIDEA_DB_NAME");
                var userId = GetUserEnvironmentVariable("RESOURCEIDEA_DB_USER");
                var password = GetUserEnvironmentVariable("RESOURCEIDEA_DB_PASSWORD");

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

        private static string GetUserEnvironmentVariable(string environmentVariableKey)
        {
            string? value = Environment.GetEnvironmentVariable(environmentVariableKey, EnvironmentVariableTarget.User);
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ResourceIdeaException($"Environment variable '{environmentVariableKey}' is not set.");
            }

            return value.Trim();
        }
    }
}

