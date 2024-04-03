namespace EastSeat.ResourceIdea.Domain.TenantManagement.ValueObjects;

/// <summary>
/// Empty tenant instance.
/// </summary>
public sealed record EmptyTenant
{
    public static Entities.Tenant Instance { get; } = new()
    {
        TenantId = Guid.Empty,
        Organization = string.Empty
    };

    private EmptyTenant() { }
}
