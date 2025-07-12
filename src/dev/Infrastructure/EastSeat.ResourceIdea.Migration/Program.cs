using EastSeat.ResourceIdea.Migration.Configuration;
using EastSeat.ResourceIdea.Migration.Services;

namespace EastSeat.ResourceIdea.Migration;

/// <summary>
/// Entry point for the migration application.
/// </summary>
internal class Program
{
    /// <summary>
    /// Main entry point for the application.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    private static void Main(string[] args)
    {
        Console.WriteLine("Migration Tool Started");

        // Initialize logging - this will also display the log file path
        _ = MigrationLogger.GetLogFilePath(); // This triggers static constructor which shows log path

        var tables = TableDefinitions.TablesToMigrate;
        Console.WriteLine($"Successfully loaded {tables.Count} table definitions:");
        MigrationLogger.LogInfo($"Loaded {tables.Count} table definitions for migration");

        // Migrate tables in the order specified by MigrationOrder in the JSON file
        foreach (var table in tables)
        {
            Console.WriteLine($"Order {table.MigrationOrder}: {table.Schema}.{table.Table}");
            MigrationService.RunMigration(table);
        }

        Console.WriteLine("Migration Tool Completed");
        Console.WriteLine($"Detailed logs available at: {MigrationLogger.GetLogFilePath()}");
        MigrationLogger.LogInfo("Migration session completed");
    }
}