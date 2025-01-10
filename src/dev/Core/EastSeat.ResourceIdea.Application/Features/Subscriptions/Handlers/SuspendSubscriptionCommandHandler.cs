using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Commands;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Contracts;
using EastSeat.ResourceIdea.Domain.Subscriptions.Models;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Handlers;

/// <summary>
/// Handles the command to suspend a subscription.
/// </summary>
public sealed class SuspendSubscriptionCommandHandler(ISubscriptionsService subscriptionsServices)
    : BaseHandler, IRequestHandler<SuspendSubscriptionCommand, ResourceIdeaResponse<SubscriptionModel>>
{
    private readonly ISubscriptionsService _subscriptionsService = subscriptionsServices;

    public Task<ResourceIdeaResponse<SubscriptionModel>> Handle(SuspendSubscriptionCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
