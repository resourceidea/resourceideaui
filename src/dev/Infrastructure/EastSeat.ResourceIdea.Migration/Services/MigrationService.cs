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
        Console.Write($" - Migrating [{tableDefinition.Schema}].[{tableDefinition.Table}]...");

        try
        {
            HashSet<MigrationSourceData> sourceData = GetSourceDataToMigrate(tableDefinition);
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write($" {sourceData.Count} items to migrate");
            Console.ResetColor();

            MigrationResult migrationResult = MigrateDataToDestination(tableDefinition, sourceData);

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
                ItemMigrationResult result = ExecuteDataMigrationCommands(tableDefinition, item, destinationConnection);

                var resultTuple = new Tuple<MigrationSourceData, ItemMigrationResult>(item, result);

                _ = result switch
                {
                    ItemMigrationResult.Skipped => migrationResult.Skipped.Add(resultTuple),
                    ItemMigrationResult.Migrated => migrationResult.Migrated.Add(resultTuple),
                    _ => migrationResult.Failed.Add(resultTuple)
                };
            }
        }
        catch (Exception)
        {
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
        try
        {
            // Build check query to see if record already exists
            var migrableColumns = tableDefinition.Destination.Columns.Where(c => c.IsMigratable && !string.IsNullOrEmpty(c.SourceColumn)).ToList();

            if (migrableColumns.Count == 0)
            {
                return ItemMigrationResult.Skipped; // Nothing to migrate
            }

            // Check if record already exists using migrable columns
            var checkConditions = migrableColumns.Select(c => $"[{c.Name}] = @{c.Name}").ToList();
            var checkQuery = new SqlCommand($"SELECT COUNT(*) FROM [{tableDefinition.Destination.Schema}].[{tableDefinition.Destination.Table}] WHERE {string.Join(" AND ", checkConditions)}", connection);

            foreach (var column in migrableColumns)
            {
                var sourceValue = sourceData.GetValue(column.SourceColumn!);
                checkQuery.Parameters.AddWithValue($"@{column.Name}", sourceValue ?? DBNull.Value);
            }

            using var checkReader = checkQuery.ExecuteReader();
            if (checkReader.Read() && checkReader.GetInt32(0) > 0)
            {
                checkReader.Close();
                return ItemMigrationResult.Skipped;
            }
            checkReader.Close();

            // Build insert command
            var insertColumns = migrableColumns.Select(c => $"[{c.Name}]").ToList();
            var insertValues = migrableColumns.Select(c => $"@{c.Name}").ToList();

            // Add non-migrable columns with default values
            var nonMigrableColumns = tableDefinition.Destination.Columns.Where(c => !c.IsMigratable).ToList();
            foreach (var column in nonMigrableColumns)
            {
                insertColumns.Add($"[{column.Name}]");
                insertValues.Add(GetDefaultValueForColumn(column));
            }

            var insertCommand = new SqlCommand($"INSERT INTO [{tableDefinition.Destination.Schema}].[{tableDefinition.Destination.Table}] ({string.Join(", ", insertColumns)}) VALUES ({string.Join(", ", insertValues)})", connection);

            // Add parameters for migrable columns
            foreach (var column in migrableColumns)
            {
                var sourceValue = sourceData.GetValue(column.SourceColumn!);
                insertCommand.Parameters.AddWithValue($"@{column.Name}", sourceValue ?? DBNull.Value);
            }

            insertCommand.ExecuteNonQuery();
            return ItemMigrationResult.Migrated;
        }
        catch
        {
            return ItemMigrationResult.Failed;
        }
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
            "tenantid" => "NEWID()",
            "created" => "SYSDATETIMEOFFSET()",
            "createdby" => "'MIGRATION'",
            "lastmodified" => "SYSDATETIMEOFFSET()",
            "lastmodifiedby" => "'MIGRATION'",
            "isdeleted" => "0",
            "deleted" => "NULL",
            "deletedby" => "NULL",
            _ when column.Type.Contains("bit") => "0",
            _ when column.Type.Contains("datetime") => "NULL",
            _ when column.Type.Contains("datetimeoffset") => "NULL",
            _ when column.Type.Contains("varchar") || column.Type.Contains("nvarchar") => "NULL",
            _ when column.Type.Contains("int") || column.Type.Contains("decimal") || column.Type.Contains("float") => "0",
            _ => "NULL"
        };
    }
}
