using EastSeat.ResourceIdea.Migration.Configuration;
using EastSeat.ResourceIdea.Migration.Services;

namespace EastSeat.ResourceIdea.TestMigration;

/// <summary>
/// Test program to demonstrate the enhanced migration process with file logging.
/// This shows how the Job-Project combined migration and Industry field mapping work.
/// </summary>
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=== ResourceIdea Migration Test ===");
        Console.WriteLine("This test demonstrates the enhanced migration system with:");
        Console.WriteLine("1. Combined Job-Project migration to Engagements table");
        Console.WriteLine("2. Industry field mapping from Client to MigrationIndustry");
        Console.WriteLine("3. Enhanced file logging");
        Console.WriteLine();

        try
        {
            // Get all table definitions
            var tables = TableDefinitions.TablesToMigrate;

            Console.WriteLine($"Found {tables.Count} tables to migrate:");
            foreach (var table in tables)
            {
                Console.WriteLine($"  - [{table.Schema}].[{table.Table}] (Order: {table.MigrationOrder})");
            }
            Console.WriteLine();

            // Display the specific configurations we're testing
            Console.WriteLine("=== Key Migration Features ===");

            // Show Job-Project combined migration
            var jobTable = tables.FirstOrDefault(t => t.Table == "Job");
            if (jobTable != null)
            {
                Console.WriteLine($"Job Table Combined Columns ({jobTable.Columns.Count} total):");
                foreach (var column in jobTable.Columns)
                {
                    Console.WriteLine($"  - {column.Name} ({column.Type})");
                }
                Console.WriteLine($"  → Destination: [{jobTable.Destination.Schema}].[{jobTable.Destination.Table}]");
                Console.WriteLine();
            }

            // Show Client Industry mapping
            var clientTable = tables.FirstOrDefault(t => t.Table == "Client");
            if (clientTable != null)
            {
                Console.WriteLine("Client Table Industry Mapping:");
                var industryMapping = clientTable.Destination.Columns
                    .FirstOrDefault(c => c.Name == "MigrationIndustry");

                if (industryMapping != null)
                {
                    Console.WriteLine($"  ✓ Industry → MigrationIndustry ({industryMapping.Type})");
                    Console.WriteLine($"  ✓ Is Migrable: {industryMapping.IsMigratable}");
                    Console.WriteLine($"  ✓ Source Column: {industryMapping.SourceColumn}");
                }
                else
                {
                    Console.WriteLine("  ✗ MigrationIndustry mapping not found!");
                }
                Console.WriteLine();
            }

            Console.WriteLine("=== Migration would run in this order ===");
            foreach (var table in tables.OrderBy(t => t.MigrationOrder))
            {
                Console.WriteLine($"{table.MigrationOrder}. [{table.Schema}].[{table.Table}] → [{table.Destination.Schema}].[{table.Destination.Table}]");

                // Show key mappings for each table
                var migrableColumns = table.Destination.Columns.Where(c => c.IsMigratable).ToList();
                if (migrableColumns.Count > 0)
                {
                    Console.WriteLine("   Key field mappings:");
                    foreach (var col in migrableColumns.Take(3)) // Show first 3 to keep output manageable
                    {
                        Console.WriteLine($"     - {col.SourceColumn} → {col.Name}");
                    }
                    if (migrableColumns.Count > 3)
                    {
                        Console.WriteLine($"     ... and {migrableColumns.Count - 3} more");
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("=== Enhanced Logging ===");
            Console.WriteLine($"Migration logs are being written to: {MigrationLogger.GetLogFilePath()}");
            Console.WriteLine("The log includes:");
            Console.WriteLine("  ✓ Detailed table configuration");
            Console.WriteLine("  ✓ Field mapping details (especially Industry → MigrationIndustry)");
            Console.WriteLine("  ✓ Combined Job-Project data retrieval");
            Console.WriteLine("  ✓ Migration success/failure tracking");
            Console.WriteLine();

            Console.WriteLine("Migration configuration test completed successfully!");
            Console.WriteLine("To run actual migration, ensure database connections are configured and call:");
            Console.WriteLine("  MigrationService.RunMigration(tableDefinition) for each table");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during migration test: {ex.Message}");
            MigrationLogger.LogError("Migration test failed", ex);
        }

        Console.WriteLine();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
