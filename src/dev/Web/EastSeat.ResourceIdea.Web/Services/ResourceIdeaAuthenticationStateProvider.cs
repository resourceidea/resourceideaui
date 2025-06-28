using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using System.Security.Claims;

namespace EastSeat.ResourceIdea.Web.Services;

/// <summary>
/// Custom authentication state provider for ResourceIdea application.
/// Integrates with ASP.NET Core Identity to provide authentication state.
/// </summary>
public class ResourceIdeaAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ResourceIdeaAuthenticationStateProvider(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Gets the current authentication state.
    /// </summary>
    /// <returns>The authentication state containing user information and claims.</returns>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext?.User?.Identity?.IsAuthenticated == true)
        {
            var user = await _userManager.GetUserAsync(httpContext.User);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id),
                    new(ClaimTypes.Name, user.UserName ?? string.Empty),
                    new(ClaimTypes.Email, user.Email ?? string.Empty),
                    new("FirstName", user.FirstName),
                    new("LastName", user.LastName),
                    new("TenantId", user.TenantId.Value.ToString())
                };

                // Add role claims
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
                var principal = new ClaimsPrincipal(identity);

                return new AuthenticationState(principal);
            }
        }

        // Return anonymous user if not authenticated
        var anonymousIdentity = new ClaimsIdentity();
        var anonymousPrincipal = new ClaimsPrincipal(anonymousIdentity);
        return new AuthenticationState(anonymousPrincipal);
    }

    /// <summary>
    /// Notifies about authentication state changes.
    /// </summary>
    public void NotifyAuthenticationStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
