using AutoMapper;

using EastSeat.ResourceIdea.Application.Constants;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Commands;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Specifications;
using EastSeat.ResourceIdea.Application.Responses;
using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;
using EastSeat.ResourceIdea.Domain.Subscriptions.Enums;
using EastSeat.ResourceIdea.Domain.Subscriptions.Models;
using EastSeat.ResourceIdea.Domain.Subscriptions.ValueObjects;

using MediatR;

using Optional;

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

        Subscription subscription = subscriptionQueryResult.Match(
            some: subscription => subscription,
            none: () => EmptySubscription.Instance);

        if (subscription == EmptySubscription.Instance)
        {
            return new ResourceIdeaResponse<SubscriptionModel>
            {
                Success = false,
                Message = "Subscription to cancel was not found.",
                ErrorCode = ErrorCodes.ItemNotFound.ToString()
            };
        }

        subscription.Status = SubscriptionStatus.Canceled;

        var subscriptionUpdateResult = await _subscriptionRepository.UpdateAsync(subscription, cancellationToken);

        var updatedSubscription = subscriptionUpdateResult.Match(
            some: subscription => subscription,
            none: () => EmptySubscription.Instance);

        if (updatedSubscription == EmptySubscription.Instance)
        {
            return new ResourceIdeaResponse<SubscriptionModel>
            {
                Success = false,
                Message = "Subscription cancelation failed.",
                ErrorCode = ErrorCodes.SubscriptionCancelationFailure.ToString()
            };
        }

        return new ResourceIdeaResponse<SubscriptionModel>
        {
            Success = true,
            Content = Option.Some(_mapper.Map<SubscriptionModel>(updatedSubscription)),
            Message = "Subscription activated successfully."
        };
    }
}
