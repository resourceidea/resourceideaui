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
    [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;

    private LoginModel loginModel { get; set; } = new();

    [SupplyParameterFromQuery]
    public string? ReturnUrl { get; set; }

    /// <summary>
    /// Checks if the component is running in an interactive context with HttpContext available.
    /// </summary>
    private bool IsInteractiveWithHttpContext =>
        HttpContextAccessor?.HttpContext != null && !HttpContextAccessor.HttpContext.Response.HasStarted;

    private async Task HandleLoginWithLoadingState()
    {
        if (!IsInteractiveWithHttpContext)
        {
            SetError("Authentication services are temporarily unavailable. Please refresh and try again.");
            return;
        }

        IsLoading = true;
        StateHasChanged(); // Show loading state immediately

        try
        {
            await ExecuteAsync(async () =>
            {
                ClearError();

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
                    // Don't call StateHasChanged here to avoid interfering with authentication
                    Navigation.NavigateTo(ReturnUrl ?? "/");
                    return;
                }
                else
                {
                    SetError("Invalid email or password.");
                }
            }, "Login authentication");
        }
        finally
        {
            IsLoading = false;
            StateHasChanged();
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