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

        // Enhanced logging for migration start
        MigrationLogger.LogTableMigrationStart(tableDefinition);
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

        // Special handling for Job table - needs to JOIN with Project table
        if (tableDefinition.Schema == "dbo" && tableDefinition.Table == "Job")
        {
            return GetJobProjectCombinedData(tableDefinition, connection);
        }

        // Special handling for AspNetUsers_Resource joined table
        if (tableDefinition.IsJoinedTable && tableDefinition.JoinType == "ResourceAspNetUsers")
        {
            return GetResourceAspNetUsersCombinedData(tableDefinition, connection);
        }

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
    /// Gets combined Job and Project data for migration to Engagements table.
    /// </summary>
    /// <param name="tableDefinition">The Job table definition.</param>
    /// <param name="connection">The database connection.</param>
    /// <returns>A collection of combined Job-Project data.</returns>
    private static HashSet<MigrationSourceData> GetJobProjectCombinedData(TableDefinition tableDefinition, SqlConnection connection)
    {
        // Build the JOIN query to combine Job and Project data
        var joinQuery = @"
            SELECT 
                j.[JobId],
                j.[Description],
                j.[ProjectId],
                j.[Color],
                j.[Status],
                j.[Manager],
                j.[Partner],
                p.[Name] as ProjectName,
                p.[ClientId] as ProjectClientId
            FROM [dbo].[Job] j
            INNER JOIN [dbo].[Project] p ON j.[ProjectId] = p.[ProjectId]";

        var command = new SqlCommand(joinQuery, connection);
        using var reader = command.ExecuteReader();
        var data = new HashSet<MigrationSourceData>();

        while (reader.Read())
        {
            var sourceData = new MigrationSourceData();

            // Map all the combined columns using ordinals for performance
            sourceData.SetValue("JobId", reader.IsDBNull(0) ? null : reader.GetValue(0));
            sourceData.SetValue("Description", reader.IsDBNull(1) ? null : reader.GetValue(1));
            sourceData.SetValue("ProjectId", reader.IsDBNull(2) ? null : reader.GetValue(2));
            sourceData.SetValue("Color", reader.IsDBNull(3) ? null : reader.GetValue(3));
            sourceData.SetValue("Status", reader.IsDBNull(4) ? null : reader.GetValue(4));
            sourceData.SetValue("Manager", reader.IsDBNull(5) ? null : reader.GetValue(5));
            sourceData.SetValue("Partner", reader.IsDBNull(6) ? null : reader.GetValue(6));
            sourceData.SetValue("ProjectName", reader.IsDBNull(7) ? null : reader.GetValue(7));
            sourceData.SetValue("ProjectClientId", reader.IsDBNull(8) ? null : reader.GetValue(8));

            data.Add(sourceData);
        }

        return data;
    }

    /// <summary>
    /// Gets combined Resource and AspNetUsers data for migration to ApplicationUsers and Employees tables.
    /// </summary>
    /// <param name="tableDefinition">The AspNetUsers_Resource table definition.</param>
    /// <param name="connection">The database connection.</param>
    /// <returns>A collection of combined Resource-AspNetUsers data.</returns>
    private static HashSet<MigrationSourceData> GetResourceAspNetUsersCombinedData(TableDefinition tableDefinition, SqlConnection connection)
    {
        // Build the JOIN query to combine Resource and AspNetUsers data
        var joinQuery = @"
            SELECT 
                r.[ResourceId],
                r.[Fullname],
                r.[Email] as ResourceEmail,
                r.[CompanyCode],
                r.[JoinDate],
                r.[TerminationDate],
                r.[JobPositionId],
                r.[JobsManagedColor],
                u.[Id],
                u.[Email],
                u.[EmailConfirmed],
                u.[PasswordHash],
                u.[SecurityStamp],
                u.[PhoneNumber],
                u.[PhoneNumberConfirmed],
                u.[TwoFactorEnabled],
                u.[LockoutEndDateUtc],
                u.[LockoutEnabled],
                u.[AccessFailedCount],
                u.[UserName]
            FROM [dbo].[Resource] r
            INNER JOIN [dbo].[AspNetUsers] u ON r.[Email] = u.[Email]";

        var command = new SqlCommand(joinQuery, connection);
        using var reader = command.ExecuteReader();
        var data = new HashSet<MigrationSourceData>();

        while (reader.Read())
        {
            var sourceData = new MigrationSourceData();

            // Map all the combined columns using ordinals for performance
            sourceData.SetValue("ResourceId", reader.IsDBNull(0) ? null : reader.GetValue(0));
            sourceData.SetValue("Fullname", reader.IsDBNull(1) ? null : reader.GetValue(1));
            sourceData.SetValue("ResourceEmail", reader.IsDBNull(2) ? null : reader.GetValue(2));
            sourceData.SetValue("CompanyCode", reader.IsDBNull(3) ? null : reader.GetValue(3));
            sourceData.SetValue("JoinDate", reader.IsDBNull(4) ? null : reader.GetValue(4));
            sourceData.SetValue("TerminationDate", reader.IsDBNull(5) ? null : reader.GetValue(5));
            sourceData.SetValue("JobPositionId", reader.IsDBNull(6) ? null : reader.GetValue(6));
            sourceData.SetValue("JobsManagedColor", reader.IsDBNull(7) ? null : reader.GetValue(7));
            sourceData.SetValue("Id", reader.IsDBNull(8) ? null : reader.GetValue(8));
            sourceData.SetValue("Email", reader.IsDBNull(9) ? null : reader.GetValue(9));
            sourceData.SetValue("EmailConfirmed", reader.IsDBNull(10) ? null : reader.GetValue(10));
            sourceData.SetValue("PasswordHash", reader.IsDBNull(11) ? null : reader.GetValue(11));
            sourceData.SetValue("SecurityStamp", reader.IsDBNull(12) ? null : reader.GetValue(12));
            sourceData.SetValue("PhoneNumber", reader.IsDBNull(13) ? null : reader.GetValue(13));
            sourceData.SetValue("PhoneNumberConfirmed", reader.IsDBNull(14) ? null : reader.GetValue(14));
            sourceData.SetValue("TwoFactorEnabled", reader.IsDBNull(15) ? null : reader.GetValue(15));
            sourceData.SetValue("LockoutEndDateUtc", reader.IsDBNull(16) ? null : reader.GetValue(16));
            sourceData.SetValue("LockoutEnabled", reader.IsDBNull(17) ? null : reader.GetValue(17));
            sourceData.SetValue("AccessFailedCount", reader.IsDBNull(18) ? null : reader.GetValue(18));
            sourceData.SetValue("UserName", reader.IsDBNull(19) ? null : reader.GetValue(19));

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
                    ItemMigrationResult result;

                    // Handle multi-table migrations
                    if (tableDefinition.Destination.DestinationTables != null && tableDefinition.Destination.DestinationTables.Count > 0)
                    {
                        result = ExecuteMultiTableMigrationCommands(tableDefinition, item, destinationConnection);
                    }
                    else
                    {
                        result = ExecuteDataMigrationCommands(tableDefinition, item, destinationConnection);
                    }

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
    /// Executes migration commands for multi-table migrations (e.g., AspNetUsers_Resource to ApplicationUsers and Employees).
    /// </summary>
    /// <param name="tableDefinition">The table definition containing destination mappings.</param>
    /// <param name="sourceData">The source data item to migrate.</param>
    /// <param name="connection">The database connection to use.</param>
    /// <returns>The result of the migration operation.</returns>
    private static ItemMigrationResult ExecuteMultiTableMigrationCommands(TableDefinition tableDefinition, MigrationSourceData sourceData, SqlConnection connection)
    {
        var tableName = $"{tableDefinition.Schema}.{tableDefinition.Table}";
        var insertedIds = new Dictionary<string, object>();

        try
        {
            using var transaction = connection.BeginTransaction();

            try
            {
                // Process each destination table in order
                foreach (var destinationTable in tableDefinition.Destination.DestinationTables!)
                {
                    var tableResult = ExecuteSingleTableInsert(destinationTable, sourceData, connection, transaction, insertedIds);
                    if (tableResult != ItemMigrationResult.Migrated && tableResult != ItemMigrationResult.Skipped)
                    {
                        transaction.Rollback();
                        return ItemMigrationResult.Failed;
                    }
                }

                transaction.Commit();
                return ItemMigrationResult.Migrated;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MigrationLogger.LogError($"Multi-table migration failed for table: {tableName}", ex);
                return ItemMigrationResult.Failed;
            }
        }
        catch (Exception ex)
        {
            MigrationLogger.LogError($"Multi-table migration transaction failed for table: {tableName}", ex);
            return ItemMigrationResult.Failed;
        }
    }

    /// <summary>
    /// Executes a single table insert for multi-table migrations.
    /// </summary>
    /// <param name="destinationTable">The destination table definition.</param>
    /// <param name="sourceData">The source data.</param>
    /// <param name="connection">The database connection.</param>
    /// <param name="transaction">The database transaction.</param>
    /// <param name="insertedIds">Dictionary to track inserted IDs for linked tables.</param>
    /// <returns>The result of the insert operation.</returns>
    private static ItemMigrationResult ExecuteSingleTableInsert(
        DestinationTableDefinition destinationTable,
        MigrationSourceData sourceData,
        SqlConnection connection,
        SqlTransaction transaction,
        Dictionary<string, object> insertedIds)
    {
        try
        {
            // Log table being processed
            MigrationLogger.LogInfo($"Processing single table insert for: [{destinationTable.Schema}].[{destinationTable.Table}]");

            // Build check query to see if record already exists
            var identifierColumns = GetIdentifierColumnsForTable(destinationTable);

            if (identifierColumns.Count == 0)
            {
                return ItemMigrationResult.Skipped;
            }

            // Check if record already exists using identifier columns
            var checkConditions = identifierColumns.Select(c => $"[{c.Name}] = @{c.Name}").ToList();
            var checkQuery = new SqlCommand($"SELECT COUNT(*) FROM [{destinationTable.Schema}].[{destinationTable.Table}] WHERE {string.Join(" AND ", checkConditions)}", connection, transaction);

            MigrationLogger.LogInfo($"Checking for existing record in [{destinationTable.Schema}].[{destinationTable.Table}] using {identifierColumns.Count} identifier columns: {string.Join(", ", identifierColumns.Select(c => c.Name))}");

            foreach (var column in identifierColumns)
            {
                var sourceValue = GetColumnValueForTable(column, sourceData, connection, transaction, insertedIds);
                checkQuery.Parameters.AddWithValue($"@{column.Name}", sourceValue ?? DBNull.Value);
                MigrationLogger.LogInfo($"Identifier column {column.Name} = {sourceValue}");
            }

            using var checkReader = checkQuery.ExecuteReader();
            if (checkReader.Read() && checkReader.GetInt32(0) > 0)
            {
                checkReader.Close();
                MigrationLogger.LogInfo($"Record already exists in [{destinationTable.Schema}].[{destinationTable.Table}], skipping");

                // For skipped records, we still need to capture the existing ID for linked tables
                if (destinationTable.Schema == "identity" && destinationTable.Table == "ApplicationUsers")
                {
                    var existingIdQuery = new SqlCommand($"SELECT Id FROM [{destinationTable.Schema}].[{destinationTable.Table}] WHERE {string.Join(" AND ", checkConditions)}", connection, transaction);
                    foreach (var column in identifierColumns)
                    {
                        var sourceValue = GetColumnValueForTable(column, sourceData, connection, transaction, insertedIds);
                        existingIdQuery.Parameters.AddWithValue($"@{column.Name}", sourceValue ?? DBNull.Value);
                    }

                    var existingId = existingIdQuery.ExecuteScalar();
                    if (existingId != null)
                    {
                        insertedIds["ApplicationUsers"] = existingId;
                        MigrationLogger.LogInfo($"Captured existing ApplicationUsers Id: {existingId}");
                    }
                }

                return ItemMigrationResult.Skipped;
            }
            checkReader.Close();

            MigrationLogger.LogInfo($"No existing record found, proceeding with insert into [{destinationTable.Schema}].[{destinationTable.Table}]");

            // Build insert command with all destination columns
            var insertColumns = new List<string>();
            var insertValues = new List<string>();
            var insertCommand = new SqlCommand();
            insertCommand.Connection = connection;
            insertCommand.Transaction = transaction;

            string? newId = null;

            foreach (var column in destinationTable.Columns)
            {
                insertColumns.Add($"[{column.Name}]");

                var columnValue = GetColumnValueForTable(column, sourceData, connection, transaction, insertedIds);

                if (columnValue is string stringValue && IsDefaultValueExpression(stringValue))
                {
                    insertValues.Add(stringValue);

                    // Track new ID for linking
                    if (column.Name.Equals("Id", StringComparison.OrdinalIgnoreCase) && stringValue == "NEWID()")
                    {
                        newId = Guid.NewGuid().ToString();
                        insertValues[insertValues.Count - 1] = $"'{newId}'";
                    }
                }
                else
                {
                    var paramName = $"@{column.Name}";
                    insertValues.Add(paramName);
                    insertCommand.Parameters.AddWithValue(paramName, columnValue ?? DBNull.Value);
                }
            }

            insertCommand.CommandText = $"INSERT INTO [{destinationTable.Schema}].[{destinationTable.Table}] ({string.Join(", ", insertColumns)}) VALUES ({string.Join(", ", insertValues)})";

            insertCommand.ExecuteNonQuery();

            // Store the inserted ID for linking
            if (newId != null)
            {
                insertedIds[destinationTable.Table] = newId;
            }

            return ItemMigrationResult.Migrated;
        }
        catch (Exception ex)
        {
            MigrationLogger.LogError($"Single table insert failed for [{destinationTable.Schema}].[{destinationTable.Table}]", ex);
            return ItemMigrationResult.Failed;
        }
    }

    /// <summary>
    /// Gets the identifier columns for a specific destination table.
    /// </summary>
    /// <param name="destinationTable">The destination table definition.</param>
    /// <returns>List of identifier columns.</returns>
    private static List<DestinationColumnDefinition> GetIdentifierColumnsForTable(DestinationTableDefinition destinationTable)
    {
        // Special handling for ApplicationUsers table - use UserName as identifier to check for duplicates
        if (destinationTable.Schema == "identity" && destinationTable.Table == "ApplicationUsers")
        {
            var userNameColumn = destinationTable.Columns.FirstOrDefault(c => c.Name == "UserName");
            if (userNameColumn != null)
            {
                return new List<DestinationColumnDefinition> { userNameColumn };
            }
        }

        // Use migrable columns as identifiers, or if none exist, use columns with specific names
        var migrableColumns = destinationTable.Columns.Where(c => c.IsMigratable && !string.IsNullOrEmpty(c.SourceColumn)).ToList();

        if (migrableColumns.Count > 0)
        {
            return migrableColumns;
        }

        // Fallback to common identifier column names
        var identifierNames = new[] { "Id", "MigrationClientId", "MigrationCompanyCode", "MigrationJobId", "MigrationProjectId", "MigrationResourceId", "MigrationUserId" };
        return destinationTable.Columns.Where(c => identifierNames.Contains(c.Name)).ToList();
    }

    /// <summary>
    /// Gets the value for a destination column in multi-table migrations.
    /// </summary>
    /// <param name="column">The destination column definition.</param>
    /// <param name="sourceData">The source data.</param>
    /// <param name="connection">The database connection for lookups.</param>
    /// <param name="transaction">The database transaction.</param>
    /// <param name="insertedIds">Dictionary of inserted IDs for linked tables.</param>
    /// <returns>The value for the column.</returns>
    private static object? GetColumnValueForTable(
        DestinationColumnDefinition column,
        MigrationSourceData sourceData,
        SqlConnection connection,
        SqlTransaction transaction,
        Dictionary<string, object> insertedIds)
    {
        // Handle linked columns (references to other destination tables in the same migration)
        if (!string.IsNullOrEmpty(column.LinkedTable) && !string.IsNullOrEmpty(column.LinkedColumn))
        {
            if (insertedIds.TryGetValue(column.LinkedTable, out var linkedId))
            {
                return linkedId;
            }
            return null;
        }

        // Handle lookup columns
        if (!string.IsNullOrEmpty(column.LookupTable) && !string.IsNullOrEmpty(column.LookupColumn)
            && !string.IsNullOrEmpty(column.LookupCondition) && !string.IsNullOrEmpty(column.LookupSource))
        {
            var lookupResult = PerformLookupWithTransaction(column, sourceData, connection, transaction);

            // Special handling for JobPositionId - if NULL, find a default job position in the same company
            if (lookupResult == null && column.Name == "JobPositionId")
            {
                return GetDefaultJobPositionId(sourceData, connection, transaction);
            }

            return lookupResult;
        }

        // Handle transform columns
        if (!string.IsNullOrEmpty(column.Transform) && !string.IsNullOrEmpty(column.SourceColumn))
        {
            return ApplyTransformWithTransaction(column, sourceData, connection, transaction);
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
    /// Performs a lookup operation with transaction support.
    /// </summary>
    /// <param name="column">The column definition with lookup configuration.</param>
    /// <param name="sourceData">The source data.</param>
    /// <param name="connection">The database connection.</param>
    /// <param name="transaction">The database transaction.</param>
    /// <returns>The looked up value.</returns>
    private static object? PerformLookupWithTransaction(DestinationColumnDefinition column, MigrationSourceData sourceData, SqlConnection connection, SqlTransaction transaction)
    {
        var lookupValue = sourceData.GetValue(column.LookupSource!);
        if (lookupValue == null)
        {
            return null;
        }

        var lookupQuery = new SqlCommand($"SELECT [{column.LookupColumn}] FROM [{column.LookupTable}] WHERE [{column.LookupCondition}] = @lookupValue", connection, transaction);
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
    /// Applies a transformation with transaction support.
    /// </summary>
    /// <param name="column">The column definition with transform configuration.</param>
    /// <param name="sourceData">The source data.</param>
    /// <param name="connection">The database connection for lookups.</param>
    /// <param name="transaction">The database transaction.</param>
    /// <returns>The transformed value.</returns>
    private static object? ApplyTransformWithTransaction(DestinationColumnDefinition column, MigrationSourceData sourceData, SqlConnection connection, SqlTransaction transaction)
    {
        var sourceValue = sourceData.GetValue(column.SourceColumn!);

        return column.Transform switch
        {
            "InvertActive" => InvertActiveToBool(sourceValue),
            "ConditionalDeletedDate" => GetConditionalDeletedDate(sourceValue),
            "ConditionalDeletedBy" => GetConditionalDeletedBy(sourceValue),
            "EnsureStaffDepartment" => EnsureStaffDepartment(sourceValue, connection, transaction),
            "SplitFullnameFirst" => SplitFullnameFirst(sourceValue),
            "SplitFullnameLast" => SplitFullnameLast(sourceValue),
            "Uppercase" => sourceValue?.ToString()?.ToUpperInvariant(),
            _ => sourceValue
        };
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
        var identifierNames = new[] { "Id", "MigrationClientId", "MigrationCompanyCode", "MigrationJobId", "MigrationProjectId" };
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
            return ApplyTransform(column, sourceData, connection);
        }

        // Handle migrable columns
        if (column.IsMigratable && !string.IsNullOrEmpty(column.SourceColumn))
        {
            var value = sourceData.GetValue(column.SourceColumn);

            // Log important field mappings (like Industry → MigrationIndustry)
            if (column.SourceColumn.Equals("Industry", StringComparison.OrdinalIgnoreCase) ||
                column.Name.Equals("MigrationIndustry", StringComparison.OrdinalIgnoreCase))
            {
                MigrationLogger.LogFieldMapping("Client", column.SourceColumn, column.Name, value);
            }

            return value;
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
    /// <param name="connection">The database connection for lookups.</param>
    /// <returns>The transformed value.</returns>
    private static object? ApplyTransform(DestinationColumnDefinition column, MigrationSourceData sourceData, SqlConnection connection)
    {
        var sourceValue = sourceData.GetValue(column.SourceColumn!);

        return column.Transform switch
        {
            "InvertActive" => InvertActiveToBool(sourceValue),
            "ConditionalDeletedDate" => GetConditionalDeletedDate(sourceValue),
            "ConditionalDeletedBy" => GetConditionalDeletedBy(sourceValue),
            "EnsureStaffDepartment" => EnsureStaffDepartment(sourceValue, connection, null),
            "SplitFullnameFirst" => SplitFullnameFirst(sourceValue),
            "SplitFullnameLast" => SplitFullnameLast(sourceValue),
            "Uppercase" => sourceValue?.ToString()?.ToUpperInvariant(),
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
    /// Ensures a STAFF department exists for the given company and returns its ID.
    /// </summary>
    /// <param name="companyCodeValue">The company code value.</param>
    /// <param name="connection">The database connection.</param>
    /// <param name="transaction">The database transaction (optional).</param>
    /// <returns>The department ID of the STAFF department.</returns>
    private static object? EnsureStaffDepartment(object? companyCodeValue, SqlConnection connection, SqlTransaction? transaction)
    {
        if (companyCodeValue == null)
        {
            return null;
        }

        var companyCode = companyCodeValue.ToString();

        // Get TenantId from the company code
        var tenantQuery = new SqlCommand("SELECT [TenantId] FROM [dbo].[Tenants] WHERE [MigrationCompanyCode] = @companyCode", connection, transaction);
        tenantQuery.Parameters.AddWithValue("@companyCode", companyCode);

        using var tenantReader = tenantQuery.ExecuteReader();
        if (!tenantReader.Read())
        {
            tenantReader.Close();
            return null;
        }

        var tenantId = tenantReader.GetValue(0);
        tenantReader.Close();

        // Check if STAFF department exists for this tenant
        var departmentQuery = new SqlCommand("SELECT [Id] FROM [dbo].[Departments] WHERE [Name] = @name AND [TenantId] = @tenantId", connection, transaction);
        departmentQuery.Parameters.AddWithValue("@name", "STAFF");
        departmentQuery.Parameters.AddWithValue("@tenantId", tenantId);

        using var departmentReader = departmentQuery.ExecuteReader();
        if (departmentReader.Read())
        {
            var departmentId = departmentReader.GetValue(0);
            departmentReader.Close();
            return departmentId;
        }
        departmentReader.Close();

        // Create STAFF department if it doesn't exist
        var newDepartmentId = Guid.NewGuid().ToString();
        var insertDepartmentQuery = new SqlCommand(@"
            INSERT INTO [dbo].[Departments] 
            ([Id], [Name], [TenantId], [Created], [CreatedBy], [LastModified], [LastModifiedBy], [IsDeleted])
            VALUES 
            (@id, @name, @tenantId, SYSDATETIMEOFFSET(), 'MIGRATION', SYSDATETIMEOFFSET(), 'MIGRATION', 0)", connection, transaction);

        insertDepartmentQuery.Parameters.AddWithValue("@id", newDepartmentId);
        insertDepartmentQuery.Parameters.AddWithValue("@name", "STAFF");
        insertDepartmentQuery.Parameters.AddWithValue("@tenantId", tenantId);

        insertDepartmentQuery.ExecuteNonQuery();

        return newDepartmentId;
    }

    /// <summary>
    /// Splits a full name and returns the first part (first name).
    /// </summary>
    /// <param name="fullnameValue">The full name value.</param>
    /// <returns>The first name.</returns>
    private static object? SplitFullnameFirst(object? fullnameValue)
    {
        if (fullnameValue == null)
        {
            return null;
        }

        var fullname = fullnameValue.ToString();
        if (string.IsNullOrWhiteSpace(fullname))
        {
            return null;
        }

        var parts = fullname.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length > 0 ? parts[0] : null;
    }

    /// <summary>
    /// Splits a full name and returns the last part(s) (last name).
    /// </summary>
    /// <param name="fullnameValue">The full name value.</param>
    /// <returns>The last name.</returns>
    private static object? SplitFullnameLast(object? fullnameValue)
    {
        if (fullnameValue == null)
        {
            return null;
        }

        var fullname = fullnameValue.ToString();
        if (string.IsNullOrWhiteSpace(fullname))
        {
            return null;
        }

        var parts = fullname.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length <= 1)
        {
            return null;
        }

        return string.Join(" ", parts.Skip(1));
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
            "applicationuserid" => "NEWID()",
            "employeeid" => "NEWID()",
            "created" => "SYSDATETIMEOFFSET()",
            "createdby" => "MIGRATION",
            "lastmodified" => "SYSDATETIMEOFFSET()",
            "lastmodifiedby" => "MIGRATION",
            "isdeleted" => "0",
            "deleted" => "NULL",
            "deletedby" => "NULL",
            "address_building" => " ",
            "address_street" => " ",
            "address_city" => " ",
            "engagementstatus" => "Active",
            "startdate" => "SYSDATETIMEOFFSET()",
            "enddate" => "NULL",
            "managerid" => "NULL",
            "partnerid" => "NULL",
            "title" => "NULL",
            "description" => "NULL",
            "employeenumber" => "NULL",
            "reportsto" => "NEWID()",
            "concurrencystamp" => "NEWID()",
            "normalizedusername" => "NULL",
            "normalizedemail" => "NULL",
            "phonenumberconfirmed" => "0",
            "twofactorenabled" => "0",
            "lockoutend" => "NULL",
            "lockoutenabled" => "0",
            "accessfailedcount" => "0",
            _ when column.Type.Contains("bit") => "0",
            _ when column.Type.Contains("datetime") => "NULL",
            _ when column.Type.Contains("datetimeoffset") => "NULL",
            _ when column.Type.Contains("varchar") || column.Type.Contains("nvarchar") => "NULL",
            _ when column.Type.Contains("int") || column.Type.Contains("decimal") || column.Type.Contains("float") => "0",
            _ => "NULL"
        };
    }

    /// <summary>
    /// Gets a default JobPositionId for records with NULL JobPositionId.
    /// </summary>
    /// <param name="sourceData">The source data.</param>
    /// <param name="connection">The database connection.</param>
    /// <param name="transaction">The database transaction.</param>
    /// <returns>A default JobPositionId or null if none found.</returns>
    private static object? GetDefaultJobPositionId(MigrationSourceData sourceData, SqlConnection connection, SqlTransaction transaction)
    {
        try
        {
            var companyCode = sourceData.GetValue("CompanyCode")?.ToString();
            if (string.IsNullOrEmpty(companyCode))
            {
                MigrationLogger.LogWarning($"Cannot find default JobPositionId: CompanyCode is null or empty");
                return null;
            }

            // First try to find a "General" or "Staff" job position for the company
            var query = new SqlCommand(@"
                SELECT TOP 1 jp.Id 
                FROM [dbo].[JobPositions] jp
                INNER JOIN [dbo].[Tenants] t ON jp.TenantId = t.TenantId
                WHERE t.MigrationCompanyCode = @CompanyCode 
                  AND (jp.Title LIKE '%General%' OR jp.Title LIKE '%Staff%' OR jp.Title LIKE '%Employee%')
                ORDER BY jp.Title", connection, transaction);

            query.Parameters.AddWithValue("@CompanyCode", companyCode);

            var result = query.ExecuteScalar();
            if (result != null)
            {
                MigrationLogger.LogInfo($"Found default JobPositionId for CompanyCode {companyCode}: {result}");
                return result;
            }

            // If no specific job position found, get any job position for the company
            query = new SqlCommand(@"
                SELECT TOP 1 jp.Id 
                FROM [dbo].[JobPositions] jp
                INNER JOIN [dbo].[Tenants] t ON jp.TenantId = t.TenantId
                WHERE t.MigrationCompanyCode = @CompanyCode 
                ORDER BY jp.Title", connection, transaction);

            query.Parameters.AddWithValue("@CompanyCode", companyCode);

            result = query.ExecuteScalar();
            if (result != null)
            {
                MigrationLogger.LogInfo($"Found fallback JobPositionId for CompanyCode {companyCode}: {result}");
                return result;
            }

            MigrationLogger.LogWarning($"No JobPositionId found for CompanyCode {companyCode}");
            return null;
        }
        catch (Exception ex)
        {
            MigrationLogger.LogError($"Error getting default JobPositionId for CompanyCode: {sourceData.GetValue("CompanyCode")}", ex);
            return null;
        }
    }
}
