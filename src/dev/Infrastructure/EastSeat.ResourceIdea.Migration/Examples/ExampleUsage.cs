using EastSeat.ResourceIdea.Migration.Configuration;
using EastSeat.ResourceIdea.Migration.Services;

namespace EastSeat.ResourceIdea.Migration.Examples;

/// <summary>
/// Example usage of the migration service with the new structure.
/// </summary>
public static class ExampleUsage
{
    /// <summary>
    /// Example of how to run migration for all tables defined in the JSON configuration.
    /// This method demonstrates graceful error handling where individual table failures
    /// don't stop the overall migration process.
    /// </summary>
    public static void RunAllMigrations()
    {
        Console.WriteLine("Starting migration process...");

        var tablesToMigrate = TableDefinitions.TablesToMigrate;

        Console.WriteLine($"Found {tablesToMigrate.Count} tables to migrate:");

        foreach (var tableDefinition in tablesToMigrate)
        {
            // Each table migration is handled independently
            // If one table fails, processing continues with the next table
            MigrationService.RunMigration(tableDefinition);
        }

        Console.WriteLine("Migration process completed.");
        Console.WriteLine("Note: Individual table failures are reported but don't stop the overall process.");
    }

    /// <summary>
    /// Example of how to migrate a specific table by name.
    /// </summary>
    /// <param name="tableName">The name of the table to migrate.</param>
    public static void RunSpecificTableMigration(string tableName)
    {
        var tablesToMigrate = TableDefinitions.TablesToMigrate;
        var tableDefinition = tablesToMigrate.FirstOrDefault(t =>
            t.Table.Equals(tableName, StringComparison.OrdinalIgnoreCase));

        if (tableDefinition == null)
        {
            Console.WriteLine($"Table '{tableName}' not found in migration configuration.");
            return;
        }

        Console.WriteLine($"Migrating specific table: {tableDefinition.Schema}.{tableDefinition.Table}");

        try
        {
            MigrationService.RunMigration(tableDefinition);
            Console.WriteLine($"Successfully completed migration for {tableDefinition.Table}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error migrating {tableDefinition.Table}: {ex.Message}");
        }
    }
}
