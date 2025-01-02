using EastSeat.ResourceIdea.Domain.Exceptions;

namespace EastSeat.ResourceIdea.Domain.Clients.ValueObjects;

/// <summary>
/// Client DepartmentId.
/// </summary>
public readonly record struct ClientId
{
    /// <summary>
    /// Client DepartmentId value.
    /// </summary>
    public Guid Value { get; }

    private ClientId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Create a new Client DepartmentId.
    /// </summary>
    /// <param name="value">Client DepartmentId as a Guid.</param>
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
    /// Create a new Client ID.
    /// </summary>
    /// <param name="value">Client ID as a string.</param>
    /// <returns>Instance of <see cref="ClientId"/>.</returns>
    /// <exception cref="InvalidEntityIdException">Thrown when creating a new <see cref="ClientId"/> 
    /// from a string that cannot be parsed to a Guid.</exception>
    public static ClientId Create(string value)
    {
        if (!Guid.TryParse(value, out var clientId))
        {
            throw new InvalidEntityIdException("ClientId is not a valid Guid");
        }

        return Create(clientId);
    }

    /// <summary>
    /// Empty client id.
    /// </summary>
    public static ClientId Empty => new(Guid.Empty);
}
