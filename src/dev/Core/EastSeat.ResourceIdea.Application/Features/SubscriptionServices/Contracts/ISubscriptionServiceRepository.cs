using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Contracts;

/// <summary>
/// Queries and updates data about the services that are subscribed to.
/// </summary>
public interface ISubscriptionServiceRepository : IAsyncRepository<SubscriptionService> { }
