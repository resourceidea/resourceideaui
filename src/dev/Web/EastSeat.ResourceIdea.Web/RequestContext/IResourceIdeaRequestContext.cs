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
}