using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Tenants.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Tenants.Commands;

/// <summary>
/// Command to create a tenant.
/// </summary>
public sealed class CreateTenantCommand : IRequest<ResourceIdeaResponse<TenantModel>>
{
    /// <summary>Tenant's organization name. </summary>
    public string Organization { get; set; } = string.Empty;
}