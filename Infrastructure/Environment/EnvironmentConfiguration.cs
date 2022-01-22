namespace ResourceIdea.Infrastructure.Environment;

public static class EnvironmentConfiguration
{
    public static void ConfigureEnvironmentVariables()
    {
        var appDirectory = Directory.GetCurrentDirectory();
        var dotEnvPath = Path.Combine(appDirectory, ".env");

        if (File.Exists(dotEnvPath))
        {
            foreach (string? envConfiguration in File.ReadLines(dotEnvPath))
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
    }
}