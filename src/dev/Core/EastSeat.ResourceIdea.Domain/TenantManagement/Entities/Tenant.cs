using EastSeat.ResourceIdea.Domain.Common.Entities;

namespace EastSeat.ResourceIdea.Domain.TenantManagement.Entities;

/// <summary>
/// Tenant entity.
/// </summary>
public class Tenant : BaseEntity
{
    /// <summary>
    /// Tenant's organization name.
    /// </summary>
    public string Organization { get; set; } = string.Empty;
}