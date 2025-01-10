using EastSeat.ResourceIdea.Application.Extensions;
using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Contracts;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Queries;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Specifications;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Models;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Handlers;

/// <summary>
/// Handles the query to get a list of subscription services.
/// </summary>
/// <param name="subscriptionServicesService">The subscription services service.</param>
public sealed class GetSubscriptionServicesListQueryHandler(ISubscriptionServicesService subscriptionServicesService)
    : BaseHandler, IRequestHandler<GetSubscriptionServicesListQuery, ResourceIdeaResponse<PagedListResponse<SubscriptionServiceModel>>>
{
    private readonly ISubscriptionServicesService _subscriptionServicesService = subscriptionServicesService;

    /// <summary>
    /// Handles the request to get a paged list of subscription services.
    /// </summary>
    /// <param name="request">The request containing the query parameters.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A response containing a paged list of subscription service models.</returns>
    public async Task<ResourceIdeaResponse<PagedListResponse<SubscriptionServiceModel>>> Handle(
        GetSubscriptionServicesListQuery request,
        CancellationToken cancellationToken)
    {
        var querySpecification = GetSubscriptionServicesQuerySpecification(request.Filter);
        var response = await _subscriptionServicesService.GetPagedListAsync(
            request.CurrentPageNumber,
            request.PageSize,
            querySpecification,
            cancellationToken);

        var handlerResponse = GetHandlerResponse<SubscriptionService, SubscriptionServiceModel>(response);

        return handlerResponse;
    }

    /// <summary>
    /// Creates a specification for querying subscription services based on the provided filters.
    /// </summary>
    /// <param name="queryFilters">The query filters as a string.</param>
    /// <returns>A specification for querying subscription services.</returns>
    private static BaseSpecification<SubscriptionService> GetSubscriptionServicesQuerySpecification(string queryFilters)
    {
        var filters = queryFilters.GetFiltersAsDictionary(delimiter: [';'], keyValueSeparator: ['=']);

        return new SubscriptionServiceNameSpecification(filters);
    }
}
