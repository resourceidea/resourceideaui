using EastSeat.ResourceIdea.Domain.Tenants.Models;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Tenants.Queries;

/// <summary>
/// Query to get a tenant by its unique identifier.
/// </summary>
public sealed class GetTenantByIdQuery : IRequest<ResourceIdeaResponse<TenantModel>>
{
    /// <summary>
    /// Tenant ID.
    /// </summary>
    public TenantId TenantId { get; set; }
}