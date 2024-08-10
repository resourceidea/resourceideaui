using AutoMapper;
using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Commands;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Specifications;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;
using EastSeat.ResourceIdea.Domain.Subscriptions.Enums;
using EastSeat.ResourceIdea.Domain.Subscriptions.Models;
using EastSeat.ResourceIdea.Domain.Subscriptions.ValueObjects;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Handlers;

/// <summary>
/// Handles the command to suspend a subscription.
/// </summary>
public sealed class SuspendSubscriptionCommandHandler(
    IAsyncRepository<Subscription> subscriptionRepository,
    IMapper mapper)
    : IRequestHandler<SuspendSubscriptionCommand, ResourceIdeaResponse<SubscriptionModel>>
{
    private readonly IAsyncRepository<Subscription> _subscriptionRepository = subscriptionRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<SubscriptionModel>> Handle(SuspendSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var querySpecification = new GetSubscriptionByIdSpecification(request.SubscriptionId);
        var subscriptionQueryResult = await _subscriptionRepository.GetByIdAsync(querySpecification, cancellationToken);

        Subscription subscription = subscriptionQueryResult.Match(
            some: subscription => subscription,
            none: () => EmptySubscription.Instance);

        if (subscription == EmptySubscription.Instance)
        {
            return new ResourceIdeaResponse<SubscriptionModel>
            {
                Success = false,
                Message = "Subscription to suspend was not found.",
                ErrorCode = ErrorCode.ItemNotFound.ToString()
            };
        }

        subscription.Status = SubscriptionStatus.Suspended;

        var subscriptionUpdateResult = await _subscriptionRepository.UpdateAsync(subscription, cancellationToken);

        var updatedSubscription = subscriptionUpdateResult.Match(
            some: subscription => subscription,
            none: () => EmptySubscription.Instance);

        if (updatedSubscription == EmptySubscription.Instance)
        {
            return new ResourceIdeaResponse<SubscriptionModel>
            {
                Success = false,
                Message = "Subscription suspension failed.",
                ErrorCode = ErrorCode.SubscriptionSuspensionFailure.ToString()
            };
        }

        return new ResourceIdeaResponse<SubscriptionModel>
        {
            Success = true,
            Content = Optional<SubscriptionModel>.Some(_mapper.Map<SubscriptionModel>(updatedSubscription)),
            Message = "Subscription activated successfully."
        };
    }
}
