namespace EastSeat.ResourceIdea.Migration.Model;

/// <summary>
/// Represents the schema definition for a database column.
/// </summary>
/// <param name="Schema">The schema of the table containing the column.</param>
/// <param name="Name">The name of the column.</param>
/// <param name="Type">The SQL data type of the column.</param>
public readonly record struct ColumnSchema(string Schema, string Name, string Type);
