using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EastSeat.ResourceIdea.Web.RequestContext;

public sealed class ResourceIdeaRequestContext(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager) : IResourceIdeaRequestContext, IAuthenticationContext
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public TenantId Tenant => GetTenantId();

    public ApplicationUserId ApplicationUser => GetApplicationUserId();

    // Explicit interface implementation for IAuthenticationContext
    TenantId IAuthenticationContext.TenantId => Tenant;
    ApplicationUserId IAuthenticationContext.ApplicationUserId => ApplicationUser;

    private TenantId GetTenantId()
    {
        var user = GetCurrentApplicationUser();
        if (user != null)
        {
            return user.TenantId;
        }

        // Fallback to hardcoded value for development - TODO: Remove in production
        return TenantId.Create("841C6122-59E8-4294-93B8-D21C0BEB6724");
    }

    private ApplicationUserId GetApplicationUserId()
    {
        var user = GetCurrentApplicationUser();
        if (user != null)
        {
            return user.ApplicationUserId;
        }

        // Return empty if no authenticated user context
        return ApplicationUserId.Empty;
    }

    private ApplicationUser? GetCurrentApplicationUser()
    {
        try
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return null;
            }

            var claimsIdentity = httpContext.User?.Identity as ClaimsIdentity;
            if (claimsIdentity?.IsAuthenticated != true)
            {
                return null;
            }

            var userName = claimsIdentity.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return null;
            }

            // Note: This is a synchronous call, which is not ideal, but necessary for this pattern
            // In a real application, you might want to cache this or use a different approach
            var user = _userManager.FindByNameAsync(userName).GetAwaiter().GetResult();
            
            // Additional safety check: if user lookup fails, treat as unauthenticated
            return user;
        }
        catch (Exception)
        {
            // If any exception occurs during user lookup (e.g., database issues),
            // treat as unauthenticated for safety
            return null;
        }
    }
}