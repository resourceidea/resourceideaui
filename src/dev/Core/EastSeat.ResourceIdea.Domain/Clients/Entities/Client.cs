using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Common.Entities;

namespace EastSeat.ResourceIdea.Domain.Clients.Entities;

public class Client : BaseEntity
{
    /// <summary>
    /// Client ID.
    /// </summary>
    public ClientId Id { get; set; }

    /// <summary>
    /// Client address.
    /// </summary>
    public Address Address { get; set; } = Address.Empty;

    /// <summary>
    /// Client name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Checks if the instance of <see cref="Client"/> is empty.
    /// </summary>
    /// <returns>True if instance is empty; Otherwise, False.</returns>
    public bool IsEmpty() => this == EmptyClient.Instance;
}