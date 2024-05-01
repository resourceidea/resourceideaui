using EastSeat.ResourceIdea.Domain.Clients.Entities;

namespace EastSeat.ResourceIdea.Domain.Clients.ValueObjects;

/// <summary>
/// Representation of an <see cref="Client"/>.
/// </summary>
public sealed record EmptyClient
{
    /// <summary>
    /// Instance of an empty <see cref="Client"/>.
    /// </summary>
    public static Client Instance => new()
    {
        Id = ClientId.Empty,
        Name = string.Empty,
        Address = Address.Empty
    };

    private EmptyClient() { }
}
