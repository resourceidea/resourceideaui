# Azure Application Insights OpenTelemetry Setup

This document describes the Azure Application Insights OpenTelemetry setup implemented in the ResourceIdea application.

## Overview

The application is configured to use Azure Application Insights with OpenTelemetry for comprehensive monitoring, logging, and performance tracking. This includes automatic instrumentation for ASP.NET Core, HTTP client, SQL Server operations, and custom telemetry for business operations.

## Configuration

### Application Insights Connection String

The Application Insights connection string can be configured in multiple ways (in order of precedence):

1. **Environment Variable**: `APPLICATIONINSIGHTS_CONNECTION_STRING`
2. **Configuration Section**: `ConnectionStrings:ApplicationInsights`
3. **Configuration Section**: `ApplicationInsights:ConnectionString`

#### appsettings.json Example
```json
{
  "ApplicationInsights": {
    "ConnectionString": "InstrumentationKey=your-key;IngestionEndpoint=https://your-region.in.applicationinsights.azure.com/;LiveEndpoint=https://your-region.livediagnostics.monitor.azure.com/"
  }
}
```

#### Environment Variable Example
```bash
export APPLICATIONINSIGHTS_CONNECTION_STRING="InstrumentationKey=your-key;IngestionEndpoint=https://your-region.in.applicationinsights.azure.com/"
```

## Features

### Automatic Instrumentation

The following automatic instrumentation is included:

- **ASP.NET Core**: HTTP requests, responses, and performance metrics
- **HTTP Client**: Outbound HTTP calls
- **SQL Server**: Database operations and performance
- **Dependencies**: External service calls

### Custom Telemetry Sources

Three custom ActivitySources are configured for application-specific telemetry:

- `EastSeat.ResourceIdea.Application`: Business logic operations
- `EastSeat.ResourceIdea.DataStore`: Data access operations  
- `EastSeat.ResourceIdea.Web`: Web-specific operations

### Example Usage

Here's an example of how custom telemetry is used in a query handler:

```csharp
public class TenantEmployeesQueryHandler
{
    private static readonly ActivitySource ActivitySource = new("EastSeat.ResourceIdea.Application");

    public async Task<ResourceIdeaResponse<PagedListResponse<TenantEmployeeModel>>> Handle(
        TenantEmployeesQuery query,
        CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("TenantEmployeesQuery.Handle");
        activity?.SetTag("tenant.id", query.TenantId.ToString());
        activity?.SetTag("query.pageNumber", query.PageNumber);
        activity?.SetTag("query.pageSize", query.PageSize);

        // ... business logic ...

        activity?.SetTag("operation.result", queryResponse.IsSuccess ? "success" : "failure");
        return result;
    }
}
```

## Deployment Considerations

### Development Environment

- Connection string can be left empty in development if you don't want to send telemetry
- No telemetry will be configured if connection string is not provided

### Production Environment

- Set the `APPLICATIONINSIGHTS_CONNECTION_STRING` environment variable with your Application Insights connection string
- Consider using Azure Key Vault or Azure App Configuration for secure configuration management

### Azure App Service

Azure App Service can automatically configure Application Insights. You can:

1. Enable Application Insights in the Azure portal for your App Service
2. The connection string will be automatically set via environment variable

## Monitoring and Alerting

With this setup, you can monitor:

- **Request Performance**: Response times, throughput, and error rates
- **Dependencies**: Database and external service performance
- **Custom Business Metrics**: Specific operations like employee queries
- **Exceptions**: Detailed error tracking and stack traces
- **Application Map**: Visual representation of service dependencies

## Best Practices

1. **Use Descriptive Activity Names**: Use clear, hierarchical names like `{Service}.{Operation}`
2. **Add Relevant Tags**: Include important context like tenant ID, user ID, operation parameters
3. **Track Success/Failure**: Always include operation result status
4. **Avoid Sensitive Data**: Don't include passwords or personal information in tags
5. **Use Consistent Naming**: Follow a consistent pattern for tag names and values

## Troubleshooting

### No Telemetry Appearing

1. Verify the connection string is correct and accessible
2. Check that the connection string format matches Azure requirements
3. Ensure network connectivity to Azure Application Insights endpoints
4. Check application logs for any OpenTelemetry initialization errors

### Performance Impact

- OpenTelemetry is designed to be low-overhead
- Sampling can be configured to reduce data volume if needed
- Monitor CPU and memory usage after deployment

## Additional Resources

- [Azure Application Insights Documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview)
- [OpenTelemetry .NET Documentation](https://opentelemetry.io/docs/instrumentation/net/)
- [Azure Monitor OpenTelemetry Documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/app/opentelemetry-enable)