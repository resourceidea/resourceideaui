using System;

namespace EastSeat.ResourceIdea.DataStore.Configuration.DatabaseStartup;

/// <summary>
/// Configuration for a database startup tasks.
/// </summary>
public class DatabaseStartupTasksConfig
{
    /// <summary>
    /// Whether the database startup tasks are enabled.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// The tasks to run on startup.
    /// </summary>
    public List<DatabaseStartupTask> Tasks { get; set; } = [];
}
