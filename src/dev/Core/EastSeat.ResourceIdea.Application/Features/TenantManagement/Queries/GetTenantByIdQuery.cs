using EastSeat.ResourceIdea.Domain.Common.Responses;
using EastSeat.ResourceIdea.Domain.Tenant.Models;
using EastSeat.ResourceIdea.Domain.Tenant.ValueObjects;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.TenantManagement.Queries;

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