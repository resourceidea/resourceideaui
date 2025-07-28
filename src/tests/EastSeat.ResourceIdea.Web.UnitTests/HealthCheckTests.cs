using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Xunit;

namespace EastSeat.ResourceIdea.Web.UnitTests;

public class HealthCheckTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public HealthCheckTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        
        // Set required environment variable for database connection
        Environment.SetEnvironmentVariable("RESOURCEIDEA_CONNECTION_STRING", 
            "Server=(localdb)\\mssqllocaldb;Database=TestResourceIdea;Trusted_Connection=true;");
    }

    [Fact]
    public async Task HealthCheck_ShouldReturnOk_WhenApplicationIsHealthy()
    {
        // Arrange
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Replace health checks with simple test checks
                services.Configure<HealthCheckServiceOptions>(options =>
                {
                    options.Registrations.Clear();
                    options.Registrations.Add(new HealthCheckRegistration(
                        "test",
                        provider => new TestHealthCheck(HealthStatus.Healthy),
                        HealthStatus.Unhealthy,
                        null,
                        TimeSpan.FromSeconds(30)));
                });
            });
        }).CreateClient();

        // Act
        var response = await client.GetAsync("/health");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task HealthCheck_ShouldReturnServiceUnavailable_WhenApplicationIsUnhealthy()
    {
        // Arrange
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Configure health check to return unhealthy status
                services.Configure<HealthCheckServiceOptions>(options =>
                {
                    options.Registrations.Clear();
                    options.Registrations.Add(new HealthCheckRegistration(
                        "test",
                        provider => new TestHealthCheck(HealthStatus.Unhealthy),
                        HealthStatus.Unhealthy,
                        null,
                        TimeSpan.FromSeconds(30)));
                });
            });
        }).CreateClient();

        // Act
        var response = await client.GetAsync("/health");

        // Assert
        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
    }

    [Fact]
    public async Task HealthCheck_ShouldReturnHealthyContent_WhenApplicationIsHealthy()
    {
        // Arrange
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.Configure<HealthCheckServiceOptions>(options =>
                {
                    options.Registrations.Clear();
                    options.Registrations.Add(new HealthCheckRegistration(
                        "test",
                        provider => new TestHealthCheck(HealthStatus.Healthy),
                        HealthStatus.Unhealthy,
                        null,
                        TimeSpan.FromSeconds(30)));
                });
            });
        }).CreateClient();

        // Act
        var response = await client.GetAsync("/health");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Healthy", content);
    }
}

/// <summary>
/// Test health check implementation for unit testing
/// </summary>
public class TestHealthCheck : IHealthCheck
{
    private readonly HealthStatus _status;

    public TestHealthCheck(HealthStatus status)
    {
        _status = status;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new HealthCheckResult(_status));
    }
}