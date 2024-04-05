using System.Runtime.CompilerServices;

using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.TenantManagement.Commands;
using EastSeat.ResourceIdea.Application.Features.TenantManagement.Validators;
using EastSeat.ResourceIdea.Domain.Common.Constants;
using EastSeat.ResourceIdea.Domain.Common.Exceptions;
using EastSeat.ResourceIdea.Domain.Common.Responses;
using EastSeat.ResourceIdea.Domain.TenantManagement.Entities;
using EastSeat.ResourceIdea.Domain.TenantManagement.Models;

using MediatR;

using Optional;

namespace EastSeat.ResourceIdea.Application.Features.TenantManagement.Handlers;

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
                Content = Option.None<TenantModel>()
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
            none: () => throw new UpdateItemNotFoundException("Update tenant to be updated was not found.")
        );

        return new ResourceIdeaResponse<TenantModel>
        {
            Success = true,
            Message = "Tenant updated successfully",
            Content = Option.Some(_mapper.Map<TenantModel>(updatedTenant))
        };
    }
}
