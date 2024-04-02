using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.TenantManagement.Queries;
using EastSeat.ResourceIdea.Domain.Common.Responses;
using EastSeat.ResourceIdea.Domain.Entities;
using EastSeat.ResourceIdea.Domain.Tenant.Models;
using EastSeat.ResourceIdea.Domain.Tenant.ValueObjects;

using MediatR;

using Optional;

namespace EastSeat.ResourceIdea.Application.Features.TenantManagement.Handlers;

public sealed class GetTenantByIdQueryHandler(
    IAsyncRepository<Tenant> tenantRepository,
    IMapper mapper) : IRequestHandler<GetTenantByIdQuery, ResourceIdeaResponse<TenantModel>>
{
    private readonly IAsyncRepository<Tenant> _tenantRepository = tenantRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<TenantModel>> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
    {
        Option<Tenant> tenantQuery = await _tenantRepository.GetByIdAsync(request.TenantId.Value, cancellationToken);
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
