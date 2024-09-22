using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Contracts;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Queries;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Specifications;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Handlers;

public sealed class GetSubscriptionServiceByIdQueryHandler(
    ISubscriptionServicesService subscriptionServicesService,
    IMapper mapper) : IRequestHandler<GetSubscriptionServiceByIdQuery, ResourceIdeaResponse<SubscriptionServiceModel>>
{
    private readonly ISubscriptionServicesService _subscriptionServicesService = subscriptionServicesService;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<SubscriptionServiceModel>> Handle(
        GetSubscriptionServiceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var querySpecification = new SubscriptionServiceGetByIdSpecification(request.SubscriptionServiceId);
        var response = await _subscriptionServicesService.GetByIdAsync(
            querySpecification,
            cancellationToken);

        return _mapper.Map<ResourceIdeaResponse<SubscriptionServiceModel>>(response);
    }
}
