using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.Roles.Models;

/// <summary>
/// Role model.
/// </summary>
public record RoleModel
{
    /// <summary>
    /// Role identifier.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Role name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Role description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Tenant identifier associated with the role.
    /// Null for system roles.
    /// </summary>
    public TenantId? TenantId { get; set; }

    /// <summary>
    /// Indicates whether this is a backend/system role.
    /// </summary>
    public bool IsBackendRole { get; set; }

    /// <summary>
    /// Indicates whether this is a system role (built-in).
    /// </summary>
    public bool IsSystemRole => IsBackendRole && TenantId == null;

    /// <summary>
    /// Role claims associated with this role.
    /// </summary>
    public IReadOnlyList<RoleClaimModel> Claims { get; set; } = [];
}