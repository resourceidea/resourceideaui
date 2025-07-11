// ----------------------------------------------------------------------------------
// File: TelemetryExtensionsTests.cs
// Path: src/tests/EastSeat.ResourceIdea.Web.UnitTests/Extensions/TelemetryExtensionsTests.cs
// Description: Unit tests for TelemetryExtensions.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Web.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Xunit;

namespace EastSeat.ResourceIdea.Web.UnitTests.Extensions;

public class TelemetryExtensionsTests
{
    [Fact]
    public void AddResourceIdeaTelemetry_WithoutConnectionString_DoesNotThrow()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        // Act & Assert
        var exception = Record.Exception(() => services.AddResourceIdeaTelemetry(configuration));
        Assert.Null(exception);
    }

    [Fact]
    public void AddResourceIdeaTelemetry_WithConnectionString_ConfiguresOpenTelemetry()
    {
        // Arrange
        var services = new ServiceCollection();
        var configurationData = new Dictionary<string, string?>
        {
            { "ApplicationInsights:ConnectionString", "InstrumentationKey=test-key;IngestionEndpoint=https://test.com/" }
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configurationData)
            .Build();

        // Act
        services.AddResourceIdeaTelemetry(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider);
    }

    [Fact]
    public void ApplicationActivitySource_IsCreated()
    {
        // Act & Assert
        Assert.NotNull(TelemetryExtensions.ApplicationActivitySource);
        Assert.Equal("EastSeat.ResourceIdea.Application", TelemetryExtensions.ApplicationActivitySource.Name);
    }

    [Fact]
    public void DataStoreActivitySource_IsCreated()
    {
        // Act & Assert
        Assert.NotNull(TelemetryExtensions.DataStoreActivitySource);
        Assert.Equal("EastSeat.ResourceIdea.DataStore", TelemetryExtensions.DataStoreActivitySource.Name);
    }

    [Fact]
    public void WebActivitySource_IsCreated()
    {
        // Act & Assert
        Assert.NotNull(TelemetryExtensions.WebActivitySource);
        Assert.Equal("EastSeat.ResourceIdea.Web", TelemetryExtensions.WebActivitySource.Name);
    }

    [Theory]
    [InlineData("ConnectionStrings:ApplicationInsights")]
    [InlineData("ApplicationInsights:ConnectionString")]
    public void AddResourceIdeaTelemetry_ReadsConnectionStringFromConfiguration(string configKey)
    {
        // Arrange
        var services = new ServiceCollection();
        var testConnectionString = "InstrumentationKey=test-key;IngestionEndpoint=https://test.com/";
        var configurationData = new Dictionary<string, string?>
        {
            { configKey, testConnectionString }
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configurationData)
            .Build();

        // Act
        var exception = Record.Exception(() => services.AddResourceIdeaTelemetry(configuration));

        // Assert
        Assert.Null(exception);
    }
}