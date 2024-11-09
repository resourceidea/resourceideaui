using Microsoft.AspNetCore.Identity;

namespace EastSeat.ResourceIdea.DataStore.Identity.Entities;

/// <summary>
/// Represents an application role.
/// </summary>
public class ApplicationIdentityUserRole : IdentityUserRole<string>
{
    /// <summary>
    /// Gets or sets the role associated with the user.
    /// </summary>
    public ApplicationRole? Role { get; set; }

    /// <summary>
    /// Gets or sets the user associated with the role.
    /// </summary>
    public ApplicationUser? User { get; set; }
}