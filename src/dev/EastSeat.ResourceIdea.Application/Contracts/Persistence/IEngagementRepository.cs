using EastSeat.ResourceIdea.Domain.Entities;

namespace EastSeat.ResourceIdea.Application.Contracts.Persistence;

/// <summary>
/// Engagement repository.
/// </summary>
public interface IEngagementRepository : IAsyncRepository<Engagement>
{
}
