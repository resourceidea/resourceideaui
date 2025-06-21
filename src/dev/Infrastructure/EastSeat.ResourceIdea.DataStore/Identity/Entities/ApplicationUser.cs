using EastSeat.ResourceIdea.Domain.Users.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;

using Microsoft.AspNetCore.Identity;

namespace EastSeat.ResourceIdea.DataStore.Identity.Entities;

/// <summary>
/// Application user entity.
/// </summary>
public class ApplicationUser : IdentityUser, IApplicationUser
{
    /// <inheritdoc />
    public ApplicationUserId ApplicationUserId { get; set; }

    /// <inheritdoc />
    public TenantId TenantId { get; set; }

    /// <inheritdoc />
    public string FirstName { get; set; } = string.Empty;

    /// <inheritdoc />
    public string LastName { get; set; } = string.Empty;
}