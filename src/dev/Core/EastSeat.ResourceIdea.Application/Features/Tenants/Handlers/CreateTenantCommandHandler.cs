using AutoMapper;
using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Tenants.Commands;
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
    IAsyncRepository<Tenant> tenantRepository,
    IMapper mapper)
    : IRequestHandler<CreateTenantCommand, ResourceIdeaResponse<TenantModel>>
{
    private readonly IAsyncRepository<Tenant> _tenantRepository = tenantRepository;
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
        var addTenantResult = await _tenantRepository.AddAsync(tenant, cancellationToken);
        if (addTenantResult.IsFailure)
        {
            return ResourceIdeaResponse<TenantModel>.Failure(addTenantResult.Error);
        }

        return ResourceIdeaResponse<TenantModel>.Success(Optional<TenantModel>.Some(_mapper.Map<TenantModel>(addTenantResult)));
    }
}