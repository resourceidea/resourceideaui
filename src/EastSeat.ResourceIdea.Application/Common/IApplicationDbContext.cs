namespace EastSeat.ResourceIdea.Application.Common;

/// <summary>
/// Interface for database context abstraction
/// </summary>
public interface IApplicationDbContext
{
    // DbSets will be added by Infrastructure layer
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
