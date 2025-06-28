// ----------------------------------------------------------------------------------
// File: TelemetryExtensions.cs
// Path: src/dev/Web/EastSeat.ResourceIdea.Web/Extensions/TelemetryExtensions.cs
// Description: Extension methods for configuring OpenTelemetry and Azure Application Insights.
// ----------------------------------------------------------------------------------

using Azure.Monitor.OpenTelemetry.AspNetCore;
using System.Diagnostics;

namespace EastSeat.ResourceIdea.Web.Extensions;

/// <summary>
/// Extension methods for configuring telemetry services.
/// </summary>
public static class TelemetryExtensions
{
    /// <summary>
    /// ActivitySource for ResourceIdea Application telemetry.
    /// </summary>
    public static readonly ActivitySource ApplicationActivitySource = new("EastSeat.ResourceIdea.Application");

    /// <summary>
    /// ActivitySource for ResourceIdea DataStore telemetry.
    /// </summary>
    public static readonly ActivitySource DataStoreActivitySource = new("EastSeat.ResourceIdea.DataStore");

    /// <summary>
    /// ActivitySource for ResourceIdea Web telemetry.
    /// </summary>
    public static readonly ActivitySource WebActivitySource = new("EastSeat.ResourceIdea.Web");

    /// <summary>
    /// Adds Azure Monitor OpenTelemetry configuration to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddResourceIdeaTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        // Get the connection string from multiple possible sources
        var connectionString = configuration.GetConnectionString("ApplicationInsights")
                              ?? configuration["ApplicationInsights:ConnectionString"]
                              ?? Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING");

        // Only configure telemetry if connection string is provided
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            services.AddOpenTelemetry()
                .UseAzureMonitor(options =>
                {
                    options.ConnectionString = connectionString;
                })
                .WithTracing(tracing =>
                {
                    tracing
                        .AddSource(ApplicationActivitySource.Name)
                        .AddSource(DataStoreActivitySource.Name)
                        .AddSource(WebActivitySource.Name);
                });
        }

        return services;
    }
}