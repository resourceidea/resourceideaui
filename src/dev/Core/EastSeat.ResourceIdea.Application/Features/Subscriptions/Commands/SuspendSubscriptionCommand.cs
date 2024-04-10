using EastSeat.ResourceIdea.Domain.Common.Responses;
using EastSeat.ResourceIdea.Domain.Subscriptions.Models;
using EastSeat.ResourceIdea.Domain.Subscriptions.ValueObjects;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Commands;

/// <summary>
/// Command to suspend a subscription.
/// </summary>
public sealed class SuspendSubscriptionCommand : IRequest<ResourceIdeaResponse<SubscriptionModel>>
{
    /// <summary>
    /// Id of the subscription to be suspended.
    /// </summary>
    public SubscriptionId SubscriptionId { get; set; }
}
