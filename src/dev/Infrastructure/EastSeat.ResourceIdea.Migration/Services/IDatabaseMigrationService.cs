using EastSeat.ResourceIdea.Migration.Models;

namespace EastSeat.ResourceIdea.Migration.Services;

/// <summary>
/// Service interface for database migration operations.
/// </summary>
public interface IDatabaseMigrationService
{
    /// <summary>
    /// Migrates data for a specific table from source to destination database.
    /// </summary>
    /// <param name="tableName">Name of the table to migrate.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Migration result.</returns>
    Task<MigrationResult> MigrateTableAsync(string tableName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Migrates data for all tables from source to destination database.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Migration result.</returns>
    Task<MigrationResult> MigrateAllTablesAsync(CancellationToken cancellationToken = default);
}
