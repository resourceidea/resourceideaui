using AutoMapper;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Commands;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Specifications;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;
using EastSeat.ResourceIdea.Domain.Subscriptions.Enums;
using EastSeat.ResourceIdea.Domain.Subscriptions.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Handlers;

public sealed class CancelSubscriptionCommandHandler(
    IAsyncRepository<Subscription> subscriptionRepository,
    IMapper mapper)
    : IRequestHandler<CancelSubscriptionCommand, ResourceIdeaResponse<SubscriptionModel>>
{
    private readonly IAsyncRepository<Subscription> _subscriptionRepository = subscriptionRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<SubscriptionModel>> Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var querySpecification = new GetSubscriptionByIdSpecification(request.SubscriptionId);
        var subscriptionQueryResult = await _subscriptionRepository.GetByIdAsync(querySpecification, cancellationToken);
        if (subscriptionQueryResult.IsFailure)
        {
            return ResourceIdeaResponse<SubscriptionModel>.Failure(subscriptionQueryResult.Error);
        }

        Subscription subscription = subscriptionQueryResult.Content.Value;
        subscription.Status = SubscriptionStatus.Canceled;

        var subscriptionUpdateResult = await _subscriptionRepository.UpdateAsync(subscription, cancellationToken);
        if (subscriptionUpdateResult.IsFailure)
        {
            return ResourceIdeaResponse<SubscriptionModel>.Failure(subscriptionUpdateResult.Error);
        }

        return ResourceIdeaResponse<SubscriptionModel>
                    .Success(Optional<SubscriptionModel>.Some(_mapper.Map<SubscriptionModel>(subscriptionUpdateResult.Content.Value)));
    }
}
