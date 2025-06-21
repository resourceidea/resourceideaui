using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using EastSeat.ResourceIdea.Migration.Configuration;
using EastSeat.ResourceIdea.Migration.Models;
using Polly;
using System.Data;

namespace EastSeat.ResourceIdea.Migration.Services;

/// <summary>
/// Service for migrating data between Azure SQL databases.
/// </summary>
public sealed class DatabaseMigrationService : IDatabaseMigrationService
{
    private readonly IConnectionStringService _connectionStringService;
    private readonly ILogger<DatabaseMigrationService> _logger;
    private readonly MigrationOptions _migrationOptions;
    private readonly IAsyncPolicy _retryPolicy;    /// <summary>
                                                   /// Initializes a new instance of the <see cref="DatabaseMigrationService"/> class.
                                                   /// </summary>
                                                   /// <param name="connectionStringService">Connection string service.</param>
                                                   /// <param name="migrationOptions">Migration configuration options.</param>
                                                   /// <param name="logger">Logger instance.</param>
    public DatabaseMigrationService(
        IConnectionStringService connectionStringService,
        IOptions<MigrationOptions> migrationOptions,
        ILogger<DatabaseMigrationService> logger)
    {
        ArgumentNullException.ThrowIfNull(connectionStringService);
        ArgumentNullException.ThrowIfNull(migrationOptions);
        ArgumentNullException.ThrowIfNull(logger);

        _connectionStringService = connectionStringService;
        _migrationOptions = migrationOptions.Value;
        _logger = logger;// Configure retry policy with exponential backoff
        _retryPolicy = Policy
            .Handle<SqlException>()
            .Or<TimeoutException>()
            .WaitAndRetryAsync(
                retryCount: _migrationOptions.MaxRetryAttempts,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(
                    Math.Pow(2, retryAttempt) * _migrationOptions.RetryDelaySeconds),
                onRetry: (exception, timespan, retryCount, context) =>
                {
                    _logger.LogWarning("Retry attempt {RetryCount} after {Delay}ms. Exception: {Exception}",
                        retryCount, timespan.TotalMilliseconds, exception.Message);
                });
    }    /// <inheritdoc />
    public async Task<MigrationResult> MigrateTableAsync(
        string tableName,
        CancellationToken cancellationToken = default)
    {
        var result = new MigrationResult
        {
            TableName = tableName ?? string.Empty,
            StartTime = DateTime.UtcNow
        };

        if (string.IsNullOrWhiteSpace(tableName))
        {
            result.Success = false;
            result.ErrorMessage = "Table name cannot be null or empty.";
            result.EndTime = DateTime.UtcNow;
            return result;
        }

        try
        {
            _logger.LogInformation("Starting migration for table: {TableName}", tableName);

            var sourceConnectionString = await _connectionStringService.GetSourceConnectionStringAsync(cancellationToken);
            var destinationConnectionString = await _connectionStringService.GetDestinationConnectionStringAsync(cancellationToken);

            await using var sourceConnection = new SqlConnection(sourceConnectionString);
            await using var destinationConnection = new SqlConnection(destinationConnectionString);

            await sourceConnection.OpenAsync(cancellationToken);
            await destinationConnection.OpenAsync(cancellationToken);

            // Get total record count for progress tracking
            var totalRecords = await GetRecordCountAsync(sourceConnection, tableName, cancellationToken);
            _logger.LogInformation("Total records to migrate from {TableName}: {TotalRecords}",
                tableName, totalRecords);

            // Clear destination table
            await ClearDestinationTableAsync(destinationConnection, tableName, cancellationToken);

            // Migrate data in batches
            var migratedRecords = await MigrateDataInBatchesAsync(
                sourceConnection,
                destinationConnection,
                tableName,
                totalRecords,
                cancellationToken);

            result.Success = true;
            result.RecordsProcessed = migratedRecords;
            result.EndTime = DateTime.UtcNow;

            _logger.LogInformation("Successfully migrated {RecordsProcessed} records from table: {TableName} in {Duration}ms",
                result.RecordsProcessed, tableName, result.Duration.TotalMilliseconds);
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = ex.Message;
            result.EndTime = DateTime.UtcNow;

            _logger.LogError(ex, "Failed to migrate table: {TableName}", tableName);
        }

        return result;
    }

    /// <inheritdoc />
    public async Task<MigrationResult> MigrateAllTablesAsync(CancellationToken cancellationToken = default)
    {
        var overallResult = new MigrationResult
        {
            TableName = "ALL_TABLES",
            StartTime = DateTime.UtcNow
        };

        try
        {
            _logger.LogInformation("Starting migration for all tables");

            var sourceConnectionString = await _connectionStringService.GetSourceConnectionStringAsync(cancellationToken);
            await using var sourceConnection = new SqlConnection(sourceConnectionString);
            await sourceConnection.OpenAsync(cancellationToken);

            var tableNames = await GetUserTablesAsync(sourceConnection, cancellationToken);
            _logger.LogInformation("Found {TableCount} tables to migrate", tableNames.Count);

            var totalRecordsProcessed = 0;
            var successfulTables = 0;
            var failedTables = new List<string>();

            foreach (var tableName in tableNames)
            {
                var tableResult = await MigrateTableAsync(tableName, cancellationToken);
                totalRecordsProcessed += tableResult.RecordsProcessed;

                if (tableResult.Success)
                {
                    successfulTables++;
                }
                else
                {
                    failedTables.Add(tableName);
                }
            }

            overallResult.Success = failedTables.Count == 0;
            overallResult.RecordsProcessed = totalRecordsProcessed;
            overallResult.EndTime = DateTime.UtcNow;

            if (failedTables.Count > 0)
            {
                overallResult.ErrorMessage = $"Failed to migrate {failedTables.Count} tables: {string.Join(", ", failedTables)}";
            }

            _logger.LogInformation("Migration completed. Successfully migrated {SuccessfulTables}/{TotalTables} tables, {TotalRecords} total records in {Duration}ms",
                successfulTables, tableNames.Count, totalRecordsProcessed, overallResult.Duration.TotalMilliseconds);
        }
        catch (Exception ex)
        {
            overallResult.Success = false;
            overallResult.ErrorMessage = ex.Message;
            overallResult.EndTime = DateTime.UtcNow;

            _logger.LogError(ex, "Failed to migrate all tables");
        }

        return overallResult;
    }

    /// <summary>
    /// Gets the record count for a table.
    /// </summary>
    /// <param name="connection">Database connection.</param>
    /// <param name="tableName">Table name.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Record count.</returns>
    private async Task<int> GetRecordCountAsync(
        SqlConnection connection,
        string tableName,
        CancellationToken cancellationToken)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            await using var command = new SqlCommand($"SELECT COUNT(*) FROM [{tableName}]", connection)
            {
                CommandTimeout = _migrationOptions.CommandTimeoutSeconds
            };

            var result = await command.ExecuteScalarAsync(cancellationToken);
            return Convert.ToInt32(result);
        });
    }

    /// <summary>
    /// Clears the destination table.
    /// </summary>
    /// <param name="connection">Database connection.</param>
    /// <param name="tableName">Table name.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    private async Task ClearDestinationTableAsync(
        SqlConnection connection,
        string tableName,
        CancellationToken cancellationToken)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            await using var command = new SqlCommand($"DELETE FROM [{tableName}]", connection)
            {
                CommandTimeout = _migrationOptions.CommandTimeoutSeconds
            };

            var deletedRows = await command.ExecuteNonQueryAsync(cancellationToken);
            _logger.LogInformation("Cleared {DeletedRows} existing records from destination table: {TableName}",
                deletedRows, tableName);
        });
    }

    /// <summary>
    /// Migrates data in batches between source and destination.
    /// </summary>
    /// <param name="sourceConnection">Source database connection.</param>
    /// <param name="destinationConnection">Destination database connection.</param>
    /// <param name="tableName">Table name.</param>
    /// <param name="totalRecords">Total number of records.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Number of migrated records.</returns>
    private async Task<int> MigrateDataInBatchesAsync(
        SqlConnection sourceConnection,
        SqlConnection destinationConnection,
        string tableName,
        int totalRecords,
        CancellationToken cancellationToken)
    {
        var offset = 0;
        var totalMigrated = 0;

        while (offset < totalRecords)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var currentBatchSize = Math.Min(_migrationOptions.BatchSize, totalRecords - offset);

            await _retryPolicy.ExecuteAsync(async () =>
            {
                // Read batch from source
                await using var sourceCommand = new SqlCommand(
                    $"SELECT * FROM [{tableName}] ORDER BY (SELECT NULL) OFFSET {offset} ROWS FETCH NEXT {currentBatchSize} ROWS ONLY",
                    sourceConnection)
                {
                    CommandTimeout = _migrationOptions.CommandTimeoutSeconds
                };

                await using var reader = await sourceCommand.ExecuteReaderAsync(cancellationToken);
                var dataTable = new DataTable();
                dataTable.Load(reader);

                if (dataTable.Rows.Count > 0)
                {
                    // Write batch to destination
                    using var bulkCopy = new SqlBulkCopy(destinationConnection)
                    {
                        DestinationTableName = tableName,
                        BulkCopyTimeout = _migrationOptions.CommandTimeoutSeconds,
                        BatchSize = currentBatchSize
                    };

                    await bulkCopy.WriteToServerAsync(dataTable, cancellationToken);
                    totalMigrated += dataTable.Rows.Count;

                    if (_migrationOptions.EnableDetailedLogging)
                    {
                        _logger.LogDebug("Migrated batch for {TableName}: {CurrentMigrated}/{TotalRecords} records",
                            tableName, totalMigrated, totalRecords);
                    }
                }
            });

            offset += currentBatchSize;
        }

        return totalMigrated;
    }

    /// <summary>
    /// Gets the list of user tables in the database.
    /// </summary>
    /// <param name="connection">Database connection.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of table names.</returns>
    private async Task<List<string>> GetUserTablesAsync(
        SqlConnection connection,
        CancellationToken cancellationToken)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            var tables = new List<string>();

            await using var command = new SqlCommand(
                "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA = 'dbo'",
                connection)
            {
                CommandTimeout = _migrationOptions.CommandTimeoutSeconds
            };

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                tables.Add(reader.GetString(0));
            }

            return tables;
        });
    }
}
