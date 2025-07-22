using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EastSeat.ResourceIdea.DataStore;

/// <summary>
/// Design-time factory for ResourceIdeaDBContext.
/// This factory is used by EF Core tools for migrations and other design-time operations.
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ResourceIdeaDBContext>
{
    public ResourceIdeaDBContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ResourceIdeaDBContext>();

        // Try to get connection string from environment variable first
        string? connectionString = Environment.GetEnvironmentVariable("RESOURCEIDEA_CONNECTION_STRING");

        // If not found, try the user environment variable (for local development)
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = Environment.GetEnvironmentVariable("RESOURCEIDEA_CONNECTION_STRING", EnvironmentVariableTarget.User);
        }

        // If still not found, use a default development connection string
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = "Server=(localdb)\\mssqllocaldb;Database=ResourceIdeaDB;Trusted_Connection=true;MultipleActiveResultSets=true";
        }

        optionsBuilder.UseSqlServer(connectionString);

        return new ResourceIdeaDBContext(optionsBuilder.Options);
    }
}
