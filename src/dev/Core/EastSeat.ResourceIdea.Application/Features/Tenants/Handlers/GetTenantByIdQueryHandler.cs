using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Tenants.Queries;
using EastSeat.ResourceIdea.Application.Features.Tenants.Specifications;
using EastSeat.ResourceIdea.Domain.Common.Responses;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.Models;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

using MediatR;

using Optional;

namespace EastSeat.ResourceIdea.Application.Features.Tenants.Handlers;

public sealed class GetTenantByIdQueryHandler(
    IAsyncRepository<Tenant> tenantRepository,
    IMapper mapper) : IRequestHandler<GetTenantByIdQuery, ResourceIdeaResponse<TenantModel>>
{
    private readonly IAsyncRepository<Tenant> _tenantRepository = tenantRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<TenantModel>> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
    {
        var getTenantByIdSpecification = new TenantGetByIdSpecification(request.TenantId);
        Option<Tenant> tenantQuery = await _tenantRepository.GetByIdAsync(getTenantByIdSpecification, cancellationToken);
        Tenant tenant = tenantQuery.Match(
            some: tenant => tenant,
            none: () => EmptyTenant.Instance
        );

        if (tenant == EmptyTenant.Instance)
        {
            return ResourceIdeaResponse<TenantModel>.NotFound();
        }

        return new ResourceIdeaResponse<TenantModel>
        {
            Success = true,
            Content = Option.Some(_mapper.Map<TenantModel>(tenant))
        };
    }
}
