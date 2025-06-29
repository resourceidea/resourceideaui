# EastSeat.ResourceIdea.Migration

A secure, high-performance console application for migrating data between Azure SQL databases. This tool follows Azure best practices including managed identity authentication, retry policies, and comprehensive logging.

## Features

- **Secure Authentication**: Uses Azure managed identity and Key Vault for connection strings
- **Batch Processing**: Configurable batch sizes for optimal performance
- **Retry Logic**: Exponential backoff retry policy for transient failures
- **Comprehensive Logging**: Detailed logging with configurable levels
- **Command Line Interface**: Easy-to-use CLI with multiple migration options
- **Graceful Cancellation**: Proper handling of Ctrl+C interruption

## Prerequisites

- .NET 9.0 runtime
- Azure SQL Database (source and destination)
- Azure Key Vault with connection strings stored as secrets
- Appropriate Azure permissions (see Security section below)

## Configuration

### Azure Key Vault Setup

Store your database connection strings in Azure Key Vault with the following secret names:

- `source-database-connection-string`: Connection string for the source database
- `destination-database-connection-string`: Connection string for the destination database

### Application Configuration

Update `appsettings.json` with your Azure Key Vault URI:

```json
{
  "KeyVault": {
    "VaultUri": "https://your-keyvault.vault.azure.net/"
  },
  "Migration": {
    "BatchSize": 1000,
    "CommandTimeoutSeconds": 300,
    "MaxRetryAttempts": 3,
    "RetryDelaySeconds": 5,
    "EnableDetailedLogging": true
  }
}
```

### Environment Variables

You can override configuration using environment variables with the `MIGRATION_` prefix:

```bash
MIGRATION_Migration__BatchSize=2000
MIGRATION_KeyVault__VaultUri=https://your-keyvault.vault.azure.net/
```

## Usage

### Migrate All Tables

```bash
dotnet run migrate-all
```

### Migrate Specific Table

```bash
dotnet run migrate-table "TableName"
```

### Command Line Options

- `migrate-all`: Migrates all user tables from source to destination database
- `migrate-table <tableName>`: Migrates a specific table by name

## Security

### Authentication

The application uses `DefaultAzureCredential` which supports multiple authentication methods in order of preference:

1. **Managed Identity** (recommended for Azure-hosted applications)
2. **Azure CLI** (for local development)
3. **Visual Studio** / **Visual Studio Code**
4. **Environment Variables** (for CI/CD scenarios)

### Required Azure Permissions

#### Key Vault Permissions

- `Key Vault Secrets User` role or equivalent permissions:
  - `Microsoft.KeyVault/vaults/secrets/getSecret/action`

#### SQL Database Permissions

- **Source Database**: `db_datareader` role minimum
- **Destination Database**: `db_datawriter` and `db_ddladmin` roles

### Connection String Security

- Never store connection strings in code or configuration files
- Use Azure Key Vault for secure secret management
- Enable Key Vault access logging for audit trails
- Rotate connection strings regularly

## Performance Optimization

### Batch Size Configuration

The default batch size is 1,000 records. Adjust based on your data characteristics:

- **Large records (many columns/large data)**: Use smaller batch sizes (500-1,000)
- **Small records**: Use larger batch sizes (2,000-5,000)
- **Memory constraints**: Monitor memory usage and adjust accordingly

### Connection Management

- Connection pooling is handled automatically by SqlConnection
- Command timeouts are configurable (default: 5 minutes)
- Bulk copy operations are optimized for large data transfers

### Monitoring Performance

Enable detailed logging to monitor:

- Records processed per batch
- Processing time per table
- Retry attempts and failures
- Overall migration duration

## Error Handling

### Retry Policy

The application implements exponential backoff retry logic:

- Maximum retry attempts: 3 (configurable)
- Base delay: 5 seconds (configurable)
- Exponential backoff: 2^attempt * base delay

### Supported Error Scenarios

- Transient SQL connection failures
- Timeout exceptions
- Network interruptions
- Resource contention

### Logging

All operations are logged with appropriate levels:

- **Information**: Migration progress and completion
- **Warning**: Retry attempts
- **Error**: Failed operations with details
- **Debug**: Detailed batch processing (when enabled)

## Development

### Building the Project

```bash
dotnet build
```

### Running Tests

```bash
dotnet test
```

### Publishing for Deployment

```bash
dotnet publish -c Release -r win-x64 --self-contained
```

## Troubleshooting

### Common Issues

1. **Authentication Failures**
   - Verify Azure credentials are properly configured
   - Check Key Vault access permissions
   - Ensure the correct tenant and subscription context

2. **Connection String Issues**
   - Verify secret names in Key Vault match expected names
   - Test connection strings manually
   - Check network connectivity to Azure SQL

3. **Performance Issues**
   - Adjust batch size based on data characteristics
   - Monitor memory usage during migration
   - Consider running during off-peak hours

4. **Timeout Errors**
   - Increase command timeout for large tables
   - Check network latency between regions
   - Monitor database performance metrics

### Logging Configuration

For debugging, increase logging verbosity in `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "EastSeat.ResourceIdea.Migration": "Trace"
    }
  }
}
```

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For issues and questions:

1. Check the troubleshooting section above
2. Review application logs for detailed error information
3. Verify Azure resource configurations and permissions
