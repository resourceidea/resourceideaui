using AutoMapper;

using EastSeat.ResourceIdea.Application.Extensions;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Contracts;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Queries;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Specifications;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;
using EastSeat.ResourceIdea.Domain.Subscriptions.Models;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Handlers;

/// <summary>
/// Handles the query to get a list of subscriptions.
/// </summary>
/// <param name="subscriptionsService">Subscriptions repository.</param>
/// <param name="mapper"><see cref="IMapper"/> for the <see cref="SubscriptionModel"/></param>
public sealed class GetSubscriptionsListQueryHandler(
    ISubscriptionsService subscriptionsService,
    IMapper mapper) : IRequestHandler<GetSubscriptionsListQuery, ResourceIdeaResponse<PagedListResponse<SubscriptionModel>>>
{
    private readonly ISubscriptionsService _subscriptionsService = subscriptionsService;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<PagedListResponse<SubscriptionModel>>> Handle(
        GetSubscriptionsListQuery request,
        CancellationToken cancellationToken)
    {
        var periodSpecification = GetSubscriptionBySubscriptionBeforeDateSpecification(request.Query.Filter)
            .And(GetSubscriptionBySubscriptionAfterDateSpecification(request.Query.Filter));

        BaseSpecification<Subscription> specification = GetSubscriptionServiceSpecification(request.Query.Filter)
            .Or(GetSubscriptionStatusSpecification(request.Query.Filter))
            .Or(GetSubscriptionTypeSpecification(request.Query.Filter))
            .Or(GetSubscriptionBySubscribedOnDateSpecification(request.Query.Filter))
            .Or(periodSpecification);

        var response = await _subscriptionsService.GetPagedListAsync(
            request.Query.PageNumber,
            request.Query.PageSize,
            specification,
            cancellationToken);

        return _mapper.Map<ResourceIdeaResponse<PagedListResponse<SubscriptionModel>>>(response);
    }

    private static BaseSpecification<Subscription> GetSubscriptionBySubscriptionAfterDateSpecification(string queryFilters)
    {
        var filters = queryFilters.GetFiltersAsDictionary(delimiter: [';'], keyValueSeparator: ['=']);

        return new GetSubscriptionBySubscribedAfterDateSpecification(filters);
    }

    private static BaseSpecification<Subscription> GetSubscriptionBySubscriptionBeforeDateSpecification(string queryFilters)
    {
        var filters = queryFilters.GetFiltersAsDictionary(delimiter: [';'], keyValueSeparator: ['=']);

        return new GetSubscriptionBySubscribedBeforeDateSpecification(filters);
    }

    private static BaseSpecification<Subscription> GetSubscriptionBySubscribedOnDateSpecification(string queryFilters)
    {
        var filters = queryFilters.GetFiltersAsDictionary(delimiter: [';'], keyValueSeparator: ['=']);

        return new GetSubscriptionBySubscribedOnDateSpecification(filters);
    }

    private static BaseSpecification<Subscription> GetSubscriptionTypeSpecification(string queryFilters)
    {
        var filters = queryFilters.GetFiltersAsDictionary(delimiter: [';'], keyValueSeparator: ['=']);

        return new GetSubscriptionByTypeSpecification(filters);
    }

    private static BaseSpecification<Subscription> GetSubscriptionServiceSpecification(string queryFilters)
    {
        var filters = queryFilters.GetFiltersAsDictionary(delimiter: [';'], keyValueSeparator: ['=']);

        return new GetSubscriptionBySubscriptionServiceIdSpecification(filters);
    }

    private static BaseSpecification<Subscription> GetSubscriptionStatusSpecification(string queryFilters)
    {
        var filters = queryFilters.GetFiltersAsDictionary(delimiter: [';'], keyValueSeparator: ['=']);

        return new GetSubscriptionByStatusSpecification(filters);
    }
}
