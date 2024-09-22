using AutoMapper;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Commands;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Contracts;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Specifications;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;
using EastSeat.ResourceIdea.Domain.Subscriptions.Enums;
using EastSeat.ResourceIdea.Domain.Subscriptions.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Handlers;

/// <summary>
/// Handles the command to suspend a subscription.
/// </summary>
public sealed class SuspendSubscriptionCommandHandler(
    ISubscriptionsService subscriptionsServices,
    IMapper mapper) : IRequestHandler<SuspendSubscriptionCommand, ResourceIdeaResponse<SubscriptionModel>>
{
    private readonly ISubscriptionsService _subscriptionsService = subscriptionsServices;
    private readonly IMapper _mapper = mapper;

    public Task<ResourceIdeaResponse<SubscriptionModel>> Handle(SuspendSubscriptionCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
