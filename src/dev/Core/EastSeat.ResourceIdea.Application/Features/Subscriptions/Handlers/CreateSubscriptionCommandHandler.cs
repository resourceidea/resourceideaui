using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Commands;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Contracts;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;
using EastSeat.ResourceIdea.Domain.Subscriptions.Enums;
using EastSeat.ResourceIdea.Domain.Subscriptions.Models;
using EastSeat.ResourceIdea.Domain.Subscriptions.ValueObjects;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Handlers;

public sealed class CreateSubscriptionCommandHandler(
    ISubscriptionsService subscriptionService,
    IMapper mapper) : IRequestHandler<CreateSubscriptionCommand, ResourceIdeaResponse<SubscriptionModel>>
{
    private readonly ISubscriptionsService _subscriptionService = subscriptionService;
    private readonly IMapper _mapper = mapper;

    public Task<ResourceIdeaResponse<SubscriptionModel>> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // TODO: Implement CreateSubscriptionCommandHandler.
        throw new NotImplementedException();
    }
}