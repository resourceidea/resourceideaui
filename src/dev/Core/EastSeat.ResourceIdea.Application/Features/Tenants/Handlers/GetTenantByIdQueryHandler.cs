using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Tenants.Contracts;
using EastSeat.ResourceIdea.Application.Features.Tenants.Queries;
using EastSeat.ResourceIdea.Application.Features.Tenants.Specifications;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Tenants.Handlers;

public sealed class GetTenantByIdQueryHandler(
    ITenantsService tenantsService,
    IMapper mapper) : IRequestHandler<GetTenantByIdQuery, ResourceIdeaResponse<TenantModel>>
{
    private readonly ITenantsService _tenantsService = tenantsService;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<TenantModel>> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
    {
        var getTenantByIdSpecification = new TenantGetByIdSpecification(request.TenantId);
        var response = await _tenantsService.GetByIdAsync(getTenantByIdSpecification, cancellationToken);

        return _mapper.Map<ResourceIdeaResponse<TenantModel>>(response);
    }
}
