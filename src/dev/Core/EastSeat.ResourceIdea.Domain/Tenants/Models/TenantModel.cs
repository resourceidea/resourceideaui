using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.Tenants.Models;

/// <summary>
/// Tenant model.
/// </summary>
public record TenantModel
{
    /// <summary>
    /// Tenant Id.
    /// </summary>
    public TenantId TenantId { get; set; }

    /// <summary>
    /// Tenant's organization name.
    /// </summary>
    public string Organization { get; set; } = string.Empty;
}