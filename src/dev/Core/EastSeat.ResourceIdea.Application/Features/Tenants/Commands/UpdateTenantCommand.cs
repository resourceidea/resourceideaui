// ----------------------------------------------------------------------------------
// File: UpdateTenantCommand.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Tenants\Commands\UpdateTenantCommand.cs
// Description: Command to update a tenant.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Domain.Tenants.Models;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Tenants.Commands;

/// <summary>
/// Command to update a tenant.
/// </summary>
public sealed class UpdateTenantCommand : IRequest<ResourceIdeaResponse<TenantModel>>
{
    /// <summary>Tenant's organization name. </summary>
    public string Organization { get; set; } = string.Empty;
}
