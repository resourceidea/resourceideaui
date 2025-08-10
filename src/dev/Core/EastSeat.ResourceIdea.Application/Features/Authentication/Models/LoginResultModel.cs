// ----------------------------------------------------------------------------------
// File: LoginResultModel.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Authentication\Models\LoginResultModel.cs
// Description: Model representing the result of a login operation.
// ----------------------------------------------------------------------------------

namespace EastSeat.ResourceIdea.Application.Features.Authentication.Models;

/// <summary>
/// Represents the result of a login operation.
/// </summary>
public class LoginResultModel
{
    /// <summary>
    /// Gets or sets whether the login was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the error message if login failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets whether the account is locked out.
    /// </summary>
    public bool IsLockedOut { get; set; }

    /// <summary>
    /// Gets or sets whether the account is not allowed to sign in.
    /// </summary>
    public bool IsNotAllowed { get; set; }

    /// <summary>
    /// Gets or sets the redirect URL after successful login.
    /// </summary>
    public string? RedirectUrl { get; set; }

    /// <summary>
    /// Creates a successful login result.
    /// </summary>
    /// <param name="redirectUrl">The URL to redirect to after login</param>
    /// <returns>A successful login result</returns>
    public static LoginResultModel Success(string? redirectUrl = null)
    {
        return new LoginResultModel
        {
            IsSuccess = true,
            RedirectUrl = redirectUrl
        };
    }

    /// <summary>
    /// Creates a failed login result.
    /// </summary>
    /// <param name="errorMessage">The error message</param>
    /// <param name="isLockedOut">Whether the account is locked out</param>
    /// <param name="isNotAllowed">Whether the account is not allowed</param>
    /// <returns>A failed login result</returns>
    public static LoginResultModel Failure(string errorMessage, bool isLockedOut = false, bool isNotAllowed = false)
    {
        return new LoginResultModel
        {
            IsSuccess = false,
            ErrorMessage = errorMessage,
            IsLockedOut = isLockedOut,
            IsNotAllowed = isNotAllowed
        };
    }
}