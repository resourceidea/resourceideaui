using EastSeat.ResourceIdea.DataStore.Configuration.DatabaseStartup;
using Microsoft.Extensions.Configuration;

namespace EastSeat.ResourceIdea.DataStore.Wrappers;

public class ConfigurationWrapper : IConfigurationWrapper
{
    private readonly IConfiguration _configuration;

    public ConfigurationWrapper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DatabaseStartupTasksConfig GetDatabaseStartupTasksConfig()
    {
        return _configuration.GetSection("DatabaseStartupTasks").Get<DatabaseStartupTasksConfig>() ?? new DatabaseStartupTasksConfig();
    }
}
