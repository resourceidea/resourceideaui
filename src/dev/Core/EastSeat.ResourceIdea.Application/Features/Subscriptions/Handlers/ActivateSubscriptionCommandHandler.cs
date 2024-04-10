using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Commands;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Specifications;
using EastSeat.ResourceIdea.Domain.Common.Exceptions;
using EastSeat.ResourceIdea.Domain.Common.Responses;
using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;
using EastSeat.ResourceIdea.Domain.Subscriptions.Enums;
using EastSeat.ResourceIdea.Domain.Subscriptions.Models;
using EastSeat.ResourceIdea.Domain.Subscriptions.ValueObjects;

using MediatR;

using Optional;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Handlers;

/// <summary>
/// Handles the activation of a subscription.
/// </summary>
public sealed class ActivateSubscriptionCommandHandler(
    IAsyncRepository<Subscription> subscriptionRepository,
    IMapper mapper)
    : IRequestHandler<ActivateSubscriptionCommand, ResourceIdeaResponse<SubscriptionModel>>
{
    private readonly IAsyncRepository<Subscription> _subscriptionRepository = subscriptionRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<SubscriptionModel>> Handle(ActivateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var querySpecification = new GetSubscriptionByIdSpecification(request.SubscriptionId);
        var subscriptionQueryResult = await _subscriptionRepository.GetByIdAsync(querySpecification, cancellationToken);

        Subscription subscription = subscriptionQueryResult.Match(
            some: subscription => subscription,
            none: () => EmptySubscription.Instance);

        subscription.Status = SubscriptionStatus.Active;

        var subscriptionUpdateResult = await _subscriptionRepository.UpdateAsync(subscription, cancellationToken);

        var updatedSubscription = subscriptionUpdateResult.Match(
            some: subscription => subscription,
            none: () => throw new UpdateItemNotFoundException("Subscription to update was not found."));

        return new ResourceIdeaResponse<SubscriptionModel>
        {
            Success = true,
            Content = Option.Some(_mapper.Map<SubscriptionModel>(updatedSubscription)),
            Message = "Subscription activated successfully."
        };
    }
}
