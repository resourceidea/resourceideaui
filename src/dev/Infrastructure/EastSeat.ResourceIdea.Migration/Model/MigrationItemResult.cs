namespace EastSeat.ResourceIdea.Migration.Model;

public enum ItemMigrationResult
{
    /// <summary>
    /// Item was successfully migrated to the destination.
    /// </summary>
    Migrated,

    /// <summary>
    /// Item already exists in the destination and was skipped.
    /// </summary>
    Skipped,

    /// <summary>
    /// Item failed to migrate due to an error.
    /// </summary>
    Failed
}
