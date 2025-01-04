using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.Clients.Models;

/// <summary>
/// Client model.
/// </summary>
public record ClientModel
{
    /// <summary>
    /// Client DepartmentId.
    /// </summary>
    public ClientId ClientId { get; set; }

    /// <summary>
    /// Owning tenant's DepartmentId.
    /// </summary>
    public TenantId TenantId { get; set; }

    /// <summary>
    /// Client name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Client address.
    /// </summary>
    public Address Address { get; set; } = Address.Empty;
}
