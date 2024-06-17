using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.Users.Entities;

/// <summary>
/// Interface for an application user that is attached to a tenant.
/// </summary>
public interface IApplicationUser
{
    /// <summary>
    /// Application User ID.
    /// </summary>
    ApplicationUserId ApplicationUserId { get; set; }

    /// <summary>
    /// Tenant ID.
    /// </summary>
    TenantId TenantId { get; set; }

    /// <summary>
    /// First name of the user.
    /// </summary>
    string FirstName { get; set; }

    /// <summary>
    /// Last name of the user.
    /// </summary>
    string LastName { get; set; }

    /// <summary>
    /// Full name of the user.
    /// </summary>
    string FullName => $"{FirstName} {LastName}";
}