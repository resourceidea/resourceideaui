using EastSeat.ResourceIdea.Application.Extensions;
using EastSeat.ResourceIdea.Application.Features.Authentication.Contracts;
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
using EastSeat.ResourceIdea.DataStore.Wrappers;
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
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddScoped<NotificationService>();
            services.AddScoped<IConfigurationWrapper, ConfigurationWrapper>();
        }

        private static string GetDbContextConnectionString() => GetUserEnvironmentVariable("RESOURCEIDEA_CONNECTION_STRING");

        private static string GetUserEnvironmentVariable(string environmentVariableKey)
        {
            try
            {
                // Try to get from process environment variables first (Azure App Service, Docker, etc.)
                string? value = Environment.GetEnvironmentVariable(environmentVariableKey);
                
                // If not found, try user environment variables (local development)
                if (string.IsNullOrWhiteSpace(value))
                {
                    value = Environment.GetEnvironmentVariable(environmentVariableKey, EnvironmentVariableTarget.User);
                }
                
                string nonEmptyValue = value.ThrowIfNullOrEmptyOrWhiteSpace();
                return nonEmptyValue;
            }
            catch (ArgumentException ex)
            {
                Debug.WriteLine($"Error retrieving environment variable '{environmentVariableKey}': {ex.Message}");
                throw new ResourceIdeaException($"Failed to retrieve environment variable '{environmentVariableKey}'.", ex);
            }
        }
    }
}

