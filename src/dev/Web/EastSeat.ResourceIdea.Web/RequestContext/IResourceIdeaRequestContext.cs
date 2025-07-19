// -------------------------------------------------------------------------------
// File: IResourceIdeaRequestContext.cs
// Path: src\dev\Web\EastSeat.ResourceIdea.Web\RequestContext\IResourceIdeaRequestContext.cs
// Description: ResourceIdea app request context interface.
// -------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Web.RequestContext;

/// <summary>
/// ResourceIdea app request context interface.
/// </summary>
public interface IResourceIdeaRequestContext
{
    TenantId Tenant { get; }

    /// <summary>
    /// Gets a value indicating whether the current user is a backend user (Developer or Support).
    /// </summary>
    bool IsBackendUser { get; }

    /// <summary>
    /// Gets the TenantId for the current user. If the user is not authenticated or
    /// the TenantId claim is missing, throws a TenantAuthenticationException.
    /// </summary>
    /// <returns>The TenantId for the current user.</returns>
    /// <exception cref="EastSeat.ResourceIdea.Web.Exceptions.TenantAuthenticationException">
    /// Thrown when the user is not authenticated or the TenantId claim is missing.
    /// </exception>
    Task<TenantId> GetTenantId();

    /// <summary>
    /// Gets a value indicating whether the current user has access to backend functionality.
    /// Backend users (Developer/Support) should have access to all tenant data for support purposes.
    /// </summary>
    /// <returns>True if the user is a backend user, false otherwise.</returns>
    bool HasBackendAccess();
}