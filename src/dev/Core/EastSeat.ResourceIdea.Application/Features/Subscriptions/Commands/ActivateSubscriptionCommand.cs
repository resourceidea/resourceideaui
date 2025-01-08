using EastSeat.ResourceIdea.Domain.Subscriptions.Models;
using EastSeat.ResourceIdea.Domain.Subscriptions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Commands;

/// <summary>
/// Command to activate the subscription.
/// </summary>
public sealed class ActivateSubscriptionCommand : IRequest<ResourceIdeaResponse<SubscriptionModel>>
{
    /// <summary>
    /// DepartmentId of the subscription to activate.
    /// </summary>
    public SubscriptionId SubscriptionId { get; set; }
}
