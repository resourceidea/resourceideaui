using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Subscriptions.Models;
using EastSeat.ResourceIdea.Domain.Subscriptions.ValueObjects;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Queries;

/// <summary>
/// Query to get a subscription by Id.
/// </summary>
public sealed class GetSubscriptionByIdQuery : IRequest<ResourceIdeaResponse<SubscriptionModel>>
{
    /// <summary>Subscription Id.</summary>
    public SubscriptionId SubscriptionId { get; set; }
}
