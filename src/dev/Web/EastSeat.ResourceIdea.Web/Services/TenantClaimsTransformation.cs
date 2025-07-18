using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;

namespace EastSeat.ResourceIdea.Web.Services;

/// <summary>
/// Claims transformation service that adds tenant information to user claims during authentication.
/// </summary>
public class TenantClaimsTransformation(UserManager<ApplicationUser> userManager, ILogger<TenantClaimsTransformation> logger) : IClaimsTransformation
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ILogger<TenantClaimsTransformation> _logger = logger;

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        // Only process authenticated users
        if (principal.Identity?.IsAuthenticated != true)
        {
            return principal;
        }

        // Check if TenantId claim already exists
        if (principal.FindFirst("TenantId") != null)
        {
            return principal;
        }

        _logger.LogInformation("Adding TenantId claim for user {UserName}", principal.Identity.Name);

        // Get the user to retrieve TenantId
        var user = await _userManager.GetUserAsync(principal);
        if (user?.TenantId == null)
        {
            _logger.LogWarning("User {UserName} found but TenantId is null", principal.Identity.Name);
            return principal;
        }

        _logger.LogInformation("Found TenantId {TenantId} for user {UserName}", user.TenantId.Value, principal.Identity.Name);

        // Create a new identity with the TenantId claim
        var identity = principal.Identity as ClaimsIdentity;
        if (identity != null)
        {
            identity.AddClaim(new Claim("TenantId", user.TenantId.Value.ToString()));
            _logger.LogInformation("Successfully added TenantId claim for user {UserName}", principal.Identity.Name);
        }

        return principal;
    }
}
