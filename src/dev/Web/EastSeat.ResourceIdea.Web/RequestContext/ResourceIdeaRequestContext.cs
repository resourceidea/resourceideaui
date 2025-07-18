using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Web.Exceptions;

namespace EastSeat.ResourceIdea.Web.RequestContext;

public sealed class ResourceIdeaRequestContext(IHttpContextAccessor httpContextAccessor) : IResourceIdeaRequestContext
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public TenantId Tenant => GetTenantIdSync();

    /// <summary>
    /// Gets the TenantId for the current user. If the user is not authenticated or
    /// the TenantId claim is missing, throws a TenantAuthenticationException.
    /// </summary>
    /// <returns>The TenantId for the current user.</returns>
    /// <exception cref="TenantAuthenticationException">
    /// Thrown when the user is not authenticated or the TenantId claim is missing.
    /// </exception>
    public async Task<TenantId> GetTenantId()
    {
        // For now, this is essentially the same as the synchronous version
        // but we make it async for consistency with the interface
        return await Task.FromResult(GetTenantIdSync());
    }

    private TenantId GetTenantIdSync()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        // Check if user is authenticated
        if (httpContext?.User?.Identity?.IsAuthenticated != true)
        {
            throw new UnauthorizedAccessException("User is not authenticated. Please sign in to access this resource.");
        }

        string? tenantIdStr = httpContext.User.FindFirst("TenantId")?.Value?.Trim();
        if (string.IsNullOrEmpty(tenantIdStr))
        {
            // TenantId claim is missing - this should trigger a redirect to login
            throw new TenantAuthenticationException("Tenant information not found in your session. Please sign in again to access this resource.");
        }

        return TenantId.Create(tenantIdStr);
    }
}