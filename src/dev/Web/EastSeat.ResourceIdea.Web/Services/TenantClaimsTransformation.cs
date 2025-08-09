using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace EastSeat.ResourceIdea.Web.Services;

/// <summary>
/// Claims transformation service that adds tenant and backend role information to user claims during authentication.
/// </summary>
public class TenantClaimsTransformation(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ILogger<TenantClaimsTransformation> logger, IMemoryCache cache) : IClaimsTransformation
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly ILogger<TenantClaimsTransformation> _logger = logger;
    private readonly IMemoryCache _cache = cache;

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        // Only process authenticated users
        if (principal.Identity?.IsAuthenticated != true)
        {
            return principal;
        }

        // Check if both required claims already exist
        var tenantIdClaim = principal.FindFirst("TenantId");
        var isBackendRoleClaim = principal.FindFirst("IsBackendRole");

        if (tenantIdClaim != null && isBackendRoleClaim != null)
        {
            return principal;
        }

        var userId = _userManager.GetUserId(principal);
        if (string.IsNullOrEmpty(userId))
        {
            return principal;
        }

        // Use cache to avoid repeated database queries for the same user
        var cacheKey = $"user_claims_{userId}";
        var cachedClaims = _cache.Get<(string TenantId, bool IsBackendRole)?>(cacheKey);

        string tenantId;
        bool isBackendUser;

        if (cachedClaims.HasValue)
        {
            tenantId = cachedClaims.Value.TenantId;
            isBackendUser = cachedClaims.Value.IsBackendRole;
        }
        else
        {
            _logger.LogInformation("Loading tenant and backend role claims for user {UserName}", principal.Identity.Name);

            // Get the user to retrieve TenantId and roles
            var user = await _userManager.GetUserAsync(principal);
            if (user == null)
            {
                _logger.LogWarning("User {UserName} not found", principal.Identity.Name);
                return principal;
            }

            // Get user roles to determine if they are backend roles
            var userRoles = await _userManager.GetRolesAsync(user);
            isBackendUser = false;

            foreach (var roleName in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role?.IsBackendRole == true)
                {
                    isBackendUser = true;
                    break;
                }
            }

            tenantId = user.TenantId.Value.ToString();

            // Cache the claims for 5 minutes to reduce database hits
            _cache.Set(cacheKey, (tenantId, isBackendUser), TimeSpan.FromMinutes(5));
        }

        // Create a new identity with the additional claims
        var identity = principal.Identity as ClaimsIdentity;
        if (identity != null)
        {
            // Add IsBackendRole claim if not present
            if (isBackendRoleClaim == null)
            {
                identity.AddClaim(new Claim("IsBackendRole", isBackendUser.ToString()));
                _logger.LogDebug("Added IsBackendRole claim: {IsBackendRole} for user {UserName}", isBackendUser, principal.Identity.Name);
            }

            // Add TenantId claim if not present
            if (tenantIdClaim == null && !string.IsNullOrEmpty(tenantId) && tenantId != Guid.Empty.ToString())
            {
                identity.AddClaim(new Claim("TenantId", tenantId));
                _logger.LogDebug("Added TenantId claim: {TenantId} for user {UserName}", tenantId, principal.Identity.Name);
            }
        }

        return principal;
    }
}
