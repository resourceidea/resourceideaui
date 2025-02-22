using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Web.RequestContext;

public sealed class ResourceIdeaRequestContext(IHttpContextAccessor httpContextAccessor) : IResourceIdeaRequestContext
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public TenantId Tenant => GetTenantId();

    private static TenantId GetTenantId()
    {
        // string? tenantIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst("TenantId")?.Value.Trim();
        // if (string.IsNullOrEmpty(tenantIdStr))
        // {
        //     // Optionally, throw or handle when the tenant id is missing.
        //     throw new ApplicationException("Tenant id not found in user session.");
        // }

        // return TenantId.Create(tenantIdStr);
        return TenantId.Create("841C6122-59E8-4294-93B8-D21C0BEB6724");
    }
}