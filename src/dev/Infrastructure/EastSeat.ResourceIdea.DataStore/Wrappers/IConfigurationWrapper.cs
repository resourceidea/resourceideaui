using EastSeat.ResourceIdea.DataStore.Configuration.DatabaseStartup;

namespace EastSeat.ResourceIdea.DataStore.Wrappers;

public interface IConfigurationWrapper
{
    DatabaseStartupTasksConfig GetDatabaseStartupTasksConfig();
}
