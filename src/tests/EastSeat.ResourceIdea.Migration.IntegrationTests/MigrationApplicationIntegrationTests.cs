using EastSeat.ResourceIdea.Migration.Configuration;
using EastSeat.ResourceIdea.Migration.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;

namespace EastSeat.ResourceIdea.Migration.IntegrationTests;

/// <summary>
/// Integration tests for the migration application.
/// These tests demonstrate how the migration service integrates with the dependency injection container.
/// Note: These tests use mock implementations since real Azure SQL databases are not available in the test environment.
/// </summary>
public sealed class MigrationApplicationIntegrationTests
{
    /// <summary>
    /// Tests that the dependency injection container can resolve all required services.
    /// </summary>
    [Fact]
    public void ServiceContainer_ShouldResolveAllServices()
    {
        // Arrange
        var host = CreateTestHost();

        // Act & Assert
        using var scope = host.Services.CreateScope();

        var connectionStringService = scope.ServiceProvider.GetService<IConnectionStringService>();
        connectionStringService.Should().NotBeNull();

        var migrationService = scope.ServiceProvider.GetService<IDatabaseMigrationService>();
        migrationService.Should().NotBeNull();

        var migrationOptions = scope.ServiceProvider.GetService<IOptions<MigrationOptions>>();
        migrationOptions.Should().NotBeNull();
        migrationOptions!.Value.Should().NotBeNull();

        var keyVaultOptions = scope.ServiceProvider.GetService<IOptions<KeyVaultOptions>>();
        keyVaultOptions.Should().NotBeNull();
        keyVaultOptions!.Value.Should().NotBeNull();
    }

    /// <summary>
    /// Tests that configuration options are bound correctly.
    /// </summary>
    [Fact]
    public void Configuration_ShouldBindOptionsCorrectly()
    {
        // Arrange
        var host = CreateTestHost();

        // Act
        using var scope = host.Services.CreateScope();
        var migrationOptions = scope.ServiceProvider.GetRequiredService<IOptions<MigrationOptions>>().Value;
        var keyVaultOptions = scope.ServiceProvider.GetRequiredService<IOptions<KeyVaultOptions>>().Value;

        // Assert
        migrationOptions.BatchSize.Should().Be(500); // From test configuration
        migrationOptions.CommandTimeoutSeconds.Should().Be(60);
        migrationOptions.MaxRetryAttempts.Should().Be(2);
        migrationOptions.RetryDelaySeconds.Should().Be(3);
        migrationOptions.EnableDetailedLogging.Should().BeFalse();

        keyVaultOptions.VaultUri.Should().Be("https://test-keyvault.vault.azure.net/");
    }

    /// <summary>
    /// Tests that logging is configured correctly.
    /// </summary>
    [Fact]
    public void Logging_ShouldBeConfiguredCorrectly()
    {
        // Arrange
        var host = CreateTestHost();

        // Act & Assert
        using var scope = host.Services.CreateScope();
        var logger = scope.ServiceProvider.GetService<ILogger<MigrationApplicationIntegrationTests>>();
        logger.Should().NotBeNull();

        // Test that logging works without exceptions
        logger!.LogInformation("Test log message");
    }

    /// <summary>
    /// Creates a test host with the required configuration and services.
    /// </summary>
    /// <returns>Configured test host.</returns>
    private static IHost CreateTestHost()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                // Use in-memory configuration for testing
                var testConfiguration = new Dictionary<string, string?>
                {
                    ["Migration:BatchSize"] = "500",
                    ["Migration:CommandTimeoutSeconds"] = "60",
                    ["Migration:MaxRetryAttempts"] = "2",
                    ["Migration:RetryDelaySeconds"] = "3",
                    ["Migration:EnableDetailedLogging"] = "false",
                    ["KeyVault:VaultUri"] = "https://test-keyvault.vault.azure.net/"
                };

                config.AddInMemoryCollection(testConfiguration);
            })
            .ConfigureServices((context, services) =>
            {
                // Configure options
                services.Configure<MigrationOptions>(
                    context.Configuration.GetSection(MigrationOptions.SectionName));
                services.Configure<KeyVaultOptions>(
                    context.Configuration.GetSection(KeyVaultOptions.SectionName));

                // Register services with mock implementations for testing
                services.AddSingleton<IConnectionStringService, TestConnectionStringService>();
                services.AddSingleton<IDatabaseMigrationService, DatabaseMigrationService>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Information);
            })
            .Build();
    }
}

/// <summary>
/// Test implementation of IConnectionStringService for integration testing.
/// </summary>
internal sealed class TestConnectionStringService : IConnectionStringService
{
    /// <inheritdoc />
    public Task<string> GetSourceConnectionStringAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult("Server=test-source;Database=TestSource;Integrated Security=true;Encrypt=True;");
    }

    /// <inheritdoc />
    public Task<string> GetDestinationConnectionStringAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult("Server=test-destination;Database=TestDestination;Integrated Security=true;Encrypt=True;");
    }
}
