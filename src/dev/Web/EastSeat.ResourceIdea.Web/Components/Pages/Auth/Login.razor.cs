using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Domain.Users.Models;
using EastSeat.ResourceIdea.Web.Components.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Auth;

public partial class Login
{
    [Inject] private SignInManager<ApplicationUser> SignInManager { get; set; } = default!;
    [Inject] private UserManager<ApplicationUser> UserManager { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    private readonly LoginModel loginModel = new();
    private string authErrorMessage = string.Empty;
    private bool isLoading = false;

    [SupplyParameterFromQuery]
    public string? ReturnUrl { get; set; }

    private async Task HandleLogin()
    {
        await ExecuteAsync(async () =>
        {
            var result = await SignInManager.PasswordSignInAsync(
                loginModel.Email!,
                loginModel.Password!,
                isPersistent: false,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                Navigation.NavigateTo(ReturnUrl ?? "/departments", forceLoad: true);
            }
            else
            {
                // Set authentication-specific error message
                authErrorMessage = "Invalid email or password.";
                StateHasChanged();
            }
        }, "User login", manageLoadingState: false);
    }

    private async Task HandleLoginWithLoadingState()
    {
        isLoading = true;
        authErrorMessage = string.Empty;
        ClearError();
        StateHasChanged();

        try
        {
            await HandleLogin();
        }
        finally
        {
            isLoading = false;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Gets the current error message to display, prioritizing authentication-specific errors.
    /// </summary>
    protected string GetDisplayErrorMessage()
    {
        return !string.IsNullOrEmpty(authErrorMessage) ? authErrorMessage : (ErrorMessage ?? string.Empty);
    }
}