using AutoMapper;

using EastSeat.ResourceIdea.Application.Extensions;
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

public sealed class GetSubscriptionServicesListQueryHandler(
    ISubscriptionServicesService subscriptionServicesService,
    IMapper mapper) : IRequestHandler<GetSubscriptionServicesListQuery, ResourceIdeaResponse<PagedListResponse<SubscriptionServiceModel>>>
{
    private readonly ISubscriptionServicesService _subscriptionServicesService = subscriptionServicesService;
    private readonly IMapper _mapper = mapper;

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

        return _mapper.Map<ResourceIdeaResponse<PagedListResponse<SubscriptionServiceModel>>>(response);
    }

    private static BaseSpecification<SubscriptionService> GetSubscriptionServicesQuerySpecification(string queryFilters)
    {
        var filters = queryFilters.GetFiltersAsDictionary(delimiter: [';'], keyValueSeparator: ['=']);

        return new SubscriptionServiceNameSpecification(filters);
    }
}
