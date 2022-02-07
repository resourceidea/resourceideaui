namespace ResourceIdea.Infrastructure.Environment;

public static class EnvironmentConfiguration
{
    public static void ConfigureEnvironmentVariables()
    {
        var appDirectory = Directory.GetCurrentDirectory();
        var dotEnvPath = Path.Combine(appDirectory, ".env");

        if (!File.Exists(dotEnvPath)) return;
        foreach (var envConfiguration in File.ReadLines(dotEnvPath))
        {
            if (envConfiguration is null || envConfiguration.StartsWith("#"))
            {
                continue;
            }

            var envConfigurationSplit = envConfiguration.Split("=");
            var envConfigurationKey = envConfigurationSplit[0];
            var envConfigurationValue = envConfigurationSplit[1];

            System.Environment.SetEnvironmentVariable(envConfigurationKey, envConfigurationValue);
        }
    }

    public static (string? username, string? email, string? password, string? firstname, string? lastname) GetAdminUserCredentials()
    {
        var username = System.Environment.GetEnvironmentVariable("RESOURCEIDEA_ADMIN_USERNAME");
        var email = System.Environment.GetEnvironmentVariable("RESOURCEIDEA_ADMIN_EMAIL");
        var password = System.Environment.GetEnvironmentVariable("RESOURCEIDEA_ADMIN_PASSWORD");
        var firstname = System.Environment.GetEnvironmentVariable("RESOURCEIDEA_ADMIN_FIRSTNAME");
        var lastname = System.Environment.GetEnvironmentVariable("RESOURCEIDEA_ADMIN_LASTNAME");
        
        return (username, email, password, firstname, lastname);
    }

    public static string? GetAdminRole()
    {
        return System.Environment.GetEnvironmentVariable("RESOURCEIDEA_ADMIN_ROLE");
    }
}