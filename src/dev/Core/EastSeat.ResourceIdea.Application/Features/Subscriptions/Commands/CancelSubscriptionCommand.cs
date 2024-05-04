using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Subscriptions.Models;
using EastSeat.ResourceIdea.Domain.Subscriptions.ValueObjects;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Commands;

/// <summary>
/// Command to cancel subscription.
/// </summary>
public sealed class CancelSubscriptionCommand : IRequest<ResourceIdeaResponse<SubscriptionModel>>
{
    /// <summary>
    /// Id of the subcription to be canceled.
    /// </summary>
    public SubscriptionId SubscriptionId { get; set; }
}
