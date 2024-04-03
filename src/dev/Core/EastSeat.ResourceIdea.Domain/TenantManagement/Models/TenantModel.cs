using EastSeat.ResourceIdea.Domain.TenantManagement.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.TenantManagement.Models;

/// <summary>
/// Tenant model.
/// </summary>
public class TenantModel
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