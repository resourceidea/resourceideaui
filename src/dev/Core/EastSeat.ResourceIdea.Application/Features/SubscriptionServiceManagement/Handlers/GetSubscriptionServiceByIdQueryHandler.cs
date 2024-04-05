using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServiceManagement.Queries;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServiceManagement.Specifications;
using EastSeat.ResourceIdea.Application.Features.TenantManagement.Contracts;
using EastSeat.ResourceIdea.Domain.Common.Responses;
using EastSeat.ResourceIdea.Domain.SubscriptionServiceManagement.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServiceManagement.Models;
using EastSeat.ResourceIdea.Domain.SubscriptionServiceManagement.ValueObjects;
using EastSeat.ResourceIdea.Domain.TenantManagement.Entities;
using EastSeat.ResourceIdea.Domain.TenantManagement.Models;
using EastSeat.ResourceIdea.Domain.TenantManagement.ValueObjects;

using MediatR;

using Optional;

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
        Option<SubscriptionService> subscriptionServiceQuery = await _subscriptionServiceRepository.GetByIdAsync(
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
            Content = Option.Some(_mapper.Map<SubscriptionServiceModel>(subscriptionService))
        };
    }
}
