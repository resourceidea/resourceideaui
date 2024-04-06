using EastSeat.ResourceIdea.Domain.Tenants.Entities;

namespace EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

/// <summary>
/// Empty tenant instance.
/// </summary>
public sealed record EmptyTenant
{
    public static Tenant Instance { get; } = new()
    {
        TenantId = Guid.Empty,
        Organization = string.Empty
    };

    private EmptyTenant() { }
}
