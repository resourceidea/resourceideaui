// -------------------------------------------------------------------------------
// File: IAuthenticationContext.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Common\Contracts\IAuthenticationContext.cs
// Description: Authentication context interface for tracking current user and tenant.
// -------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.Common.Contracts;

/// <summary>
/// Authentication context interface for tracking current user and tenant.
/// </summary>
public interface IAuthenticationContext
{
    /// <summary>
    /// Current tenant ID.
    /// </summary>
    TenantId TenantId { get; }

    /// <summary>
    /// Current application user ID.
    /// </summary>
    ApplicationUserId ApplicationUserId { get; }
}