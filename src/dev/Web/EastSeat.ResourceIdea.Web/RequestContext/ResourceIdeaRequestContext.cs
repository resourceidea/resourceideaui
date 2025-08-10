using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Web.Exceptions;

namespace EastSeat.ResourceIdea.Web.RequestContext;

public sealed class ResourceIdeaRequestContext(IHttpContextAccessor httpContextAccessor) : IResourceIdeaRequestContext
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public TenantId Tenant => GetTenantIdSync();

    public bool IsBackendUser => HasBackendAccess();

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

    /// <summary>
    /// Gets a value indicating whether the current user has access to backend functionality.
    /// Backend users (Developer/Support) should have access to all tenant data for support purposes.
    /// </summary>
    /// <returns>True if the user is a backend user, false otherwise.</returns>
    public bool HasBackendAccess()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext?.User?.Identity?.IsAuthenticated != true)
        {
            return false;
        }

        // Check if user has backend role
        var isBackendRole = httpContext.User.FindFirst("IsBackendRole")?.Value?.Trim();
        if (bool.TryParse(isBackendRole, out bool isBackend) && isBackend)
        {
            return true;
        }

        // Check if user is in Developer or Support roles
        return httpContext.User.IsInRole("Developer") || httpContext.User.IsInRole("Support");
    }

    private TenantId GetTenantIdSync()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        // Check if user is authenticated
        if (httpContext?.User?.Identity?.IsAuthenticated != true)
        {
            throw new UnauthorizedAccessException("User is not authenticated. Please sign in to access this resource.");
        }

        // Backend users don't need a tenant ID for system-wide access
        if (HasBackendAccess())
        {
            // Return a system-wide tenant ID or throw a different exception
            // For now, we'll allow backend users to work without a specific tenant
            var systemTenantId = httpContext.User.FindFirst("TenantId")?.Value?.Trim();
            if (!string.IsNullOrEmpty(systemTenantId))
            {
                return TenantId.Create(systemTenantId);
            }
            
            // For backend users without a specific tenant, we might need to handle this differently
            // This could be a system-wide tenant or we may need to refactor the architecture
            throw new InvalidOperationException("Backend users require a system tenant context. Please contact an administrator.");
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