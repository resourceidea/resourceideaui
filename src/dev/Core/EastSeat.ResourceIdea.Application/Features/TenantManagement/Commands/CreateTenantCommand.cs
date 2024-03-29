using EastSeat.ResourceIdea.Domain.Common;
using EastSeat.ResourceIdea.Domain.Common.Responses;
using EastSeat.ResourceIdea.Domain.Tenant.Models;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.TenantManagement.Commands;

/// <summary>
/// Command to create a tenant.
/// </summary>
public sealed class CreateTenantCommand : IRequest<ResourceIdeaResponse<TenantModel>>
{
    /// <summary>Tenant's organization name. </summary>
    public string Organization { get; set; } = string.Empty;
}