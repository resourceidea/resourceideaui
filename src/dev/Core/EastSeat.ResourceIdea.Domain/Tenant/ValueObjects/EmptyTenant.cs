using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EastSeat.ResourceIdea.Domain.Tenant.ValueObjects;

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
