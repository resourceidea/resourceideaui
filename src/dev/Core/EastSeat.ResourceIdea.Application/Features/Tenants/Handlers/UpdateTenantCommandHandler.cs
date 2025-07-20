using EastSeat.ResourceIdea.Application.Features.Tenants.Commands;
using EastSeat.ResourceIdea.Application.Features.Tenants.Validators;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.Models;

using MediatR;
using EastSeat.ResourceIdea.Application.Features.Tenants.Contracts;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.Enums;
namespace EastSeat.ResourceIdea.Application.Features.Tenants.Handlers;

public sealed class UpdateTenantCommandHandler(
    ITenantsService tenantsService,
    IAuthenticationContext authenticationContext)
    : IRequestHandler<UpdateTenantCommand, ResourceIdeaResponse<TenantModel>>
{
    private readonly ITenantsService _tenantsService = tenantsService;
    private readonly IAuthenticationContext _authenticationContext = authenticationContext;

    public async Task<ResourceIdeaResponse<TenantModel>> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
    {
        UpdateTenantCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid is false || validationResult.Errors.Count > 0)
        {
            return ResourceIdeaResponse<TenantModel>.Failure(ErrorCode.UpdateClientCommandValidationFailure);
        }

        Tenant tenantUpdateDetails = request.ToEntity();
        tenantUpdateDetails.TenantId = _authenticationContext.TenantId;
        var response = await _tenantsService.UpdateAsync(tenantUpdateDetails, cancellationToken);

        if (response.IsFailure)
        {
            return ResourceIdeaResponse<TenantModel>.Failure(response.Error);
        }

        if (response.Content.HasValue is false)
        {
            return ResourceIdeaResponse<TenantModel>.Failure(ErrorCode.EmptyEntityOnUpdateTenant);
        }

        return response.Content.Value.ToResourceIdeaResponse();
    }
}
