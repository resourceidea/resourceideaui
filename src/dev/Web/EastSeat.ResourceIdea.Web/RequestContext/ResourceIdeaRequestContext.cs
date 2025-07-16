using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using System.Security.Claims;

namespace EastSeat.ResourceIdea.Web.RequestContext;

public sealed class ResourceIdeaRequestContext(IHttpContextAccessor httpContextAccessor) : IResourceIdeaRequestContext, IAuthenticationContext
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public TenantId Tenant => GetTenantId();

    public ApplicationUserId ApplicationUser => GetApplicationUserId();

    // Explicit interface implementation for IAuthenticationContext
    TenantId IAuthenticationContext.TenantId => Tenant;
    ApplicationUserId IAuthenticationContext.ApplicationUserId => ApplicationUser;

    private TenantId GetTenantId()
    {
        string? tenantIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst("TenantId")?.Value?.Trim();
        if (string.IsNullOrEmpty(tenantIdStr))
        {
            // Fallback to hardcoded value for development - TODO: Remove in production
            return TenantId.Create("841C6122-59E8-4294-93B8-D21C0BEB6724");
        }

        return TenantId.Create(tenantIdStr);
    }

    private ApplicationUserId GetApplicationUserId()
    {
        string? applicationUserIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst("ApplicationUserId")?.Value?.Trim();
        if (string.IsNullOrEmpty(applicationUserIdStr))
        {
            // Try to get from sub claim as fallback
            applicationUserIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value?.Trim();
        }

        if (string.IsNullOrEmpty(applicationUserIdStr))
        {
            // Return empty if no authenticated user context
            return ApplicationUserId.Empty;
        }

        return ApplicationUserId.Create(applicationUserIdStr);
    }
}