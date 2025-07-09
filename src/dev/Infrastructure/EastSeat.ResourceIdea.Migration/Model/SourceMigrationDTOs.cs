namespace EastSeat.ResourceIdea.Migration.Model;

/// <summary>
/// Represents source data for Company migration.
/// </summary>
public readonly record struct CompanySourceDTO(string CompanyCode, string OrganizationName);

/// <summary>
/// Represents generic source data for migration with dynamic column values.
/// </summary>
public sealed class MigrationSourceData
{
    /// <summary>
    /// Gets or sets the data values indexed by column name.
    /// </summary>
    public Dictionary<string, object?> Values { get; set; } = new();

    /// <summary>
    /// Gets the value for a specific column.
    /// </summary>
    /// <param name="columnName">The column name.</param>
    /// <returns>The column value or null if not found.</returns>
    public object? GetValue(string columnName)
    {
        return Values.TryGetValue(columnName, out var value) ? value : null;
    }

    /// <summary>
    /// Sets the value for a specific column.
    /// </summary>
    /// <param name="columnName">The column name.</param>
    /// <param name="value">The column value.</param>
    public void SetValue(string columnName, object? value)
    {
        Values[columnName] = value;
    }

    /// <summary>
    /// Gets a string representation of the data for logging purposes.
    /// </summary>
    /// <returns>String representation of the data.</returns>
    public override string ToString()
    {
        var keyValuePairs = Values.Select(kvp => $"{kvp.Key}={kvp.Value ?? "NULL"}");
        return string.Join(", ", keyValuePairs);
    }
}
