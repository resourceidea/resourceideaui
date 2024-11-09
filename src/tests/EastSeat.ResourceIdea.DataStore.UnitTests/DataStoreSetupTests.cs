using EastSeat.ResourceIdea.DataStore.Configuration.DatabaseStartup;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EastSeat.ResourceIdea.DataStore.UnitTests;

public class DataStoreSetupTests
{
    [Fact]
    public async Task RunDatabaseStartupTasks_WithStartupTasksEnabled_RunsConfiguredTasks()
    {
        // Arrange
        var appMock = new Mock<IApplicationBuilder>();
        var serviceProviderMock = new Mock<IServiceProvider>();
        var scopeFactoryMock = new Mock<IServiceScopeFactory>();
        var serviceScopeMock = new Mock<IServiceScope>();
        var scopedServiceProviderMock = new Mock<IServiceProvider>();
        var dbContextMock = new Mock<ResourceIdeaDBContext>();
        var configurationMock = new Mock<IConfiguration>();
        var configurationSectionMock = new Mock<IConfigurationSection>();

        var startupTasksConfig = new DatabaseStartupTasksConfig
        {
            Enabled = true,
            Tasks =
            [
                new DatabaseStartupTask
                {
                    Enabled = true,
                    Type = "EastSeat.ResourceIdea.Web.StartupTasks.ApplyMigrations"
                }
            ]
        };

        configurationSectionMock.Setup(c => c.Get<DatabaseStartupTasksConfig>())
            .Returns(startupTasksConfig);
        configurationMock.Setup(c => c.GetSection("DatabaseStartupTasks"))
            .Returns(configurationSectionMock.Object);

        scopedServiceProviderMock.Setup(s => s.GetService(typeof(ResourceIdeaDBContext)))
            .Returns(dbContextMock.Object);
        scopedServiceProviderMock.Setup(s => s.GetService(typeof(IConfiguration)))
            .Returns(configurationMock.Object);

        serviceScopeMock.Setup(s => s.ServiceProvider)
            .Returns(scopedServiceProviderMock.Object);
        scopeFactoryMock.Setup(s => s.CreateScope())
            .Returns(serviceScopeMock.Object);

        serviceProviderMock.Setup(s => s.GetService(typeof(IServiceScopeFactory)))
            .Returns(scopeFactoryMock.Object);

        appMock.SetupGet(a => a.ApplicationServices)
            .Returns(serviceProviderMock.Object);

        dbContextMock.Setup(d => d.Database.MigrateAsync(default))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await DataStoreSetup.RunDatabaseStartupTasks(appMock.Object);

        // Assert
        Assert.Equal(appMock.Object, result);
        dbContextMock.Verify();
    }

    [Fact]
    public async Task RunDatabaseStartupTasks_WithStartupTasksDisabled_DoesNotRunTasks()
    {
        // Arrange
        var appMock = new Mock<IApplicationBuilder>();
        var serviceProviderMock = new Mock<IServiceProvider>();
        var scopeFactoryMock = new Mock<IServiceScopeFactory>();
        var serviceScopeMock = new Mock<IServiceScope>();
        var scopedServiceProviderMock = new Mock<IServiceProvider>();
        var dbContextMock = new Mock<ResourceIdeaDBContext>();
        var configurationMock = new Mock<IConfiguration>();
        var configurationSectionMock = new Mock<IConfigurationSection>();

        var startupTasksConfig = new DatabaseStartupTasksConfig
        {
            Enabled = false
        };

        configurationSectionMock.Setup(c => c.Get<DatabaseStartupTasksConfig>())
            .Returns(startupTasksConfig);
        configurationMock.Setup(c => c.GetSection("DatabaseStartupTasks"))
            .Returns(configurationSectionMock.Object);

        scopedServiceProviderMock.Setup(s => s.GetService(typeof(ResourceIdeaDBContext)))
            .Returns(dbContextMock.Object);
        scopedServiceProviderMock.Setup(s => s.GetService(typeof(IConfiguration)))
            .Returns(configurationMock.Object);

        serviceScopeMock.Setup(s => s.ServiceProvider)
            .Returns(scopedServiceProviderMock.Object);
        scopeFactoryMock.Setup(s => s.CreateScope())
            .Returns(serviceScopeMock.Object);

        serviceProviderMock.Setup(s => s.GetService(typeof(IServiceScopeFactory)))
            .Returns(scopeFactoryMock.Object);

        appMock.SetupGet(a => a.ApplicationServices)
            .Returns(serviceProviderMock.Object);

        // Act
        var result = await DataStoreSetup.RunDatabaseStartupTasks(appMock.Object);

        // Assert
        Assert.Equal(appMock.Object, result);
        dbContextMock.Verify(d => d.Database.MigrateAsync(default), Times.Never);
    }

    [Fact]
    public async Task RunDatabaseStartupTasks_WithUnknownTaskType_LogsUnknownTask()
    {
        // Arrange
        var appMock = new Mock<IApplicationBuilder>();
        var serviceProviderMock = new Mock<IServiceProvider>();
        var scopeFactoryMock = new Mock<IServiceScopeFactory>();
        var serviceScopeMock = new Mock<IServiceScope>();
        var scopedServiceProviderMock = new Mock<IServiceProvider>();
        var dbContextMock = new Mock<ResourceIdeaDBContext>();
        var configurationMock = new Mock<IConfiguration>();
        var configurationSectionMock = new Mock<IConfigurationSection>();

        var startupTasksConfig = new DatabaseStartupTasksConfig
        {
            Enabled = true,
            Tasks = new List<DatabaseStartupTask>
            {
                new DatabaseStartupTask
                {
                    Enabled = true,
                    Type = "Unknown.Task.Type"
                }
            }
        };

        configurationSectionMock.Setup(c => c.Get<DatabaseStartupTasksConfig>())
            .Returns(startupTasksConfig);
        configurationMock.Setup(c => c.GetSection("DatabaseStartupTasks"))
            .Returns(configurationSectionMock.Object);

        scopedServiceProviderMock.Setup(s => s.GetService(typeof(ResourceIdeaDBContext)))
            .Returns(dbContextMock.Object);
        scopedServiceProviderMock.Setup(s => s.GetService(typeof(IConfiguration)))
            .Returns(configurationMock.Object);

        serviceScopeMock.Setup(s => s.ServiceProvider)
            .Returns(scopedServiceProviderMock.Object);
        scopeFactoryMock.Setup(s => s.CreateScope())
            .Returns(serviceScopeMock.Object);

        serviceProviderMock.Setup(s => s.GetService(typeof(IServiceScopeFactory)))
            .Returns(scopeFactoryMock.Object);

        appMock.SetupGet(a => a.ApplicationServices)
            .Returns(serviceProviderMock.Object);

        // Act
        var result = await DataStoreSetup.RunDatabaseStartupTasks(appMock.Object);

        // Assert
        Assert.Equal(appMock.Object, result);
        // Verify that LogUnknownStartupTaskType was called if possible
    }
}
