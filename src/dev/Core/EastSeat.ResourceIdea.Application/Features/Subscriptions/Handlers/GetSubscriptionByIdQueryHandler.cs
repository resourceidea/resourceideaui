﻿using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Queries;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Specifications;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;
using EastSeat.ResourceIdea.Domain.Subscriptions.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Handlers;

public sealed class GetSubscriptionByIdQueryHandler(
    IAsyncRepository<Subscription> subscriptionRepository,
    IMapper mapper) : IRequestHandler<GetSubscriptionByIdQuery, ResourceIdeaResponse<SubscriptionModel>>
{
    private readonly IAsyncRepository<Subscription> _subscriptionRepository = subscriptionRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<SubscriptionModel>> Handle(GetSubscriptionByIdQuery request, CancellationToken cancellationToken)
    {
        GetSubscriptionByIdSpecification getSubscriptionByIdSpecification = new (request.SubscriptionId);
        var getSubscriptionByIdResult = await _subscriptionRepository.GetByIdAsync(getSubscriptionByIdSpecification, cancellationToken);
        if (getSubscriptionByIdResult.IsFailure)
        {
            return ResourceIdeaResponse<SubscriptionModel>.Failure(getSubscriptionByIdResult.Error);
        }

        return ResourceIdeaResponse<SubscriptionModel>
                    .Success(Optional<SubscriptionModel>.Some(_mapper.Map<SubscriptionModel>(getSubscriptionByIdResult.Content.Value)));
    }
}
