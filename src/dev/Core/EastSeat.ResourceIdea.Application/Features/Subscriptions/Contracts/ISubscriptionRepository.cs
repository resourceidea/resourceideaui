using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Contracts;

/// <summary>
/// Subscription repository interface.
/// </summary>
public interface ISubscriptionRepository : IAsyncRepository<Subscription> { }
