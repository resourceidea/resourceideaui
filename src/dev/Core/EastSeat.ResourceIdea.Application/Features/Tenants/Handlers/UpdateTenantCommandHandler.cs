using AutoMapper;
using EastSeat.ResourceIdea.Application.Features.Tenants.Commands;
using EastSeat.ResourceIdea.Application.Features.Tenants.Validators;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.Models;

using MediatR;
using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.Tenants.Contracts;
namespace EastSeat.ResourceIdea.Application.Features.Tenants.Handlers;

public sealed class UpdateTenantCommandHandler(
    ITenantsService tenantsService,
    IMapper mapper) : IRequestHandler<UpdateTenantCommand, ResourceIdeaResponse<TenantModel>>
{
    private readonly ITenantsService _tenantsService = tenantsService;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<TenantModel>> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
    {
        UpdateTenantCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid is false || validationResult.Errors.Count > 0)
        {
            return ResourceIdeaResponse<TenantModel>.Failure(ErrorCode.UpdateClientCommandValidationFailure);
        }

        Tenant tenantUpdateDetails = new()
        {
            TenantId = request.TenantId.Value,
            Organization = request.Organization
        };
        var response = await _tenantsService.UpdateAsync(tenantUpdateDetails, cancellationToken);

        return _mapper.Map<ResourceIdeaResponse<TenantModel>>(response);
    }
}
