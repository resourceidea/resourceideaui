// ----------------------------------------------------------------------------------
// File: AuthenticationService.cs
// Path: src\dev\Web\EastSeat.ResourceIdea.Web\Services\AuthenticationService.cs
// Description: Service for handling authentication operations.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Authentication.Contracts;
using EastSeat.ResourceIdea.Application.Features.Authentication.Models;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EastSeat.ResourceIdea.Web.Services;

/// <summary>
/// Service for handling authentication operations.
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ProtectedSessionStorage _protectedSessionStore;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(
        SignInManager<ApplicationUser> signInManager,
        AuthenticationStateProvider authenticationStateProvider,
        ProtectedSessionStorage protectedSessionStore,
        ILogger<AuthenticationService> logger)
    {
        _signInManager = signInManager;
        _authenticationStateProvider = authenticationStateProvider;
        _protectedSessionStore = protectedSessionStore;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<LoginResultModel>> LoginAsync(
        string email, 
        string password, 
        bool rememberMe, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _signInManager.PasswordSignInAsync(
                email,
                password,
                rememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User {Email} logged in successfully", email);
                return ResourceIdeaResponse<LoginResultModel>.Success(LoginResultModel.Success());
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User {Email} account is locked out", email);
                return ResourceIdeaResponse<LoginResultModel>.Success(
                    LoginResultModel.Failure("This account has been locked out. Please try again later.", isLockedOut: true));
            }

            if (result.IsNotAllowed)
            {
                _logger.LogWarning("User {Email} is not allowed to sign in", email);
                return ResourceIdeaResponse<LoginResultModel>.Success(
                    LoginResultModel.Failure("Sign in is not allowed for this account.", isNotAllowed: true));
            }

            _logger.LogWarning("Failed login attempt for user {Email}", email);
            return ResourceIdeaResponse<LoginResultModel>.Success(
                LoginResultModel.Failure("Invalid email or password."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user {Email}", email);
            return ResourceIdeaResponse<LoginResultModel>.Failure(ErrorCode.LoginFailed);
        }
    }

    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<LogoutResultModel>> LogoutAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _signInManager.SignOutAsync();
            
            // Clear any session storage that might contain stale data
            try
            {
                await _protectedSessionStore.DeleteAsync("UserSession");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error clearing session storage during logout");
                // Continue with logout even if session storage clear fails
            }

            _logger.LogInformation("User signed out successfully");
            return ResourceIdeaResponse<LogoutResultModel>.Success(LogoutResultModel.Success());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            return ResourceIdeaResponse<LogoutResultModel>.Failure(ErrorCode.DataStoreCommandFailure);
        }
    }

    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<UserValidationResult>> ValidateUserClaimsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user?.Identity?.IsAuthenticated != true)
            {
                return ResourceIdeaResponse<UserValidationResult>.Success(UserValidationResult.Failure("User is not authenticated"));
            }

            // Check if TenantId claim exists and is valid
            var tenantIdClaim = user.FindFirst("TenantId")?.Value?.Trim();
            if (string.IsNullOrEmpty(tenantIdClaim))
            {
                _logger.LogWarning("User {UserName} is missing TenantId claim", user.Identity.Name);
                return ResourceIdeaResponse<UserValidationResult>.Success(UserValidationResult.Failure("Missing TenantId claim"));
            }

            // Validate that TenantId is a valid GUID
            if (!Guid.TryParse(tenantIdClaim, out var tenantId) || tenantId == Guid.Empty)
            {
                _logger.LogWarning("User {UserName} has invalid TenantId claim: {TenantId}", user.Identity.Name, tenantIdClaim);
                return ResourceIdeaResponse<UserValidationResult>.Success(UserValidationResult.Failure("Invalid TenantId claim"));
            }

            // Check if IsBackendRole claim exists (this should be added by TenantClaimsTransformation)
            var isBackendRoleClaim = user.FindFirst("IsBackendRole")?.Value?.Trim();
            if (string.IsNullOrEmpty(isBackendRoleClaim))
            {
                _logger.LogWarning("User {UserName} is missing IsBackendRole claim", user.Identity.Name);
                return ResourceIdeaResponse<UserValidationResult>.Success(UserValidationResult.Failure("Missing IsBackendRole claim"));
            }

            _logger.LogInformation("User {UserName} has valid claims. TenantId: {TenantId}, IsBackendRole: {IsBackendRole}", 
                user.Identity.Name, tenantIdClaim, isBackendRoleClaim);
            return ResourceIdeaResponse<UserValidationResult>.Success(UserValidationResult.Success());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating user claims");
            return ResourceIdeaResponse<UserValidationResult>.Failure(ErrorCode.DataStoreQueryFailure);
        }
    }
}