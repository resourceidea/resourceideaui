using EastSeat.ResourceIdea.Domain.Common.Responses;
using EastSeat.ResourceIdea.Domain.TenantManagement.Models;
using EastSeat.ResourceIdea.Domain.TenantManagement.ValueObjects;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.TenantManagement.Commands;

/// <summary>
/// Command to update a tenant.
/// </summary>
public sealed class UpdateTenantCommand : IRequest<ResourceIdeaResponse<TenantModel>>
{
    /// <summary>Tenant's unique identifier. </summary>
    public TenantId TenantId { get; set; }

    /// <summary>Tenant's organization name. </summary>
    public string Organization { get; set; } = string.Empty;
}
