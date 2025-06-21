namespace EastSeat.ResourceIdea.Migration.Models;

/// <summary>
/// Represents the result of a migration operation.
/// </summary>
public sealed class MigrationResult
{
    /// <summary>
    /// Gets or sets the name of the table that was migrated.
    /// </summary>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the migration was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the number of records processed during migration.
    /// </summary>
    public int RecordsProcessed { get; set; }

    /// <summary>
    /// Gets or sets the start time of the migration.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Gets or sets the end time of the migration.
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Gets the duration of the migration.
    /// </summary>
    public TimeSpan Duration => EndTime - StartTime;

    /// <summary>
    /// Gets or sets the error message if the migration failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Returns a string representation of the migration result.
    /// </summary>
    /// <returns>String representation of the result.</returns>
    public override string ToString()
    {
        var status = Success ? "SUCCESS" : "FAILED";
        var errorInfo = !Success && !string.IsNullOrEmpty(ErrorMessage) ? $" - {ErrorMessage}" : string.Empty;

        return $"[{status}] {TableName}: {RecordsProcessed} records in {Duration.TotalMilliseconds:F1}ms{errorInfo}";
    }
}
