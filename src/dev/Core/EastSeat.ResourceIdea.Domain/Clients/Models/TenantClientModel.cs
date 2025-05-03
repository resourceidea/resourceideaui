using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.Clients.Models;

/// <summary>
/// Represents a model for a tenant client.
/// </summary>
/// <param name="ClientId">ClientId</param>
/// <param name="Address"></param>
/// <param name="Name"></param>
public record TenantClientModel(ClientId ClientId, Address Address, string Name)
{
    /// <summary>
    /// Represents an empty instance of <see cref="TenantClientModel"/> with default values.
    /// </summary>
    public static TenantClientModel Empty => new(ClientId.Empty, Address.Empty, string.Empty);

    /// <summary>
    /// Checks if the current instance is empty (equivalent to the Empty property).
    /// </summary>
    public bool IsEmpty => this == Empty;
}
