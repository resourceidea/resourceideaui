using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Contracts;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Queries;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Specifications;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;
using EastSeat.ResourceIdea.Domain.Subscriptions.Models;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Handlers;

public sealed class GetSubscriptionByIdQueryHandler(ISubscriptionsService subscriptionsService)
    : BaseHandler, IRequestHandler<GetSubscriptionByIdQuery, ResourceIdeaResponse<SubscriptionModel>>
{
    private readonly ISubscriptionsService _subscriptionsService = subscriptionsService;

    public async Task<ResourceIdeaResponse<SubscriptionModel>> Handle(GetSubscriptionByIdQuery request, CancellationToken cancellationToken)
    {
        GetSubscriptionByIdSpecification getSubscriptionByIdSpecification = new (request.SubscriptionId);
        var response = await _subscriptionsService.GetByIdAsync(getSubscriptionByIdSpecification, cancellationToken);
        var handlerResponse = GetHandlerResponse<Subscription, SubscriptionModel>(response, ErrorCode.NotFound);

        return handlerResponse;
    }
}
