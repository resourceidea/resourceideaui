using EastSeat.ResourceIdea.DataStore.Configuration.DatabaseStartup;
using EastSeat.ResourceIdea.DataStore.Wrappers;
using Microsoft.Extensions.Configuration;
using Moq;

namespace EastSeat.ResourceIdea.DataStore.UnitTests;

public class DataStoreSetupTests
{
    [Fact]
    public void ConfigurationWrapper_GetDatabaseStartupTasksConfig_WithEnabledConfig_ReturnsCorrectConfig()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            { "DatabaseStartupTasks:Enabled", "true" },
            { "DatabaseStartupTasks:Tasks:0:Enabled", "true" },
            { "DatabaseStartupTasks:Tasks:0:Type", "EastSeat.ResourceIdea.Web.StartupTasks.ApplyMigrations" }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        var wrapper = new ConfigurationWrapper(configuration);

        // Act
        var result = wrapper.GetDatabaseStartupTasksConfig();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Enabled);
        Assert.Single(result.Tasks);
        Assert.True(result.Tasks[0].Enabled);
        Assert.Equal("EastSeat.ResourceIdea.Web.StartupTasks.ApplyMigrations", result.Tasks[0].Type);
    }

    [Fact]
    public void ConfigurationWrapper_GetDatabaseStartupTasksConfig_WithDisabledConfig_ReturnsCorrectConfig()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            { "DatabaseStartupTasks:Enabled", "false" }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        var wrapper = new ConfigurationWrapper(configuration);

        // Act
        var result = wrapper.GetDatabaseStartupTasksConfig();

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Enabled);
    }

    [Fact]
    public void ConfigurationWrapper_GetDatabaseStartupTasksConfig_WithMissingConfig_ReturnsEmptyConfig()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();
        var wrapper = new ConfigurationWrapper(configuration);

        // Act
        var result = wrapper.GetDatabaseStartupTasksConfig();

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Enabled); // Default value
    }
}
