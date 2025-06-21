namespace EastSeat.ResourceIdea.Migration.Configuration;

/// <summary>
/// Configuration options for the migration process.
/// </summary>
public sealed class MigrationOptions
{
    /// <summary>
    /// Configuration section name.
    /// </summary>
    public const string SectionName = "Migration";

    /// <summary>
    /// Gets or sets the batch size for data migration operations.
    /// </summary>
    public int BatchSize { get; set; } = 1000;

    /// <summary>
    /// Gets or sets the command timeout in seconds for database operations.
    /// </summary>
    public int CommandTimeoutSeconds { get; set; } = 300;

    /// <summary>
    /// Gets or sets the maximum number of retry attempts for failed operations.
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Gets or sets the delay between retry attempts in seconds.
    /// </summary>
    public int RetryDelaySeconds { get; set; } = 5;

    /// <summary>
    /// Gets or sets a value indicating whether detailed logging is enabled.
    /// </summary>
    public bool EnableDetailedLogging { get; set; } = true;
}
