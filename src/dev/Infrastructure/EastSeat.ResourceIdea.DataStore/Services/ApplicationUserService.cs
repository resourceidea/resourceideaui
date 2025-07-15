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
using Microsoft.EntityFrameworkCore;

namespace EastSeat.ResourceIdea.DataStore.Services;

/// <summary>
/// Handles operations managing the application users.
/// </summary>
public class ApplicationUserService(UserManager<ApplicationUser> userManager, ResourceIdeaDBContext dbContext) : IApplicationUserService
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
    public async Task<ResourceIdeaResponse<IApplicationUser>> UpdateApplicationUserAsync(
        ApplicationUserId applicationUserId,
        string firstName,
        string lastName)
    {
        using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            ApplicationUser? applicationUser = await userManager.FindByIdAsync(applicationUserId.ToString());
            if (applicationUser == null)
            {
                await transaction.RollbackAsync();
                return ResourceIdeaResponse<IApplicationUser>.Failure(ErrorCode.ApplicationUserNotFound);
            }

            applicationUser.FirstName = firstName;
            applicationUser.LastName = lastName;

            IdentityResult result = await userManager.UpdateAsync(applicationUser);
            if (!result.Succeeded)
            {
                await transaction.RollbackAsync();
                return ResourceIdeaResponse<IApplicationUser>.Failure(ErrorCode.UpdateApplicationUserFailure);
            }

            // Update corresponding Employee record to keep names in sync
            var employee = await dbContext.Employees
                .FirstOrDefaultAsync(e => e.ApplicationUserId == applicationUserId);
            if (employee != null)
            {
                employee.FirstName = firstName;
                employee.LastName = lastName;
                dbContext.Employees.Update(employee);
                
                int changes = await dbContext.SaveChangesAsync();
                if (changes < 1)
                {
                    await transaction.RollbackAsync();
                    return ResourceIdeaResponse<IApplicationUser>.Failure(ErrorCode.DbUpdateFailureOnUpdateEmployee);
                }
            }

            await transaction.CommitAsync();
            return ResourceIdeaResponse<IApplicationUser>.Success(Optional<IApplicationUser>.Some(applicationUser));
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return ResourceIdeaResponse<IApplicationUser>.Failure(ErrorCode.UpdateApplicationUserFailure);
        }
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
}
