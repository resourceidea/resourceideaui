// ===================================================================================
// File: Profile.razor.cs
// Path: src\dev\Web\EastSeat.ResourceIdea.Web\Components\Pages\Profile.razor.cs
// Description: Code-behind for the user profile page.
// ===================================================================================

using EastSeat.ResourceIdea.Application.Features.Employees.Commands;
using EastSeat.ResourceIdea.Application.Features.Employees.Queries;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;
using EastSeat.ResourceIdea.Web.Components.Base;
using EastSeat.ResourceIdea.Web.RequestContext;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace EastSeat.ResourceIdea.Web.Components.Pages;

[Authorize]
public partial class Profile : ResourceIdeaComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = null!;
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    private string? message;
    private bool isErrorMessage;
    private TenantEmployeeModel? CurrentEmployee;
    private string? ManagerName;

    protected override async Task OnInitializedAsync()
    {
        await ExecuteAsync(async () =>
        {
            await LoadUserProfile();
        }, "Loading profile");
    }

    private async Task LoadUserProfile()
    {
        await ExecuteAsync(async () =>
        {
            // Get the current user's ApplicationUserId from authentication claims
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var userIdClaim = authState.User.FindFirst(ClaimTypes.NameIdentifier);
            
            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                message = "Unable to identify current user. Please sign in again.";
                isErrorMessage = true;
                return;
            }

            var applicationUserId = ApplicationUserId.Create(userIdClaim.Value);
            
            // Get current user's profile
            var query = new GetCurrentUserProfileQuery
            {
                ApplicationUserId = applicationUserId,
                TenantId = ResourceIdeaRequestContext.Tenant
            };

            var response = await Mediator.Send(query);
            if (response.IsSuccess && response.Content.Value is not null)
            {
                CurrentEmployee = response.Content.Value;
                
                // Load manager information if the employee has a manager
                if (CurrentEmployee.EmployeeId != EmployeeId.Empty)
                {
                    await LoadManagerName();
                }
            }
            else
            {
                CurrentEmployee = TenantEmployeeModel.Empty;
                message = "Unable to load your profile information.";
                isErrorMessage = true;
            }
        }, "Loading user profile", manageLoadingState: false);

        StateHasChanged();
    }

    private async Task LoadManagerName()
    {
        if (CurrentEmployee == null || CurrentEmployee.IsEmpty)
            return;

        await ExecuteAsync(async () =>
        {
            // Get the current employee's full data to access manager ID
            var employeeQuery = new GetEmployeeByIdQuery
            {
                EmployeeId = CurrentEmployee.EmployeeId,
                TenantId = ResourceIdeaRequestContext.Tenant
            };

            var employeeResponse = await Mediator.Send(employeeQuery);
            if (employeeResponse.IsSuccess && employeeResponse.Content.Value is not null)
            {
                var employee = employeeResponse.Content.Value;
                
                if (employee.ManagerId != EmployeeId.Empty)
                {
                    // Get manager information
                    var managerQuery = new GetEmployeeByIdQuery
                    {
                        EmployeeId = employee.ManagerId,
                        TenantId = ResourceIdeaRequestContext.Tenant
                    };

                    var managerResponse = await Mediator.Send(managerQuery);
                    if (managerResponse.IsSuccess && managerResponse.Content.Value is not null)
                    {
                        var manager = managerResponse.Content.Value;
                        ManagerName = $"{manager.FirstName} {manager.LastName}".Trim();
                    }
                }
            }
        }, "Loading manager information", manageLoadingState: false);
    }

    private async Task ResetPassword()
    {
        if (CurrentEmployee == null || string.IsNullOrEmpty(CurrentEmployee.Email))
        {
            message = "Unable to reset password. Email information is missing.";
            isErrorMessage = true;
            StateHasChanged();
            return;
        }

        await ExecuteAsync(async () =>
        {
            var resetCommand = new ResetEmployeePasswordCommand
            {
                EmployeeId = CurrentEmployee.EmployeeId,
                Email = CurrentEmployee.Email,
                TenantId = ResourceIdeaRequestContext.Tenant
            };

            var response = await Mediator.Send(resetCommand);
            if (response.IsSuccess && response.Content.Value is not null)
            {
                var temporaryPassword = response.Content.Value;
                message = $"Password reset successfully. Temporary password: {temporaryPassword}. You must change this password on your next login.";
                isErrorMessage = false;
            }
            else
            {
                message = "Failed to reset password. Please try again or contact your administrator.";
                isErrorMessage = true;
            }
        }, "Resetting password", manageLoadingState: false);

        StateHasChanged();
    }

    private void ClearMessage()
    {
        message = null;
        StateHasChanged();
    }
}