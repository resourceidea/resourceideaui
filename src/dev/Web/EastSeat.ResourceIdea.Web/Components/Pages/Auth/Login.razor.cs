using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Domain.Users.Models;
using EastSeat.ResourceIdea.Web.Components.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Auth;

public partial class Login : ResourceIdeaComponentBase
{
    [Inject] private SignInManager<ApplicationUser> SignInManager { get; set; } = default!;
    [Inject] private UserManager<ApplicationUser> UserManager { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private LoginModel loginModel = new();

    [SupplyParameterFromQuery]
    public string? ReturnUrl { get; set; }

    private async Task HandleLoginWithLoadingState()
    {
        await ExecuteAsync(async () =>
        {
            // Validate input before proceeding
            if (string.IsNullOrWhiteSpace(loginModel.Email) || string.IsNullOrWhiteSpace(loginModel.Password))
            {
                throw new InvalidOperationException("Please fill in all required fields.");
            }

            // Find user by email first, then use username for sign-in
            var user = await UserManager.FindByEmailAsync(loginModel.Email!);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid email or password.");
            }

            var signInResult = await SignInManager.PasswordSignInAsync(
                user.UserName!,
                loginModel.Password!,
                isPersistent: false,
                lockoutOnFailure: false);

            if (signInResult.Succeeded)
            {
                // Successful login - use JavaScript redirect to avoid Blazor Server response issues
                var redirectUrl = ReturnUrl ?? "/";
                await JSRuntime.InvokeVoidAsync("window.location.href", redirectUrl);
                return;
            }
            else if (signInResult.IsLockedOut)
            {
                throw new InvalidOperationException("Account is locked out. Please try again later.");
            }
            else if (signInResult.RequiresTwoFactor)
            {
                throw new InvalidOperationException("Two-factor authentication is required.");
            }
            else
            {
                throw new InvalidOperationException("Invalid email or password.");
            }
        }, "User login attempt");
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