using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.Common.Contracts;

/// <summary>
/// Provides access to the current user's authentication context.
/// </summary>
public interface IAuthenticationContext
{
    /// <summary>
    /// Gets the tenant ID for the current authenticated user.
    /// </summary>
    TenantId TenantId { get; }
}