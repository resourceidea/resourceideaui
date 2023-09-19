using EastSeat.ResourceIdea.Domain.Entities;

namespace EastSeat.ResourceIdea.Application.Contracts.Persistence;

/// <summary>
/// Subscription repository.
/// </summary>
public interface ISubscriptionRepository : IAsyncRepository<Subscription>
{
    /// <summary>
    /// Check if the subscriber name is already in user.
    /// </summary>
    /// <param name="subscriberName">Subscriber name to check.</param>
    /// <returns>True if name if already in use; otherwise False.</returns>
    Task<bool> IsSubscriberNameAlreadyInUse(string? subscriberName);
}
