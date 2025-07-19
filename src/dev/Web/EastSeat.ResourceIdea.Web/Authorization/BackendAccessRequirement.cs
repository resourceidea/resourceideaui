// -------------------------------------------------------------------------------
// File: BackendAccessRequirement.cs
// Path: src/dev/Web/EastSeat.ResourceIdea.Web/Authorization/BackendAccessRequirement.cs
// Description: Authorization requirement for backend access validation
// -------------------------------------------------------------------------------

using Microsoft.AspNetCore.Authorization;

namespace EastSeat.ResourceIdea.Web.Authorization;

/// <summary>
/// Authorization requirement that validates backend access.
/// </summary>
public class BackendAccessRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// Gets the requirement description.
    /// </summary>
    public string Description => "User must have backend access (Developer or Support role)";
}