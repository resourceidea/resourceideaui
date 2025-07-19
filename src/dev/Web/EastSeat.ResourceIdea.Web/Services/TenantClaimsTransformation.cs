using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;

namespace EastSeat.ResourceIdea.Web.Services;

/// <summary>
/// Claims transformation service that adds tenant and backend role information to user claims during authentication.
/// </summary>
public class TenantClaimsTransformation(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ILogger<TenantClaimsTransformation> logger) : IClaimsTransformation
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly ILogger<TenantClaimsTransformation> _logger = logger;

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        // Only process authenticated users
        if (principal.Identity?.IsAuthenticated != true)
        {
            return principal;
        }

        // Check if TenantId claim already exists
        if (principal.FindFirst("TenantId") != null && principal.FindFirst("IsBackendRole") != null)
        {
            return principal;
        }

        _logger.LogInformation("Adding tenant and backend role claims for user {UserName}", principal.Identity.Name);

        // Get the user to retrieve TenantId and roles
        var user = await _userManager.GetUserAsync(principal);
        if (user == null)
        {
            _logger.LogWarning("User {UserName} not found", principal.Identity.Name);
            return principal;
        }

        // Get user roles to determine if they are backend roles
        var userRoles = await _userManager.GetRolesAsync(user);
        bool isBackendUser = false;

        foreach (var roleName in userRoles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role?.IsBackendRole == true)
            {
                isBackendUser = true;
                break;
            }
        }

        // Create a new identity with the additional claims
        var identity = principal.Identity as ClaimsIdentity;
        if (identity != null)
        {
            // Add IsBackendRole claim
            if (principal.FindFirst("IsBackendRole") == null)
            {
                identity.AddClaim(new Claim("IsBackendRole", isBackendUser.ToString()));
                _logger.LogInformation("Added IsBackendRole claim: {IsBackendRole} for user {UserName}", isBackendUser, principal.Identity.Name);
            }

            // Add TenantId claim only if not already present
            if (principal.FindFirst("TenantId") == null)
            {
                if (user.TenantId.Value == Guid.Empty)
                {
                    if (isBackendUser)
                    {
                        // Backend users might not have a specific tenant - log this but don't fail
                        _logger.LogInformation("Backend user {UserName} does not have a specific TenantId", principal.Identity.Name);
                    }
                    else
                    {
                        _logger.LogWarning("Non-backend user {UserName} found but TenantId is empty", principal.Identity.Name);
                    }
                }
                else
                {
                    identity.AddClaim(new Claim("TenantId", user.TenantId.Value.ToString()));
                    _logger.LogInformation("Added TenantId claim: {TenantId} for user {UserName}", user.TenantId.Value, principal.Identity.Name);
                }
            }
        }

        return principal;
    }
}
