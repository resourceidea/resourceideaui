using EastSeat.ResourceIdea.Application.Features.Employees.Contracts;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Web.Components.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Backend;

public partial class DevTools : ResourceIdeaComponentBase
{
    [Inject] private IApplicationUserService ApplicationUserService { get; set; } = default!;
    [Inject] private UserManager<ApplicationUser> UserManager { get; set; } = default!;
    [Inject] private ILogger<DevTools> Logger { get; set; } = default!;

    private CreateUserModel createUserModel = new();
    private List<ApplicationUser>? users;
    private bool IsLoadingUsers { get; set; }

    // Custom success message property
    private string? SuccessMessage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await ExecuteAsync(async () =>
        {
            await LoadUsers();
        }, "Loading development tools");
    }

    private async Task CreateTestUser()
    {
        await ExecuteAsync(async () =>
        {
            // First, create the user using our service
            var tenantId = TenantId.Create(createUserModel.TenantIdValue);
            var result = await ApplicationUserService.AddApplicationUserAsync(
                createUserModel.FirstName,
                createUserModel.LastName,
                createUserModel.Email,
                tenantId);

            if (result.IsSuccess && result.Content.HasValue)
            {
                // The service creates a temporary password, but we want to set our own
                var user = await UserManager.FindByEmailAsync(createUserModel.Email);
                if (user != null)
                {
                    // Remove the temporary password and set our own
                    await UserManager.RemovePasswordAsync(user);
                    var passwordResult = await UserManager.AddPasswordAsync(user, createUserModel.Password);

                    if (passwordResult.Succeeded)
                    {
                        // Confirm the email for test users
                        user.EmailConfirmed = true;
                        await UserManager.UpdateAsync(user);

                        SetSuccessMessage($"Test user '{createUserModel.Email}' created successfully with TenantId '{createUserModel.TenantIdValue}'!");
                        createUserModel = new CreateUserModel(); // Reset form
                        await LoadUsers(); // Refresh the list
                    }
                    else
                    {
                        var errors = string.Join(", ", passwordResult.Errors.Select(e => e.Description));
                        throw new InvalidOperationException($"User created but password setup failed: {errors}");
                    }
                }
                else
                {
                    throw new InvalidOperationException("User created but could not be found for password setup.");
                }
            }
            else
            {
                throw new InvalidOperationException("Failed to create user. Please check the inputs and try again.");
            }
        }, "Creating test user");
    }

    private Task LoadUsers()
    {
        IsLoadingUsers = true;
        StateHasChanged();

        try
        {
            // Get all users from the UserManager
            users = UserManager.Users.ToList();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error loading users");
            SetError("Failed to load users.");
        }
        finally
        {
            IsLoadingUsers = false;
            StateHasChanged();
        }

        return Task.CompletedTask;
    }

    private void ResetForm()
    {
        createUserModel = new CreateUserModel();
        ClearError();
        ClearSuccessMessage();
    }

    private void SetSuccessMessage(string message)
    {
        SuccessMessage = message;
        ClearError();
        StateHasChanged();
    }

    private void ClearSuccessMessage()
    {
        SuccessMessage = null;
    }

    private class CreateUserModel
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tenant ID is required")]
        public string TenantIdValue { get; set; } = string.Empty;
    }
}