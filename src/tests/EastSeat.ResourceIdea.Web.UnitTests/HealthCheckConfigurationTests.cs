using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xunit;

namespace EastSeat.ResourceIdea.Web.UnitTests;

/// <summary>
/// Tests for health check configuration validation
/// </summary>
public class HealthCheckConfigurationTests
{
    [Fact]
    public void HealthChecks_ShouldBeRegisteredInServices()
    {
        // Arrange
        var services = new ServiceCollection();
        
        // Act - Apply the same configuration as in Program.cs
        services.AddHealthChecks();
        
        // Assert - Verify that health check services are registered
        var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(HealthCheckService));
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void HealthCheckService_ShouldBeAvailableAfterRegistration()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging(); // Required dependency
        services.AddOptions(); // Required dependency
        services.AddHealthChecks();
        
        // Act
        var serviceProvider = services.BuildServiceProvider();
        var healthCheckService = serviceProvider.GetService<HealthCheckService>();
        
        // Assert
        Assert.NotNull(healthCheckService);
    }
}