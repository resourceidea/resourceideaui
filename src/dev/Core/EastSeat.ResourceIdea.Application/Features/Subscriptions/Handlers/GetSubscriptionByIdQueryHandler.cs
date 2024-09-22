using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Contracts;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Queries;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Specifications;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;
using EastSeat.ResourceIdea.Domain.Subscriptions.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Handlers;

public sealed class GetSubscriptionByIdQueryHandler(
    ISubscriptionsService subscriptionsService,
    IMapper mapper) : IRequestHandler<GetSubscriptionByIdQuery, ResourceIdeaResponse<SubscriptionModel>>
{
    private readonly ISubscriptionsService _subscriptionsService = subscriptionsService;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<SubscriptionModel>> Handle(GetSubscriptionByIdQuery request, CancellationToken cancellationToken)
    {
        GetSubscriptionByIdSpecification getSubscriptionByIdSpecification = new (request.SubscriptionId);
        var response = await _subscriptionsService.GetByIdAsync(getSubscriptionByIdSpecification, cancellationToken);

        return _mapper.Map<ResourceIdeaResponse<SubscriptionModel>>(response.Content.Value);
    }
}
