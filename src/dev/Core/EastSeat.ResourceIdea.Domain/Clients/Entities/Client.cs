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
    public Address Address { get; set; }

    /// <summary>
    /// Client name.
    /// </summary>
    public required string Name { get; set; }
}