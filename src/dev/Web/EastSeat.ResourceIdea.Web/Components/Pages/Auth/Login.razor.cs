using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Domain.Users.Models;
using EastSeat.ResourceIdea.Web.Components.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Auth;

public partial class Login : ResourceIdeaComponentBase
{
    [Inject] private SignInManager<ApplicationUser> SignInManager { get; set; } = default!;
    [Inject] private UserManager<ApplicationUser> UserManager { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    private LoginModel loginModel { get; set; } = new();

    [SupplyParameterFromQuery]
    public string? ReturnUrl { get; set; }
    private async Task HandleLoginWithLoadingState()
    {
        IsLoading = true;
        StateHasChanged(); // Show loading state immediately

        Microsoft.AspNetCore.Identity.SignInResult? signInResult = null;

        try
        {
            ClearError();

            // Find user by email first, then use username for sign-in
            var user = await UserManager.FindByEmailAsync(loginModel.Email!);
            if (user == null)
            {
                SetError("Invalid email or password.");
                return;
            }

            signInResult = await SignInManager.PasswordSignInAsync(
                user.UserName!,
                loginModel.Password!,
                isPersistent: false,
                lockoutOnFailure: false);

            if (signInResult.Succeeded)
            {
                // Successful login - navigation will handle page transition
                // Don't call StateHasChanged here to avoid interfering with authentication
                Navigation.NavigateTo(ReturnUrl ?? "/");
                return;
            }
            else
            {
                SetError("Invalid email or password.");
            }
        }
        catch (Exception ex)
        {
            SetError("An error occurred during login. Please try again.");
            // Log the exception for debugging without using centralized handling
            Console.WriteLine($"Login error: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
            // Only update UI if login failed or threw exception
            // Successful logins will navigate away, so no need to update state
            if (signInResult?.Succeeded != true)
            {
                StateHasChanged();
            }
        }
    }

    /// <summary>
    /// Gets the current error message to display for authentication errors.
    /// </summary>
    public string GetDisplayErrorMessage()
    {
        return ErrorMessage ?? string.Empty;
    }

    /// <summary>
    /// Handles invalid form submission to help with debugging.
    /// </summary>
    private void HandleInvalidSubmit()
    {
        // This helps identify when validation is failing
        Console.WriteLine($"Form validation failed. Email: '{loginModel.Email}', Password length: {loginModel.Password?.Length ?? 0}");
        SetError("Please fill in all required fields.");
    }
}