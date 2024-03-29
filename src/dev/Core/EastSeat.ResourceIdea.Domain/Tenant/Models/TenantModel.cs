namespace EastSeat.ResourceIdea.Domain.Tenant.Models;

/// <summary>
/// Tenant model.
/// </summary>
public class TenantModel
{
    /// <summary>
    /// Tenant Id.
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// Tenant's organization name.
    /// </summary>
    public string Organization { get; set; } = string.Empty;
}