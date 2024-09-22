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
/// Handles the activation of a subscription.
/// </summary>
public sealed class ActivateSubscriptionCommandHandler(
    ISubscriptionsService subscriptionService,
    IMapper mapper) : IRequestHandler<ActivateSubscriptionCommand, ResourceIdeaResponse<SubscriptionModel>>
{
    private readonly ISubscriptionsService _subscriptionService = subscriptionService;
    private readonly IMapper _mapper = mapper;

    public Task<ResourceIdeaResponse<SubscriptionModel>> Handle(ActivateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
