using AutoMapper;

using EastSeat.ResourceIdea.Application.Extensions;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Queries;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Specifications;
using EastSeat.ResourceIdea.Application.Features.Tenants.Specifications;
using EastSeat.ResourceIdea.Application.Responses;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Models;

using MediatR;

using Optional;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServiceManagement.Handlers;

public sealed class GetSubscriptionServicesListQueryHandler(
    IAsyncRepository<SubscriptionService> subscriptionServiceRepository,
    IMapper mapper)
    : IRequestHandler<GetSubscriptionServicesListQuery, ResourceIdeaResponse<PagedListResponse<SubscriptionServiceModel>>>
{
    private readonly IAsyncRepository<SubscriptionService> _subscriptionServiceRepository = subscriptionServiceRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<PagedListResponse<SubscriptionServiceModel>>> Handle(GetSubscriptionServicesListQuery request, CancellationToken cancellationToken)
    {
        var specification = GetSubscriptionServicesQuerySpecification(request.Filter);

        PagedListResponse<SubscriptionService> subscriptionServices = await _subscriptionServiceRepository.GetPagedListAsync(
            request.CurrentPageNumber,
            request.PageSize,
            specification,
            cancellationToken);

        return new ResourceIdeaResponse<PagedListResponse<SubscriptionServiceModel>>
        {
            Success = true,
            Content = Option.Some(_mapper.Map<PagedListResponse<SubscriptionServiceModel>>(subscriptionServices))
        };
    }

    private static BaseSpecification<SubscriptionService> GetSubscriptionServicesQuerySpecification(string queryFilters)
    {
        var filters = queryFilters.GetFiltersAsDictionary(delimiter: [';'], keyValueSeparator: ['=']);

        return new SubscriptionServiceNameSpecification(filters);
    }
}
