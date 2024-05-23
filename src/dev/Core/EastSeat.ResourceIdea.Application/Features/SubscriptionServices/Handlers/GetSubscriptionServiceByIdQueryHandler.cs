using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Queries;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Specifications;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Models;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.ValueObjects;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServiceManagement.Handlers;

public sealed class GetSubscriptionServiceByIdQueryHandler(
    IAsyncRepository<SubscriptionService> subscriptionServiceRepository,
    IMapper mapper) : IRequestHandler<GetSubscriptionServiceByIdQuery, ResourceIdeaResponse<SubscriptionServiceModel>>
{
    private readonly IAsyncRepository<SubscriptionService> _subscriptionServiceRepository = subscriptionServiceRepository;
    private readonly IMapper _mapper = mapper;
    public async Task<ResourceIdeaResponse<SubscriptionServiceModel>> Handle(
        GetSubscriptionServiceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var getSubscriptionServiceByIdSpecification = new SubscriptionServiceGetByIdSpecification(request.SubscriptionServiceId);
        Optional<SubscriptionService> subscriptionServiceQuery = await _subscriptionServiceRepository.GetByIdAsync(
            getSubscriptionServiceByIdSpecification,
            cancellationToken);

        SubscriptionService subscriptionService = subscriptionServiceQuery.Match(
            some: subscriptionService => subscriptionService,
            none: () => EmptySubscriptionService.Instance
        );

        if (subscriptionService == EmptySubscriptionService.Instance)
        {
            return ResourceIdeaResponse<SubscriptionServiceModel>.NotFound();
        }

        return new ResourceIdeaResponse<SubscriptionServiceModel>
        {
            Success = true,
            Content = Optional<SubscriptionServiceModel>.Some(_mapper.Map<SubscriptionServiceModel>(subscriptionService))
        };
    }
}
