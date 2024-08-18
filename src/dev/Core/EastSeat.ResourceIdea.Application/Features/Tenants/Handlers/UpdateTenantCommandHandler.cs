using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Tenants.Commands;
using EastSeat.ResourceIdea.Application.Features.Tenants.Validators;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.Models;

using MediatR;
using EastSeat.ResourceIdea.Application.Enums;
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
        if (validationResult.IsValid is false || validationResult.Errors.Count > 0)
        {
            return ResourceIdeaResponse<TenantModel>.Failure(ErrorCode.UpdateClientCommandValidationFailure);
        }

        Tenant tenantUpdateDetails = new()
        {
            TenantId = request.TenantId.Value,
            Organization = request.Organization
        };
        var updateTenantResult = await _tenantRepository.UpdateAsync(tenantUpdateDetails, cancellationToken);
        if (updateTenantResult.IsFailure)
        {
            return ResourceIdeaResponse<TenantModel>.Failure(updateTenantResult.Error);
        }

        return ResourceIdeaResponse<TenantModel>
                    .Success(Optional<TenantModel>.Some(_mapper.Map<TenantModel>(updateTenantResult.Content.Value)));
    }
}
