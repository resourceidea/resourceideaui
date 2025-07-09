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

        var tables = TableDefinitions.TablesToMigrate;
        Console.WriteLine($"Successfully loaded {tables.Count} table definitions:");

        tables
            .OrderBy(t => t.Schema)
            .ThenBy(t => t.Table)
            .ToList()
            .ForEach(MigrationService.RunMigration);

        Console.WriteLine("Migration Tool Completed");
    }
}