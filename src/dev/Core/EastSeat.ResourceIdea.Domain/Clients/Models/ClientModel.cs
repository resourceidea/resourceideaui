using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.Clients.Models;

/// <summary>
/// Client model.
/// </summary>
public record ClientModel
{
    /// <summary>
    /// Client Id.
    /// </summary>
    public ClientId Id { get; set; }

    /// <summary>
    /// Owning tenant's Id.
    /// </summary>
    public TenantId TenantId { get; set; }

    /// <summary>
    /// Client name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Client address.
    /// </summary>
    public Address Address { get; set; }
}
