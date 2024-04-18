using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Tenants.Commands;
using EastSeat.ResourceIdea.Application.Features.Tenants.Validators;
using EastSeat.ResourceIdea.Domain.Common.Constants;
using EastSeat.ResourceIdea.Domain.Common.Responses;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.Models;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

using MediatR;

using Optional;

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
        if (validationResult.IsValid || validationResult.Errors.Count > 0)
        {
            return new ResourceIdeaResponse<TenantModel>
            {
                Success = false,
                Message = "Create tenant command validation failed",
                ErrorCode = ErrorCodes.CreateTenantCommandValidationFailure.ToString(),
                Content = Option.None<TenantModel>()
            };
        }

        Tenant tenant = new()
        {
            TenantId = TenantId.Create(Guid.NewGuid()).Value,
            Organization = request.Organization
        };

        Tenant newTenant = await _tenantRepository.AddAsync(tenant, cancellationToken);

        return new ResourceIdeaResponse<TenantModel>
        {
            Success = true,
            Message = "Tenant created successfully",
            Content = Option.Some(_mapper.Map<TenantModel>(newTenant))
        };
    }
}