namespace EastSeat.ResourceIdea.DataStore.Configuration.DatabaseStartup;

/// <summary>
/// Configuration for a database startup task.
/// </summary>
public class DatabaseStartupTask
{
    /// <summary>
    /// The name of the task.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Whether the task is enabled.
    /// </summary>
    public bool Enabled { get; set; }
}
