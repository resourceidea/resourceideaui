using System.Reflection;
using System.Text.Json;
using EastSeat.ResourceIdea.Migration.Model;

namespace EastSeat.ResourceIdea.Migration.Configuration;

/// <summary>
/// Provides access to table definitions for database migration.
/// </summary>
public static class TableDefinitions
{
    private static readonly Lazy<List<TableDefinition>> _tablesToMigrate
        = new(LoadTableDefinitions);

    /// <summary>
    /// Gets the collection of tables to migrate with their definitions in migration order.
    /// </summary>
    public static List<TableDefinition> TablesToMigrate
        => _tablesToMigrate.Value;

    /// <summary>
    /// Loads table definitions from the embedded JSON file.
    /// </summary>
    /// <returns>A collection of table definitions in migration order.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the table definitions file cannot be found or parsed.</exception>
    private static List<TableDefinition> LoadTableDefinitions()
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "EastSeat.ResourceIdea.Migration.Configuration.table-definitions.json";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                // Fallback to file system if embedded resource is not found
                var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var jsonFilePath = Path.Combine(currentDirectory!, "Configuration", "table-definitions.json");

                if (!File.Exists(jsonFilePath))
                {
                    throw new InvalidOperationException($"Table definitions file not found at: {jsonFilePath}");
                }

                var jsonContent = File.ReadAllText(jsonFilePath);
                return ParseTableDefinitions(jsonContent);
            }

            using var reader = new StreamReader(stream);
            var content = reader.ReadToEnd();
            return ParseTableDefinitions(content);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to load table definitions.", ex);
        }
    }

    /// <summary>
    /// Parses the JSON content and converts it to the required format.
    /// </summary>
    /// <param name="jsonContent">The JSON content containing table definitions.</param>
    /// <returns>A collection of table definitions ordered by migration order.</returns>
    private static List<TableDefinition> ParseTableDefinitions(string jsonContent)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var tableDefinitionsRoot = JsonSerializer.Deserialize<TableDefinitionsRoot>(jsonContent, options);

        if (tableDefinitionsRoot?.Tables == null)
        {
            throw new InvalidOperationException("Invalid table definitions format.");
        }

        // Sort by migration order, then by schema and table name as fallback
        return tableDefinitionsRoot.Tables
            .OrderBy(t => t.MigrationOrder)
            .ThenBy(t => t.Schema)
            .ThenBy(t => t.Table)
            .ToList();
    }
}
