using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Domain.Users.Models;
using EastSeat.ResourceIdea.Web.Components.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Web;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Auth;

public partial class Login : ResourceIdeaComponentBase
{
    [Inject] private SignInManager<ApplicationUser> SignInManager { get; set; } = default!;
    [Inject] private UserManager<ApplicationUser> UserManager { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    private LoginModel loginModel = new();

    [SupplyParameterFromQuery]
    public string? ReturnUrl { get; set; }

    private async Task HandleLoginWithLoadingState()
    {
        // Clear any previous errors
        ClearError();

        // Set loading state manually for authentication operations
        IsLoading = true;
        SafeStateHasChanged();

        try
        {
            // Validate input before proceeding
            if (string.IsNullOrWhiteSpace(loginModel.Email) || string.IsNullOrWhiteSpace(loginModel.Password))
            {
                SetError("Please fill in all required fields.");
                return;
            }

            // Find user by email first, then use username for sign-in
            var user = await UserManager.FindByEmailAsync(loginModel.Email!);
            if (user == null)
            {
                SetError("Invalid email or password.");
                return;
            }

            var signInResult = await SignInManager.PasswordSignInAsync(
                user.UserName!,
                loginModel.Password!,
                isPersistent: false,
                lockoutOnFailure: false);

            if (signInResult.Succeeded)
            {
                // Successful login - navigation will handle page transition
                Navigation.NavigateTo(ReturnUrl ?? "/", forceLoad: true);
                return;
            }
            else if (signInResult.IsLockedOut)
            {
                SetError("Account is locked out. Please try again later.");
            }
            else if (signInResult.RequiresTwoFactor)
            {
                SetError("Two-factor authentication is required.");
            }
            else
            {
                SetError("Invalid email or password.");
            }
        }
        catch (Exception ex)
        {
            // Handle unexpected exceptions during login
            SetError("An error occurred during login. Please try again.");
            // Log the exception for debugging purposes
            Console.WriteLine($"Login error: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
            SafeStateHasChanged();
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