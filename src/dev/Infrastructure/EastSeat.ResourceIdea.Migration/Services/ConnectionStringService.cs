namespace EastSeat.ResourceIdea.Migration.Services;

public class ConnectionStringService
{
    public static string GetSourceConnectionString()
    {
        const string environmentVariableKey = "RESOURCEIDEA_MIGRATION_CONNECTION_STRING";
        string connectionString = GetUserEnvironmentVariable(environmentVariableKey);

        return connectionString;
    }

    public static string GetDestinationConnectionString()
    {
        const string environmentVariableKey = "RESOURCEIDEA_CONNECTION_STRING";
        string connectionString = GetUserEnvironmentVariable(environmentVariableKey);

        return connectionString;
    }

    private static string GetUserEnvironmentVariable(string environmentVariableKey)
    {
        string? value = Environment.GetEnvironmentVariable(environmentVariableKey, EnvironmentVariableTarget.User);
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ApplicationException($"Environment variable '{environmentVariableKey}' is not set.");
        }

        return value.Trim();
    }
}
