// -------------------------------------------------------------------------------
// File: BackendAccessHandler.cs
// Path: src/dev/Web/EastSeat.ResourceIdea.Web/Authorization/BackendAccessHandler.cs
// Description: Authorization handler for backend access validation
// -------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Web.RequestContext;
using Microsoft.AspNetCore.Authorization;

namespace EastSeat.ResourceIdea.Web.Authorization;

/// <summary>
/// Authorization handler that validates backend access requirements.
/// </summary>
public class BackendAccessHandler(IResourceIdeaRequestContext requestContext) : AuthorizationHandler<BackendAccessRequirement>
{
    private readonly IResourceIdeaRequestContext _requestContext = requestContext;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BackendAccessRequirement requirement)
    {
        try
        {
            // Check if the current user has backend access
            if (_requestContext.HasBackendAccess())
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
        catch
        {
            // If there's any error checking backend access, deny access
            context.Fail();
        }

        return Task.CompletedTask;
    }
}