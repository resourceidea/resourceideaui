using EastSeat.ResourceIdea.Domain.Exceptions;

namespace EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

public readonly record struct TenantId
{
    /// <summary>
    /// Tenant id value.
    /// </summary>
    public Guid Value { get; }

    private TenantId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Create a new Tenant id.
    /// </summary>
    /// <param name="value">Tenant id as a Guid.</param>
    /// <returns>Instance of <see cref="TenantId"/>.</returns>
    /// <exception cref="InvalidEntityIdException">Thrown when creating a new TenantId
    /// from an empty Guid.</exception>
    public static TenantId Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidEntityIdException("TenantId cannot be empty");
        }

        return new TenantId(value);
    }

    /// <summary>Tenant Id is not empty.</summary>
    public bool IsNotEmpty() => this != TenantId.Empty;

    /// <summary>
    /// Empty tenant id.
    /// </summary>
    public static TenantId Empty => new (Guid.Empty);
}