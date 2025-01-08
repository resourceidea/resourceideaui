using EastSeat.ResourceIdea.Domain.SubscriptionServices.Models;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Queries;

/// <summary>
/// Query to get a subscription service by its unique identifier.
/// </summary>
public sealed class GetSubscriptionServiceByIdQuery : IRequest<ResourceIdeaResponse<SubscriptionServiceModel>>
{
    /// <summary>
    /// Subscription service ID.
    /// </summary>
    public SubscriptionServiceId SubscriptionServiceId { get; set; }
}