using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Tenants.Queries;
using EastSeat.ResourceIdea.Application.Features.Tenants.Specifications;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.Models;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

using MediatR;

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
        var getTenantByIdResult = await _tenantRepository.GetByIdAsync(getTenantByIdSpecification, cancellationToken);
        if (getTenantByIdResult.IsFailure)
        {
            return ResourceIdeaResponse<TenantModel>.Failure(getTenantByIdResult.Error);
        }

        return ResourceIdeaResponse<TenantModel>
                    .Success(Optional<TenantModel>.Some(_mapper.Map<TenantModel>(getTenantByIdResult.Content.Value)));
    }
}
