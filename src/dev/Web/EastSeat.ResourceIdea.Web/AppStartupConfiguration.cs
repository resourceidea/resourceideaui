using EastSeat.ResourceIdea.DataStore;
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

