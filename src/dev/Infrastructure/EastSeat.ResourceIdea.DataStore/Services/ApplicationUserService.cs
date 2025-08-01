// ====================================================================================================
// File: ApplicationUserService.cs
// Path: src/dev/EastSeat.ResourceIdea.DataStore/Service/ApplicationUserService.cs
// Description: Service to handle operations managing Application Users.
// ====================================================================================================

using EastSeat.ResourceIdea.Application.Features.Employees.Contracts;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.Users.Entities;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace EastSeat.ResourceIdea.DataStore.Services;

/// <summary>
/// Handles operations managing the application users.
/// </summary>
public class ApplicationUserService(UserManager<ApplicationUser> userManager) : IApplicationUserService
{
    /// <inheritdoc/>
    public async Task<ResourceIdeaResponse<IApplicationUser>> AddApplicationUserAsync(
        string firstName,
        string lastName,
        string email,
        TenantId tenantId)
    {
        ApplicationUser newApplicationUser = new()
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            UserName = email,
            ApplicationUserId = ApplicationUserId.Create(Guid.NewGuid()),
            TenantId = tenantId,
        };

        string temporaryPassword = $"Temp@{Guid.NewGuid().ToString("N")[..8]}";
        IdentityResult result = await userManager.CreateAsync(newApplicationUser, temporaryPassword);
        if (!result.Succeeded)
        {
            return ResourceIdeaResponse<IApplicationUser>.Failure(ErrorCode.AddApplicationUserFailure);
        }

        return ResourceIdeaResponse<IApplicationUser>.Success(Optional<IApplicationUser>.Some(newApplicationUser));
    }

    /// <inheritdoc/>
    public async Task<ResourceIdeaResponse<IApplicationUser>> DeleteApplicationUserAsync(
        ApplicationUserId applicationUserId)
    {
        IdentityResult result = await userManager.DeleteAsync(new ApplicationUser
        {
            ApplicationUserId = applicationUserId
        });
        if (!result.Succeeded)
        {
            return ResourceIdeaResponse<IApplicationUser>.Failure(ErrorCode.DeleteApplicationUserFailure);
        }

        return ResourceIdeaResponse<IApplicationUser>.Success(Optional<IApplicationUser>.None);
    }

    /// <inheritdoc/>
    public async Task<ResourceIdeaResponse<string>> ResetPasswordAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return ResourceIdeaResponse<string>.Failure(ErrorCode.UserNotFound);
        }

        string temporaryPassword = $"Temp@{Guid.NewGuid().ToString("N")[..8]}";
        
        // Remove current password and set new one
        var removePasswordResult = await userManager.RemovePasswordAsync(user);
        if (!removePasswordResult.Succeeded)
        {
            return ResourceIdeaResponse<string>.Failure(ErrorCode.ResetPasswordFailure);
        }

        var addPasswordResult = await userManager.AddPasswordAsync(user, temporaryPassword);
        if (!addPasswordResult.Succeeded)
        {
            return ResourceIdeaResponse<string>.Failure(ErrorCode.ResetPasswordFailure);
        }

        return ResourceIdeaResponse<string>.Success(Optional<string>.Some(temporaryPassword));
    }
}
