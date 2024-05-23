using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Tenants.Commands;
using EastSeat.ResourceIdea.Application.Features.Tenants.Validators;
using EastSeat.ResourceIdea.Application.Constants;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.Models;

using MediatR;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.Tenants.Handlers;

public sealed class UpdateTenantCommandHandler(
    IAsyncRepository<Tenant> tenantRepository,
    IMapper mapper)
    : IRequestHandler<UpdateTenantCommand, ResourceIdeaResponse<TenantModel>>
{
    private readonly IAsyncRepository<Tenant> _tenantRepository = tenantRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<TenantModel>> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
    {
        UpdateTenantCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid || validationResult.Errors.Count > 0)
        {
            return new ResourceIdeaResponse<TenantModel>
            {
                Success = false,
                Message = "Update tenant command validation failed",
                ErrorCode = ErrorCodes.UpdateTenantCommandValidationFailure.ToString(),
                Content = Optional<TenantModel>.None
            };
        }

        Tenant tenantUpdateDetails = new()
        {
            TenantId = request.TenantId.Value,
            Organization = request.Organization
        };

        var tenantUpdateResult = await _tenantRepository.UpdateAsync(tenantUpdateDetails, cancellationToken);
        Tenant updatedTenant = tenantUpdateResult.Match(
            some: tenant => tenant,
            none: () => EmptyTenant.Instance);

        return new ResourceIdeaResponse<TenantModel>
        {
            Success = true,
            Message = "Tenant updated successfully",
            Content = Optional<TenantModel>.Some(_mapper.Map<TenantModel>(updatedTenant))
        };
    }
}
