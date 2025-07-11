namespace EastSeat.ResourceIdea.Migration.Model;

/// <summary>
/// Represents the result of a migration operation.
/// </summary>
public class MigrationResult
{
    /// <summary>
    /// Gets or sets the total number of items to migrate.
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// Gets or sets the items that were skipped during migration.
    /// </summary>
    public HashSet<Tuple<MigrationSourceData, ItemMigrationResult>> Skipped { get; set; } = [];

    /// <summary>
    /// Gets or sets the items that were successfully migrated.
    /// </summary>
    public HashSet<Tuple<MigrationSourceData, ItemMigrationResult>> Migrated { get; set; } = [];

    /// <summary>
    /// Gets or sets the items that failed to migrate.
    /// </summary>
    public HashSet<Tuple<MigrationSourceData, ItemMigrationResult>> Failed { get; set; } = [];
}
