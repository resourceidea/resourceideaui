using EastSeat.ResourceIdea.Application.Features.Tenants.Contracts;
using EastSeat.ResourceIdea.Application.Features.Tenants.Queries;
using EastSeat.ResourceIdea.Application.Features.Tenants.Specifications;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Domain.Tenants.Models;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Tenants.Handlers;

public sealed class GetTenantByIdQueryHandler(ITenantsService tenantsService)
    : IRequestHandler<GetTenantByIdQuery, ResourceIdeaResponse<TenantModel>>
{
    private readonly ITenantsService _tenantsService = tenantsService;

    public async Task<ResourceIdeaResponse<TenantModel>> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
    {
        var getTenantByIdSpecification = new TenantGetByIdSpecification(request.TenantId);
        var response = await _tenantsService.GetByIdAsync(getTenantByIdSpecification, cancellationToken);
        if (response.IsFailure)
        {
            return ResourceIdeaResponse<TenantModel>.Failure(response.Error);
        }

        if (response.Content is null)
        {
            return ResourceIdeaResponse<TenantModel>.NotFound();
        }

        return response.Content.ToResourceIdeaResponse();
    }
}
