using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Subscriptions.Commands;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Contracts;
using EastSeat.ResourceIdea.Domain.Subscriptions.Models;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Handlers;

public sealed class CancelSubscriptionCommandHandler(
    ISubscriptionsService subscriptionService,
    IMapper mapper) : IRequestHandler<CancelSubscriptionCommand, ResourceIdeaResponse<SubscriptionModel>>
{
    private readonly ISubscriptionsService _subscriptionService = subscriptionService;
    private readonly IMapper _mapper = mapper;

    public Task<ResourceIdeaResponse<SubscriptionModel>> Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
