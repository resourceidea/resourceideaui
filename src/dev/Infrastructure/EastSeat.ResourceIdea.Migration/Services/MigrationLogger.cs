using System.Text;
using EastSeat.ResourceIdea.Migration.Model;

namespace EastSeat.ResourceIdea.Migration.Services;

/// <summary>
/// Simple file-based logger for migration operations.
/// </summary>
public static class MigrationLogger
{
    private static readonly string LogDirectory = Path.Combine(Environment.CurrentDirectory, "logs");
    private static readonly string LogFileName = $"migration-{DateTime.Now:yyyyMMdd-HHmmss}.log";
    private static readonly string LogFilePath = Path.Combine(LogDirectory, LogFileName);
    private static readonly object _lock = new object();

    static MigrationLogger()
    {
        // Ensure logs directory exists
        if (!Directory.Exists(LogDirectory))
        {
            Directory.CreateDirectory(LogDirectory);
        }

        // Log session start
        LogInfo("Migration session started");
        Console.WriteLine($"Logging to: {LogFilePath}");
    }

    /// <summary>
    /// Logs an informational message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public static void LogInfo(string message)
    {
        WriteToLog("INFO", message);
    }

    /// <summary>
    /// Logs an error message with optional exception details.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="exception">The exception to log.</param>
    public static void LogError(string message, Exception? exception = null)
    {
        var errorMessage = message;
        if (exception != null)
        {
            errorMessage += $"\n  Exception: {exception.GetType().Name}: {exception.Message}";
            if (!string.IsNullOrEmpty(exception.StackTrace))
            {
                errorMessage += $"\n  Stack Trace: {exception.StackTrace}";
            }

            // Log inner exceptions
            var innerException = exception.InnerException;
            while (innerException != null)
            {
                errorMessage += $"\n  Inner Exception: {innerException.GetType().Name}: {innerException.Message}";
                innerException = innerException.InnerException;
            }
        }

        WriteToLog("ERROR", errorMessage);
    }

    /// <summary>
    /// Logs a warning message.
    /// </summary>
    /// <param name="message">The warning message.</param>
    public static void LogWarning(string message)
    {
        WriteToLog("WARN", message);
    }

    /// <summary>
    /// Logs migration results for a table.
    /// </summary>
    /// <param name="tableName">The name of the table.</param>
    /// <param name="migrationResult">The migration result.</param>
    public static void LogMigrationResult(string tableName, MigrationResult migrationResult)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Migration completed for table: {tableName}");
        sb.AppendLine($"  Total: {migrationResult.Total}");
        sb.AppendLine($"  Migrated: {migrationResult.Migrated.Count}");
        sb.AppendLine($"  Skipped: {migrationResult.Skipped.Count}");
        sb.AppendLine($"  Failed: {migrationResult.Failed.Count}");

        if (migrationResult.Failed.Count > 0)
        {
            sb.AppendLine("  Failed items:");
            foreach (var (sourceData, result) in migrationResult.Failed)
            {
                sb.AppendLine($"    - Source data: {GetSourceDataSummary(sourceData)}");
            }
        }

        LogInfo(sb.ToString());
    }

    /// <summary>
    /// Logs detailed failure information for a specific migration item.
    /// </summary>
    /// <param name="tableName">The name of the table being migrated.</param>
    /// <param name="sourceData">The source data that failed to migrate.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogMigrationFailure(string tableName, MigrationSourceData sourceData, Exception exception)
    {
        var message = $"Migration failed for table: {tableName}\n" +
                     $"  Source data: {GetSourceDataSummary(sourceData)}\n" +
                     $"  Full source data: {GetFullSourceData(sourceData)}";

        LogError(message, exception);
    }

    /// <summary>
    /// Logs the start of a table migration with detailed configuration.
    /// </summary>
    /// <param name="tableDefinition">The table definition being migrated.</param>
    public static void LogTableMigrationStart(TableDefinition tableDefinition)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Starting migration for table: [{tableDefinition.Schema}].[{tableDefinition.Table}]");
        sb.AppendLine($"  Migration Order: {tableDefinition.MigrationOrder}");
        sb.AppendLine($"  Destination: [{tableDefinition.Destination.Schema}].[{tableDefinition.Destination.Table}]");
        sb.AppendLine($"  Source Columns: {tableDefinition.Columns.Count}");
        sb.AppendLine($"  Destination Columns: {tableDefinition.Destination.Columns.Count}");

        // Log migrable columns for tracking
        var migrableColumns = tableDefinition.Destination.Columns.Where(c => c.IsMigratable).ToList();
        if (migrableColumns.Count > 0)
        {
            sb.AppendLine($"  Migrable Field Mappings:");
            foreach (var column in migrableColumns)
            {
                sb.AppendLine($"    - {column.SourceColumn} → {column.Name} ({column.Type})");
            }
        }

        // Log lookup columns
        var lookupColumns = tableDefinition.Destination.Columns.Where(c => !string.IsNullOrEmpty(c.LookupTable)).ToList();
        if (lookupColumns.Count > 0)
        {
            sb.AppendLine($"  Lookup Columns:");
            foreach (var column in lookupColumns)
            {
                sb.AppendLine($"    - {column.Name}: {column.LookupSource} → {column.LookupTable}.{column.LookupColumn}");
            }
        }

        // Log transform columns
        var transformColumns = tableDefinition.Destination.Columns.Where(c => !string.IsNullOrEmpty(c.Transform)).ToList();
        if (transformColumns.Count > 0)
        {
            sb.AppendLine($"  Transform Columns:");
            foreach (var column in transformColumns)
            {
                sb.AppendLine($"    - {column.Name}: {column.SourceColumn} → Transform({column.Transform})");
            }
        }

        LogInfo(sb.ToString());
    }

    /// <summary>
    /// Logs detailed information about a successful field mapping during migration.
    /// </summary>
    /// <param name="tableName">The name of the table being migrated.</param>
    /// <param name="sourceColumn">The source column name.</param>
    /// <param name="destinationColumn">The destination column name.</param>
    /// <param name="value">The value being migrated.</param>
    public static void LogFieldMapping(string tableName, string sourceColumn, string destinationColumn, object? value)
    {
        var displayValue = value?.ToString() ?? "NULL";
        // Truncate long values for logging
        if (displayValue.Length > 100)
        {
            displayValue = displayValue.Substring(0, 97) + "...";
        }

        LogInfo($"Field mapping for {tableName}: {sourceColumn} → {destinationColumn} = '{displayValue}'");
    }

    /// <summary>
    /// Gets the current log file path.
    /// </summary>
    /// <returns>The full path to the log file.</returns>
    public static string GetLogFilePath()
    {
        return LogFilePath;
    }

    /// <summary>
    /// Writes a message to the log file with timestamp and level.
    /// </summary>
    /// <param name="level">The log level.</param>
    /// <param name="message">The message to log.</param>
    private static void WriteToLog(string level, string message)
    {
        lock (_lock)
        {
            try
            {
                var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                var logEntry = $"[{timestamp}] [{level}] {message}";

                File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // If we can't write to the log file, write to console
                Console.WriteLine($"Failed to write to log file: {ex.Message}");
                Console.WriteLine($"Original message: [{level}] {message}");
            }
        }
    }

    /// <summary>
    /// Gets a summary of source data for logging purposes.
    /// </summary>
    /// <param name="sourceData">The source data.</param>
    /// <returns>A string summary of the source data.</returns>
    private static string GetSourceDataSummary(MigrationSourceData sourceData)
    {
        var keyValues = new List<string>();

        // Try to get key identifying fields
        var identifierFields = new[] { "ClientId", "CompanyCode", "Id", "Name" };

        foreach (var field in identifierFields)
        {
            var value = sourceData.GetValue(field);
            if (value != null)
            {
                keyValues.Add($"{field}={value}");
            }
        }

        return keyValues.Count > 0 ? string.Join(", ", keyValues) : "No identifying fields found";
    }

    /// <summary>
    /// Gets the full source data as a string for detailed logging.
    /// </summary>
    /// <param name="sourceData">The source data.</param>
    /// <returns>A string representation of all source data.</returns>
    private static string GetFullSourceData(MigrationSourceData sourceData)
    {
        return sourceData.ToString();
    }
}
