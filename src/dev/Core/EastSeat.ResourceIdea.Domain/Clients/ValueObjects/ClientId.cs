using EastSeat.ResourceIdea.Domain.Exceptions;

namespace EastSeat.ResourceIdea.Domain.Clients.ValueObjects;

/// <summary>
/// Client Id.
/// </summary>
public readonly record struct ClientId
{
    /// <summary>
    /// Client Id value.
    /// </summary>
    public Guid Value { get; }

    private ClientId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Create a new Client Id.
    /// </summary>
    /// <param name="value">Client Id as a Guid.</param>
    /// <returns>Instance of <see cref="ClientId"/>.</returns>
    /// <exception cref="InvalidEntityIdException">Thrown when creating a new <see cref="ClientId"/> from an empty Guid.</exception>
    public static ClientId Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidEntityIdException("ClientId cannot be empty");
        }

        return new ClientId(value);
    }

    /// <summary>
    /// Empty client id.
    /// </summary>
    public static ClientId Empty => new(Guid.Empty);
}
