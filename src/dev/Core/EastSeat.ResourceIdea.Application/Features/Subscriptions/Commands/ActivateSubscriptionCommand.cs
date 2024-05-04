﻿using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Subscriptions.Models;
using EastSeat.ResourceIdea.Domain.Subscriptions.ValueObjects;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Commands;

/// <summary>
/// Command to activate the subscription.
/// </summary>
public sealed class ActivateSubscriptionCommand : IRequest<ResourceIdeaResponse<SubscriptionModel>>
{
    /// <summary>
    /// Id of the subscription to activate.
    /// </summary>
    public SubscriptionId SubscriptionId { get; set; }
}
