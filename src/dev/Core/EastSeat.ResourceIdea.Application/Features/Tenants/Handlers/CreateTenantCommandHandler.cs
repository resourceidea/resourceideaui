using AutoMapper;
using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Tenants.Commands;
using EastSeat.ResourceIdea.Application.Features.Tenants.Contracts;
using EastSeat.ResourceIdea.Application.Features.Tenants.Validators;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.Models;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Tenants.Handlers;

/// <summary>
/// Handles the operations required to create a tenant.
/// </summary>
public sealed class CreateTenantCommandHandler (
    ITenantsService tenantsService,
    IMapper mapper) : IRequestHandler<CreateTenantCommand, ResourceIdeaResponse<TenantModel>>
{
    private readonly ITenantsService _tenantsService = tenantsService;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<TenantModel>> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        CreateTenantCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid is false || validationResult.Errors.Count > 0)
        {
            return ResourceIdeaResponse<TenantModel>.Failure(ErrorCode.CreateTenantCommandValidationFailure);
        }

        Tenant tenant = new()
        {
            TenantId = TenantId.Create(Guid.NewGuid()).Value,
            Organization = request.Organization
        };
        var response = await _tenantsService.AddAsync(tenant, cancellationToken);
        if (response.IsFailure)
        {
            return ResourceIdeaResponse<TenantModel>.Failure(response.Error);
        }

        return _mapper.Map<ResourceIdeaResponse<TenantModel>>(response);
    }
}