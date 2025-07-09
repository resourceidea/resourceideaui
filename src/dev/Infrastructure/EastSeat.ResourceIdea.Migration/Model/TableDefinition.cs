namespace EastSeat.ResourceIdea.Migration.Model;

/// <summary>
/// Represents the root structure of the table definitions JSON file.
/// </summary>
public sealed class TableDefinitionsRoot
{
    /// <summary>
    /// Gets or sets the collection of table definitions.
    /// </summary>
    public List<TableDefinition> Tables { get; set; } = new();
}

/// <summary>
/// Represents a table definition with its source table and destination mapping.
/// </summary>
public sealed class TableDefinition
{
    /// <summary>
    /// Gets or sets the schema of the source table.
    /// </summary>
    public string Schema { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the source table.
    /// </summary>
    public string Table { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of source column definitions for this table.
    /// </summary>
    public List<SourceColumnDefinition> Columns { get; set; } = [];

    /// <summary>
    /// Gets or sets the destination table mapping.
    /// </summary>
    public DestinationTableDefinition Destination { get; set; } = new();

    // Legacy properties for backward compatibility
    /// <summary>
    /// Gets the name of the source table (legacy compatibility).
    /// </summary>
    public string TableName => Table;

    /// <summary>
    /// Gets the source columns (legacy compatibility).
    /// </summary>
    public List<ColumnSchema> SourceColumns => Columns.Select(c => new ColumnSchema(Schema, c.Name, c.Type)).ToList();

    /// <summary>
    /// Gets the destination columns (legacy compatibility).
    /// </summary>
    public List<ColumnSchema> DestinationColumns => Destination.Columns.Select(c => new ColumnSchema(Destination.Schema, c.Name, c.Type)).ToList();
}

/// <summary>
/// Represents a source column definition.
/// </summary>
public sealed class SourceColumnDefinition
{
    /// <summary>
    /// Gets or sets the name of the column.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the SQL data type of the column.
    /// </summary>
    public string Type { get; set; } = string.Empty;
}

/// <summary>
/// Represents the destination table mapping.
/// </summary>
public sealed class DestinationTableDefinition
{
    /// <summary>
    /// Gets or sets the schema of the destination table.
    /// </summary>
    public string Schema { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the destination table.
    /// </summary>
    public string Table { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of destination column definitions.
    /// </summary>
    public List<DestinationColumnDefinition> Columns { get; set; } = [];
}

/// <summary>
/// Represents a destination column definition with migration mapping.
/// </summary>
public sealed class DestinationColumnDefinition
{
    /// <summary>
    /// Gets or sets the name of the destination column.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the SQL data type of the destination column.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether this column should be migrated from the source.
    /// </summary>
    public bool IsMigratable { get; set; }

    /// <summary>
    /// Gets or sets the name of the source column to map from (if migrable).
    /// </summary>
    public string? SourceColumn { get; set; }
}
