using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Account;

/// <summary>
/// Login page component that handles user authentication using ASP.NET Identity.
/// NOTE: This component deliberately does NOT inherit from ResourceIdeaComponentBase 
/// to avoid StateHasChanged() calls that interfere with authentication.
/// </summary>
public partial class Login : ComponentBase
{
    [Inject] private SignInManager<ApplicationUser> SignInManager { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private ILogger<Login> Logger { get; set; } = default!;

    private LoginModel loginModel = new();
    private string? errorMessage;

    /// <summary>
    /// Handles the login form submission using ASP.NET Identity SignInManager.
    /// CRITICAL: No state changes or StateHasChanged() calls before authentication to avoid headers being read-only.
    /// </summary>
    private async Task HandleLoginAsync()
    {
        // Clear any previous error but don't update isLoading yet
        errorMessage = null;
        
        try
        {
            // Perform authentication FIRST - no state changes before this point
            var result = await SignInManager.PasswordSignInAsync(
                loginModel.Username,
                loginModel.Password,
                loginModel.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Success - navigate immediately without updating component state
                Navigation.NavigateTo("/", forceLoad: true);
                return;
            }

            // Authentication failed - now safe to update UI
            errorMessage = result.IsLockedOut 
                ? "Account is locked out. Please try again later."
                : result.IsNotAllowed 
                    ? "Login not allowed. Please confirm your email if required."
                    : "Invalid username or password.";

            StateHasChanged(); // Safe to call now - authentication is complete
        }
        catch (Exception ex)
        {
            // Exception occurred - safe to update state
            errorMessage = "An unexpected error occurred during login. Please try again.";
            
            Logger.LogError(ex, "Exception during user login: {Message}", ex.Message);
            StateHasChanged(); // Safe to call now
        }
    }

    /// <summary>
    /// Model for login form data.
    /// </summary>
    public class LoginModel
    {
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
