using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Contracts;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Queries;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Specifications;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Models;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Handlers;

/// <summary>
/// Handles the query to get a subscription service by its unique identifier.
/// </summary>
public sealed class GetSubscriptionServiceByIdQueryHandler(ISubscriptionServicesService subscriptionServicesService)
    : BaseHandler, IRequestHandler<GetSubscriptionServiceByIdQuery, ResourceIdeaResponse<SubscriptionServiceModel>>
{
    private readonly ISubscriptionServicesService _subscriptionServicesService = subscriptionServicesService;

    /// <summary>
    /// Handles the query to get a subscription service by its unique identifier.
    /// </summary>
    /// <param name="request">The query request containing the subscription service ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response with the subscription service model.</returns>
    public async Task<ResourceIdeaResponse<SubscriptionServiceModel>> Handle(
        GetSubscriptionServiceByIdQuery request,
        CancellationToken cancellationToken)
    {
        SubscriptionServiceGetByIdSpecification querySpecification = new(request.SubscriptionServiceId);
        ResourceIdeaResponse<SubscriptionService> response = await _subscriptionServicesService.GetByIdAsync(
            querySpecification,
            cancellationToken);

        ResourceIdeaResponse<SubscriptionServiceModel> handlerResponse = GetHandlerResponse<SubscriptionService, SubscriptionServiceModel>(response, ErrorCode.NotFound);

        return handlerResponse;
    }
}
