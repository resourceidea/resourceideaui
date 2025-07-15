// =============================================================================================
// File: IApplicationUserService.cs
// Path: src/dev/Core/EastSeat.ResourceIdea.Application/Features/Employees/Contracts/IApplicationUserService.cs
// Description: Interface the the service managing the application user operations.
// =============================================================================================

using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.Users.Entities;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Contracts;

/// <summary>
/// Interface to the application user managing service.
/// </summary>
public interface IApplicationUserService
{
    /// <summary>
    /// Add application user.
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="email"></param>
    /// <returns><see cref="ResourceIdeaResponse{IApplicationUser}"/></returns>
    Task<ResourceIdeaResponse<IApplicationUser>> AddApplicationUserAsync(
        string firstName,
        string lastName,
        string email,
        TenantId tenantId);

    /// <summary>
    /// Update application user.
    /// </summary>
    /// <param name="applicationUserId"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <returns><see cref="ResourceIdeaResponse{IApplicationUser}"/></returns>
    Task<ResourceIdeaResponse<IApplicationUser>> UpdateApplicationUserAsync(
        ApplicationUserId applicationUserId,
        string firstName,
        string lastName);

    /// <summary>
    /// Delete application user.
    /// </summary>
    /// <param name="applicationUserId"></param>
    /// <returns><see cref="ResourceIdeaResponse{IApplicationUser}"/></returns>
    Task<ResourceIdeaResponse<IApplicationUser>> DeleteApplicationUserAsync(ApplicationUserId applicationUserId);
}
