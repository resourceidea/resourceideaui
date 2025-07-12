using System.Data.SqlClient;
using EastSeat.ResourceIdea.Migration.Model;

namespace EastSeat.ResourceIdea.Migration.Services;

/// <summary>
/// Service for handling database migration operations.
/// </summary>
public class MigrationService
{
    /// <summary>
    /// Runs the migration for a specific table definition.
    /// </summary>
    /// <param name="tableDefinition">The table definition containing source and destination mapping.</param>
    public static void RunMigration(TableDefinition tableDefinition)
    {
        var tableName = $"{tableDefinition.Schema}.{tableDefinition.Table}";
        Console.Write($" - Migrating [{tableDefinition.Schema}].[{tableDefinition.Table}]...");
        MigrationLogger.LogInfo($"Starting migration for table: {tableName}");

        try
        {
            HashSet<MigrationSourceData> sourceData = GetSourceDataToMigrate(tableDefinition);
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write($" {sourceData.Count} items to migrate");
            Console.ResetColor();

            MigrationLogger.LogInfo($"Found {sourceData.Count} items to migrate for table: {tableName}");

            MigrationResult migrationResult = MigrateDataToDestination(tableDefinition, sourceData);

            // Log detailed migration results
            MigrationLogger.LogMigrationResult(tableName, migrationResult);

            // Display migration results
            Console.Write($" Results:");
            Console.Write($"  Total: {migrationResult.Total}");
            if (migrationResult.Migrated.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.Write($"  Migrated: {migrationResult.Migrated.Count}");
            Console.ResetColor();
            Console.Write($"  Skipped: {migrationResult.Skipped.Count}");
            if (migrationResult.Failed.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.Write($"  Failed: {migrationResult.Failed.Count}");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($" FAILED");
            Console.Write($"  Error: {ex.Message}");
            Console.ResetColor();

            MigrationLogger.LogError($"Migration failed for table: {tableName}", ex);
        }

        Console.WriteLine();
    }

    /// <summary>
    /// Retrieves source data from the source table.
    /// </summary>
    /// <param name="tableDefinition">The table definition containing source information.</param>
    /// <returns>A collection of source data to migrate.</returns>
    private static HashSet<MigrationSourceData> GetSourceDataToMigrate(TableDefinition tableDefinition)
    {
        string connectionString = ConnectionStringService.GetSourceConnectionString();
        using var connection = new SqlConnection(connectionString);
        connection.Open();

        // Build projection list from source columns
        string projectionList = string.Join(", ", tableDefinition.Columns.Select(c => $"[{c.Name}]"));
        var command = new SqlCommand($"SELECT {projectionList} FROM [{tableDefinition.Schema}].[{tableDefinition.Table}]", connection);

        using var reader = command.ExecuteReader();
        var data = new HashSet<MigrationSourceData>();

        while (reader.Read())
        {
            var sourceData = new MigrationSourceData();

            // Read all columns from the source
            for (int i = 0; i < tableDefinition.Columns.Count; i++)
            {
                var columnName = tableDefinition.Columns[i].Name;
                var value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                sourceData.SetValue(columnName, value);
            }

            data.Add(sourceData);
        }

        return data;
    }

    /// <summary>
    /// Migrates data from source to destination tables.
    /// </summary>
    /// <param name="tableDefinition">The table definition containing destination mapping.</param>
    /// <param name="sourceData">The source data to migrate.</param>
    /// <returns>The migration result.</returns>
    private static MigrationResult MigrateDataToDestination(TableDefinition tableDefinition, HashSet<MigrationSourceData> sourceData)
    {
        var tableName = $"{tableDefinition.Schema}.{tableDefinition.Table}";
        var migrationResult = new MigrationResult
        {
            Total = sourceData.Count
        };

        try
        {
            string destinationConnectionString = ConnectionStringService.GetDestinationConnectionString();
            using var destinationConnection = new SqlConnection(destinationConnectionString);
            destinationConnection.Open();

            foreach (var item in sourceData)
            {
                try
                {
                    ItemMigrationResult result = ExecuteDataMigrationCommands(tableDefinition, item, destinationConnection);

                    var resultTuple = new Tuple<MigrationSourceData, ItemMigrationResult>(item, result);

                    _ = result switch
                    {
                        ItemMigrationResult.Skipped => migrationResult.Skipped.Add(resultTuple),
                        ItemMigrationResult.Migrated => migrationResult.Migrated.Add(resultTuple),
                        _ => migrationResult.Failed.Add(resultTuple)
                    };

                    // Log individual failures with more detail
                    if (result == ItemMigrationResult.Failed)
                    {
                        MigrationLogger.LogWarning($"Individual item migration failed for table: {tableName}. Source data: {item}");
                    }
                }
                catch (Exception ex)
                {
                    // Log individual item failures with exception details
                    MigrationLogger.LogMigrationFailure(tableName, item, ex);

                    var resultTuple = new Tuple<MigrationSourceData, ItemMigrationResult>(item, ItemMigrationResult.Failed);
                    migrationResult.Failed.Add(resultTuple);
                }
            }
        }
        catch (Exception ex)
        {
            MigrationLogger.LogError($"Connection or general failure during migration for table: {tableName}", ex);

            // If there's a connection failure, mark all remaining items as failed
            foreach (var item in sourceData)
            {
                var resultTuple = new Tuple<MigrationSourceData, ItemMigrationResult>(item, ItemMigrationResult.Failed);
                migrationResult.Failed.Add(resultTuple);
            }
        }

        return migrationResult;
    }

    /// <summary>
    /// Executes the migration commands for a single data item.
    /// </summary>
    /// <param name="tableDefinition">The table definition containing destination mapping.</param>
    /// <param name="sourceData">The source data item to migrate.</param>
    /// <param name="connection">The database connection to use.</param>
    /// <returns>The result of the migration operation.</returns>
    private static ItemMigrationResult ExecuteDataMigrationCommands(TableDefinition tableDefinition, MigrationSourceData sourceData, SqlConnection connection)
    {
        var tableName = $"{tableDefinition.Schema}.{tableDefinition.Table}";

        try
        {
            // Build check query to see if record already exists using migrable columns that serve as unique identifiers
            var identifierColumns = GetIdentifierColumns(tableDefinition);

            if (identifierColumns.Count == 0)
            {
                return ItemMigrationResult.Skipped; // Nothing to migrate
            }

            // Check if record already exists using identifier columns
            var checkConditions = identifierColumns.Select(c => $"[{c.Name}] = @{c.Name}").ToList();
            var checkQuery = new SqlCommand($"SELECT COUNT(*) FROM [{tableDefinition.Destination.Schema}].[{tableDefinition.Destination.Table}] WHERE {string.Join(" AND ", checkConditions)}", connection);

            foreach (var column in identifierColumns)
            {
                var sourceValue = GetColumnValue(column, sourceData, connection);
                checkQuery.Parameters.AddWithValue($"@{column.Name}", sourceValue ?? DBNull.Value);
            }

            using var checkReader = checkQuery.ExecuteReader();
            if (checkReader.Read() && checkReader.GetInt32(0) > 0)
            {
                checkReader.Close();
                return ItemMigrationResult.Skipped;
            }
            checkReader.Close();

            // Build insert command with all destination columns
            var insertColumns = new List<string>();
            var insertValues = new List<string>();
            var insertCommand = new SqlCommand();
            insertCommand.Connection = connection;

            foreach (var column in tableDefinition.Destination.Columns)
            {
                insertColumns.Add($"[{column.Name}]");

                var columnValue = GetColumnValue(column, sourceData, connection);

                if (columnValue is string stringValue && IsDefaultValueExpression(stringValue))
                {
                    insertValues.Add(stringValue);
                }
                else
                {
                    var paramName = $"@{column.Name}";
                    insertValues.Add(paramName);
                    insertCommand.Parameters.AddWithValue(paramName, columnValue ?? DBNull.Value);
                }
            }

            insertCommand.CommandText = $"INSERT INTO [{tableDefinition.Destination.Schema}].[{tableDefinition.Destination.Table}] ({string.Join(", ", insertColumns)}) VALUES ({string.Join(", ", insertValues)})";

            try
            {
                insertCommand.ExecuteNonQuery();
                return ItemMigrationResult.Migrated;
            }
            catch (Exception ex)
            {
                // Log the SQL command that failed for debugging
                MigrationLogger.LogError($"SQL execution failed for table: {tableName}\n" +
                                       $"  SQL Command: {insertCommand.CommandText}\n" +
                                       $"  Source Data: {sourceData}", ex);
                return ItemMigrationResult.Failed;
            }
        }
        catch (Exception ex)
        {
            MigrationLogger.LogError($"Migration command preparation failed for table: {tableName}\n" +
                                   $"  Source Data: {sourceData}", ex);
            return ItemMigrationResult.Failed;
        }
    }

    /// <summary>
    /// Gets the columns that serve as identifiers for checking if a record already exists.
    /// </summary>
    /// <param name="tableDefinition">The table definition.</param>
    /// <returns>List of identifier columns.</returns>
    private static List<DestinationColumnDefinition> GetIdentifierColumns(TableDefinition tableDefinition)
    {
        // Use migrable columns as identifiers, or if none exist, use columns with specific names
        var migrableColumns = tableDefinition.Destination.Columns.Where(c => c.IsMigratable && !string.IsNullOrEmpty(c.SourceColumn)).ToList();

        if (migrableColumns.Count > 0)
        {
            return migrableColumns;
        }

        // Fallback to common identifier column names
        var identifierNames = new[] { "Id", "MigrationClientId", "MigrationCompanyCode" };
        return tableDefinition.Destination.Columns.Where(c => identifierNames.Contains(c.Name)).ToList();
    }

    /// <summary>
    /// Gets the value for a destination column based on its configuration.
    /// </summary>
    /// <param name="column">The destination column definition.</param>
    /// <param name="sourceData">The source data.</param>
    /// <param name="connection">The database connection for lookups.</param>
    /// <returns>The value for the column.</returns>
    private static object? GetColumnValue(DestinationColumnDefinition column, MigrationSourceData sourceData, SqlConnection connection)
    {
        // Handle lookup columns
        if (!string.IsNullOrEmpty(column.LookupTable) && !string.IsNullOrEmpty(column.LookupColumn)
            && !string.IsNullOrEmpty(column.LookupCondition) && !string.IsNullOrEmpty(column.LookupSource))
        {
            return PerformLookup(column, sourceData, connection);
        }

        // Handle transform columns
        if (!string.IsNullOrEmpty(column.Transform) && !string.IsNullOrEmpty(column.SourceColumn))
        {
            return ApplyTransform(column, sourceData);
        }

        // Handle migrable columns
        if (column.IsMigratable && !string.IsNullOrEmpty(column.SourceColumn))
        {
            return sourceData.GetValue(column.SourceColumn);
        }

        // Handle non-migrable columns with default values
        return GetDefaultValueForColumn(column);
    }

    /// <summary>
    /// Performs a lookup operation to resolve foreign key values.
    /// </summary>
    /// <param name="column">The column definition with lookup configuration.</param>
    /// <param name="sourceData">The source data.</param>
    /// <param name="connection">The database connection.</param>
    /// <returns>The looked up value.</returns>
    private static object? PerformLookup(DestinationColumnDefinition column, MigrationSourceData sourceData, SqlConnection connection)
    {
        var lookupValue = sourceData.GetValue(column.LookupSource!);
        if (lookupValue == null)
        {
            return null;
        }

        var lookupQuery = new SqlCommand($"SELECT [{column.LookupColumn}] FROM [{column.LookupTable}] WHERE [{column.LookupCondition}] = @lookupValue", connection);
        lookupQuery.Parameters.AddWithValue("@lookupValue", lookupValue);

        using var reader = lookupQuery.ExecuteReader();
        if (reader.Read())
        {
            var result = reader.IsDBNull(0) ? null : reader.GetValue(0);
            reader.Close();
            return result;
        }
        reader.Close();

        return null;
    }

    /// <summary>
    /// Applies a transformation to a source value.
    /// </summary>
    /// <param name="column">The column definition with transform configuration.</param>
    /// <param name="sourceData">The source data.</param>
    /// <returns>The transformed value.</returns>
    private static object? ApplyTransform(DestinationColumnDefinition column, MigrationSourceData sourceData)
    {
        var sourceValue = sourceData.GetValue(column.SourceColumn!);

        return column.Transform switch
        {
            "InvertActive" => InvertActiveToBool(sourceValue),
            "ConditionalDeletedDate" => GetConditionalDeletedDate(sourceValue),
            "ConditionalDeletedBy" => GetConditionalDeletedBy(sourceValue),
            _ => sourceValue
        };
    }

    /// <summary>
    /// Inverts an Active bit value to IsDeleted (Active = 1 becomes IsDeleted = 0, Active = 0 becomes IsDeleted = 1).
    /// </summary>
    /// <param name="activeValue">The active value.</param>
    /// <returns>The inverted boolean value.</returns>
    private static bool InvertActiveToBool(object? activeValue)
    {
        if (activeValue is bool boolValue)
        {
            return !boolValue;
        }

        if (activeValue is byte byteValue)
        {
            return byteValue == 0;
        }

        if (activeValue is int intValue)
        {
            return intValue == 0;
        }

        // Default to not deleted if we can't determine the active state
        return false;
    }

    /// <summary>
    /// Gets the deleted date based on the Active value (if Active = 0, return current date, otherwise null).
    /// </summary>
    /// <param name="activeValue">The active value.</param>
    /// <returns>The deleted date or null.</returns>
    private static object? GetConditionalDeletedDate(object? activeValue)
    {
        bool isActive = GetActiveBoolean(activeValue);
        return isActive ? null : "SYSDATETIMEOFFSET()";
    }

    /// <summary>
    /// Gets the deleted by value based on the Active value (if Active = 0, return 'MIGRATION', otherwise null).
    /// </summary>
    /// <param name="activeValue">The active value.</param>
    /// <returns>The deleted by value or null.</returns>
    private static object? GetConditionalDeletedBy(object? activeValue)
    {
        bool isActive = GetActiveBoolean(activeValue);
        return isActive ? null : "MIGRATION";
    }

    /// <summary>
    /// Converts various active value types to boolean.
    /// </summary>
    /// <param name="activeValue">The active value.</param>
    /// <returns>True if active, false otherwise.</returns>
    private static bool GetActiveBoolean(object? activeValue)
    {
        return activeValue switch
        {
            bool boolValue => boolValue,
            byte byteValue => byteValue != 0,
            int intValue => intValue != 0,
            _ => false
        };
    }

    /// <summary>
    /// Checks if a value is a default value expression (like NEWID(), SYSDATETIMEOFFSET()).
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>True if it's a SQL expression, false otherwise.</returns>
    private static bool IsDefaultValueExpression(string value)
    {
        var expressions = new[] { "NEWID()", "SYSDATETIMEOFFSET()", "NULL" };
        return expressions.Any(expr => value.Equals(expr, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Gets the default value expression for a non-migrable column.
    /// </summary>
    /// <param name="column">The column definition.</param>
    /// <returns>The default value expression for the column.</returns>
    private static string GetDefaultValueForColumn(DestinationColumnDefinition column)
    {
        return column.Name.ToLowerInvariant() switch
        {
            "id" => "NEWID()",
            "tenantid" => "NEWID()",
            "created" => "SYSDATETIMEOFFSET()",
            "createdby" => "'MIGRATION'",
            "lastmodified" => "SYSDATETIMEOFFSET()",
            "lastmodifiedby" => "'MIGRATION'",
            "isdeleted" => "0",
            "deleted" => "NULL",
            "deletedby" => "NULL",
            "address_building" => "''",
            "address_street" => "''",
            "address_city" => "''",
            _ when column.Type.Contains("bit") => "0",
            _ when column.Type.Contains("datetime") => "NULL",
            _ when column.Type.Contains("datetimeoffset") => "NULL",
            _ when column.Type.Contains("varchar") || column.Type.Contains("nvarchar") => "NULL",
            _ when column.Type.Contains("int") || column.Type.Contains("decimal") || column.Type.Contains("float") => "0",
            _ => "NULL"
        };
    }
}
