using EastSeat.ResourceIdea.Application.Features.Tenants.Commands;
using EastSeat.ResourceIdea.Application.Features.Tenants.Contracts;
using EastSeat.ResourceIdea.Application.Features.Tenants.Validators;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.Models;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Tenants.Handlers;

/// <summary>
/// Handles the operations required to create a tenant.
/// </summary>
public sealed class CreateTenantCommandHandler (ITenantsService tenantsService)
    : IRequestHandler<CreateTenantCommand, ResourceIdeaResponse<TenantModel>>
{
    private readonly ITenantsService _tenantsService = tenantsService;

    public async Task<ResourceIdeaResponse<TenantModel>> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        CreateTenantCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid is false || validationResult.Errors.Count > 0)
        {
            return ResourceIdeaResponse<TenantModel>.Failure(ErrorCode.CreateTenantCommandValidationFailure);
        }

        Tenant tenant = request.ToEntity();
        var response = await _tenantsService.AddAsync(tenant, cancellationToken);
        if (response.IsFailure)
        {
            return ResourceIdeaResponse<TenantModel>.Failure(response.Error);
        }

        if (response.Content != null is false)
        {
            return ResourceIdeaResponse<TenantModel>.Failure(ErrorCode.EmptyEntityOnCreateTenant);
        }

        return response.Content.ToResourceIdeaResponse();
    }
}