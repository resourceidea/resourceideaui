using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Queries;
using EastSeat.ResourceIdea.Domain.Common.Responses;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Models;

using MediatR;

using Optional;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServiceManagement.Handlers;

public sealed class GetSubscriptionServicesListQueryHandler(
    IAsyncRepository<SubscriptionService> subscriptionServiceRepository,
    IMapper mapper)
    : IRequestHandler<GetSubscriptionServicesListQuery, ResourceIdeaResponse<PagedList<SubscriptionServiceModel>>>
{
    private readonly IAsyncRepository<SubscriptionService> _subscriptionServiceRepository = subscriptionServiceRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<PagedList<SubscriptionServiceModel>>> Handle(GetSubscriptionServicesListQuery request, CancellationToken cancellationToken)
    {
        var specification = GetSubscriptionServicesQuerySpecification(request.Filter);

        PagedList<SubscriptionService> subscriptionServices = await _subscriptionServiceRepository.GetPagedListAsync(
            request.CurrentPageNumber,
            request.PageSize,
            specification,
            cancellationToken);

        return new ResourceIdeaResponse<PagedList<SubscriptionServiceModel>>
        {
            Success = true,
            Content = Option.Some(_mapper.Map<PagedList<SubscriptionServiceModel>>(subscriptionServices))
        };
    }

    private static BaseSpecification<SubscriptionService> GetSubscriptionServicesQuerySpecification(string filter)
    {
        var subscriptionServiceSpecification = new NoFilterSpecification<SubscriptionService>();
        if (!string.IsNullOrEmpty(filter))
        {
            ; // If query filter is not empty, then create and add new specification before returning. 
        }

        return subscriptionServiceSpecification;
    }
}
