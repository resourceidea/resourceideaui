using EastSeat.ResourceIdea.Domain.Entities;

namespace EastSeat.ResourceIdea.Application.Contracts.Persistence;

/// <summary>
/// Subscription repository.
/// </summary>
public interface ISubscriptionRepository : IAsyncRepository<Subscription>
{
}
