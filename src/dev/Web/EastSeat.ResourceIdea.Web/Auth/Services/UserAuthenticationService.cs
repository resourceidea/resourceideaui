using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Domain.Users.Models;

using Microsoft.AspNetCore.Identity;

namespace EastSeat.ResourceIdea.Web.Auth.Services;

/// <summary>
/// Service implementation for authenticating users.
/// </summary>
/// <param name="userManager"></param>
/// <param name="signInManager"></param>
public class UserAuthenticationService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager) : IUserAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

    /// <inheritdoc/>
    public async Task<ResourceIdeaResponse<LoginModel>> LoginAsync(
        LoginModel loginModel,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(loginModel.Email);
        if (user is null)
        {
            return ResourceIdeaResponse<LoginModel>.Failure(ErrorCode.UserNotFound);
        }

        var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, isPersistent: false, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            return ResourceIdeaResponse<LoginModel>.Success(Optional<LoginModel>.Some(loginModel));
        }

        return ResourceIdeaResponse<LoginModel>.Failure(ErrorCode.LoginFailed);
    }

    /// <inheritdoc/>
    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}