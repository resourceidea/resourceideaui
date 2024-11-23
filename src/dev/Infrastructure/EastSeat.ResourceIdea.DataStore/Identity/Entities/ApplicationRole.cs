using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

using Microsoft.AspNetCore.Identity;

namespace EastSeat.ResourceIdea.DataStore.Identity.Entities;


/// <summary>
/// Represents an application role.
/// </summary>
public class ApplicationRole : IdentityRole
{
    /// <summary>
    /// Gets or sets the tenant ID associated with the role.
    /// </summary>
    public TenantId TenantId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the role is a backend role.
    /// </summary>
    public bool IsBackendRole { get; set; }
}
