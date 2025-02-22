using EastSeat.ResourceIdea.Domain.Exceptions;

namespace EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

public readonly record struct TenantId
{
    private TenantId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Tenant id value.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Empty tenant id.
    /// </summary>
    public static TenantId Empty => new(Guid.Empty);

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

    /// <summary>
    /// Creates a tenant id from a string value.
    /// </summary>
    /// <param name="value">Tenant ID as a string.</param>
    /// <returns>Instance of <see cref="TenantId"/></returns>
    /// <exception cref="InvalidEntityIdException">Thrown when creating a new Tenant ID
    /// from a string value that can't be parsed to a Guid.</exception>
    public static TenantId Create(string value)
    {
        if (!Guid.TryParse(value, out var tenantId))
        {
            throw new InvalidEntityIdException("TenantId is not a valid Guid");
        }

        return Create(tenantId);
    }

    /// <summary>Tenant DepartmentId is not empty.</summary>
    public bool IsNotEmpty() => this != TenantId.Empty;
    
    /// <summary>
    /// Validate that the Tenant ID is not empty.
    /// </summary>
    /// <returns>Message on validation failure, otherwise empty string.</returns>
    public string ValidateRequired()
    {
        return Value == Guid.Empty ? "TenantId is required." : string.Empty;
    }
}