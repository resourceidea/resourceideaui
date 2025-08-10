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
using System.Diagnostics;
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
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _authenticationStateProvider = authenticationStateProvider ?? throw new ArgumentNullException(nameof(authenticationStateProvider));
        _protectedSessionStore = protectedSessionStore ?? throw new ArgumentNullException(nameof(protectedSessionStore));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<LoginResultModel>> LoginAsync(
        string email,
        string password,
        bool rememberMe,
        CancellationToken cancellationToken = default)
    {
        using var activity = Activity.Current?.Source.StartActivity("AuthenticationService.LoginAsync");
        activity?.SetTag("operation", "login");
        activity?.SetTag("rememberMe", rememberMe.ToString());

        try
        {
            var result = await _signInManager.PasswordSignInAsync(
                email,
                password,
                rememberMe,
                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in successfully at {Timestamp}. RememberMe: {RememberMe}",
                    DateTime.UtcNow, rememberMe);
                activity?.SetTag("result", "success");
                return ResourceIdeaResponse<LoginResultModel>.Success(LoginResultModel.Success());
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account is locked out at {Timestamp}", DateTime.UtcNow);
                activity?.SetTag("result", "locked_out");
                return ResourceIdeaResponse<LoginResultModel>.Success(
                    LoginResultModel.Failure("This account has been locked out. Please try again later.", isLockedOut: true));
            }

            if (result.IsNotAllowed)
            {
                _logger.LogWarning("User is not allowed to sign in at {Timestamp}", DateTime.UtcNow);
                activity?.SetTag("result", "not_allowed");
                return ResourceIdeaResponse<LoginResultModel>.Success(
                    LoginResultModel.Failure("Sign in is not allowed for this account.", isNotAllowed: true));
            }

            _logger.LogWarning("Failed login attempt at {Timestamp}", DateTime.UtcNow);
            activity?.SetTag("result", "failed");
            return ResourceIdeaResponse<LoginResultModel>.Success(
                LoginResultModel.Failure("Invalid email or password."));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Invalid operation during login at {Timestamp}", DateTime.UtcNow);
            activity?.SetTag("result", "error");
            activity?.SetTag("error_type", "invalid_operation");
            return ResourceIdeaResponse<LoginResultModel>.Failure(ErrorCode.LoginFailed);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid arguments during login at {Timestamp}", DateTime.UtcNow);
            activity?.SetTag("result", "error");
            activity?.SetTag("error_type", "invalid_arguments");
            return ResourceIdeaResponse<LoginResultModel>.Failure(ErrorCode.LoginFailed);
        }
        catch (NotSupportedException ex)
        {
            _logger.LogError(ex, "Unsupported operation during login at {Timestamp}", DateTime.UtcNow);
            activity?.SetTag("result", "error");
            activity?.SetTag("error_type", "not_supported");
            return ResourceIdeaResponse<LoginResultModel>.Failure(ErrorCode.LoginFailed);
        }
    }

    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<LogoutResultModel>> LogoutAsync(CancellationToken cancellationToken = default)
    {
        using var activity = Activity.Current?.Source.StartActivity("AuthenticationService.LogoutAsync");
        activity?.SetTag("operation", "logout");

        try
        {
            await _signInManager.SignOutAsync();

            // Best-effort clear any session storage that might contain stale data
            await TryClearUserSessionAsync();

            _logger.LogInformation("User signed out successfully at {Timestamp}", DateTime.UtcNow);
            activity?.SetTag("result", "success");
            return ResourceIdeaResponse<LogoutResultModel>.Success(LogoutResultModel.Success());
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Invalid operation during logout at {Timestamp}", DateTime.UtcNow);
            activity?.SetTag("result", "error");
            activity?.SetTag("error_type", "invalid_operation");
            return ResourceIdeaResponse<LogoutResultModel>.Failure(ErrorCode.DataStoreCommandFailure);
        }
        catch (NotSupportedException ex)
        {
            _logger.LogError(ex, "Unsupported operation during logout at {Timestamp}", DateTime.UtcNow);
            activity?.SetTag("result", "error");
            activity?.SetTag("error_type", "not_supported");
            return ResourceIdeaResponse<LogoutResultModel>.Failure(ErrorCode.DataStoreCommandFailure);
        }
    }

    private async Task TryClearUserSessionAsync()
    {
        try
        {
            await _protectedSessionStore.DeleteAsync("UserSession");
        }
        catch (Exception ex)
        {
            // Continue with logout even if session storage clear fails
            _logger.LogWarning(ex, "Error clearing session storage during logout at {Timestamp}", DateTime.UtcNow);
        }
    }

    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<UserValidationResult>> ValidateUserClaimsAsync(CancellationToken cancellationToken = default)
    {
        using var activity = Activity.Current?.Source.StartActivity("AuthenticationService.ValidateUserClaimsAsync");
        activity?.SetTag("operation", "validate_user_claims");

        try
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user?.Identity?.IsAuthenticated != true)
            {
                activity?.SetTag("result", "not_authenticated");
                return ResourceIdeaResponse<UserValidationResult>.Success(UserValidationResult.Failure("User is not authenticated"));
            }

            // Check if TenantId claim exists and is valid
            var tenantIdClaim = user.FindFirst("TenantId")?.Value?.Trim();
            if (string.IsNullOrEmpty(tenantIdClaim))
            {
                _logger.LogWarning("User is missing TenantId claim at {Timestamp}", DateTime.UtcNow);
                activity?.SetTag("result", "missing_tenant_claim");
                return ResourceIdeaResponse<UserValidationResult>.Success(UserValidationResult.Failure("Missing TenantId claim"));
            }

            // Validate that TenantId is a valid GUID
            if (!Guid.TryParse(tenantIdClaim, out var tenantId) || tenantId == Guid.Empty)
            {
                _logger.LogWarning("User has invalid TenantId claim at {Timestamp}", DateTime.UtcNow);
                activity?.SetTag("result", "invalid_tenant_claim");
                return ResourceIdeaResponse<UserValidationResult>.Success(UserValidationResult.Failure("Invalid TenantId claim"));
            }

            // Check if IsBackendRole claim exists (this should be added by TenantClaimsTransformation)
            var isBackendRoleClaim = user.FindFirst("IsBackendRole")?.Value?.Trim();
            if (string.IsNullOrEmpty(isBackendRoleClaim))
            {
                _logger.LogWarning("User is missing IsBackendRole claim at {Timestamp}", DateTime.UtcNow);
                activity?.SetTag("result", "missing_backend_role_claim");
                return ResourceIdeaResponse<UserValidationResult>.Success(UserValidationResult.Failure("Missing IsBackendRole claim"));
            }

            _logger.LogInformation("User has valid claims at {Timestamp}", DateTime.UtcNow);
            activity?.SetTag("result", "success");
            return ResourceIdeaResponse<UserValidationResult>.Success(UserValidationResult.Success());
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Invalid operation during user claims validation at {Timestamp}", DateTime.UtcNow);
            activity?.SetTag("result", "error");
            activity?.SetTag("error_type", "invalid_operation");
            return ResourceIdeaResponse<UserValidationResult>.Failure(ErrorCode.DataStoreQueryFailure);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid arguments during user claims validation at {Timestamp}", DateTime.UtcNow);
            activity?.SetTag("result", "error");
            activity?.SetTag("error_type", "invalid_arguments");
            return ResourceIdeaResponse<UserValidationResult>.Failure(ErrorCode.DataStoreQueryFailure);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Unauthorized access during user claims validation at {Timestamp}", DateTime.UtcNow);
            activity?.SetTag("result", "error");
            activity?.SetTag("error_type", "unauthorized");
            return ResourceIdeaResponse<UserValidationResult>.Failure(ErrorCode.DataStoreQueryFailure);
        }
    }
}