namespace EastSeat.ResourceIdea.Migration.Services;

/// <summary>
/// Service interface for managing database connection strings.
/// </summary>
public interface IConnectionStringService
{
    /// <summary>
    /// Gets the source database connection string.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The source database connection string.</returns>
    Task<string> GetSourceConnectionStringAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the destination database connection string.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The destination database connection string.</returns>
    Task<string> GetDestinationConnectionStringAsync(CancellationToken cancellationToken = default);
}
