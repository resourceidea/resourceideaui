// -------------------------------------------------------------------------------
// File: BackendAccessAttribute.cs
// Path: src/dev/Web/EastSeat.ResourceIdea.Web/Authorization/BackendAccessAttribute.cs
// Description: Authorization attribute for backend-only access (Developer/Support roles)
// -------------------------------------------------------------------------------

using Microsoft.AspNetCore.Authorization;

namespace EastSeat.ResourceIdea.Web.Authorization;

/// <summary>
/// Authorization attribute that restricts access to backend users only (Developer and Support roles).
/// Prevents tenant users from accessing backend functionality.
/// </summary>
public class BackendAccessAttribute : AuthorizeAttribute
{
    public BackendAccessAttribute()
    {
        Policy = "BackendAccess";
    }
}