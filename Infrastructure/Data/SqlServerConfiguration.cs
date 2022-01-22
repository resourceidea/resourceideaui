namespace ResourceIdea.Infrastructure.Data;

public static class SqlServerConfiguration
{
    public static void AddSqlServerService(this IServiceCollection services)
    {
        var connectionString = GetConnectionString();
        ArgumentNullException.ThrowIfNull(nameof(connectionString));
        services.AddDbContext<ResourceIdeaDBContext>(options => options.UseSqlServer(connectionString!));
    }

    public static string? GetConnectionString()
    {
        var dbServer = System.Environment.GetEnvironmentVariable("RESOURCEIDEA_DB_SERVER");
        var dbName = System.Environment.GetEnvironmentVariable("RESOURCEIDEA_DB_NAME");
        var dbUser = System.Environment.GetEnvironmentVariable("RESOURCEIDEA_DB_USER");
        var dbPassword = System.Environment.GetEnvironmentVariable("RESOURCEIDEA_DB_PASSWORD");

        if (dbServer is null || dbName is null || dbUser is null || dbPassword is null)
        {
            return null;
        }

        return $"Server={dbServer};Database={dbName};User Id={dbUser};Password={dbPassword};";
    }
}