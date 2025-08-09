// ----------------------------------------------------------------------------------
// File: LogoutResultModel.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Authentication\Models\LogoutResultModel.cs
// Description: Model representing the result of a logout operation.
// ----------------------------------------------------------------------------------

namespace EastSeat.ResourceIdea.Application.Features.Authentication.Models;

/// <summary>
/// Represents the result of a logout operation.
/// </summary>
public class LogoutResultModel
{
    /// <summary>
    /// Gets or sets whether the logout was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the redirect URL after logout.
    /// </summary>
    public string? RedirectUrl { get; set; }

    /// <summary>
    /// Gets or sets any error message if logout failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Creates a successful logout result.
    /// </summary>
    /// <param name="redirectUrl">The URL to redirect to after logout</param>
    /// <returns>A successful logout result</returns>
    public static LogoutResultModel Success(string? redirectUrl = "/")
    {
        return new LogoutResultModel
        {
            IsSuccess = true,
            RedirectUrl = redirectUrl
        };
    }

    /// <summary>
    /// Creates a failed logout result.
    /// </summary>
    /// <param name="errorMessage">The error message</param>
    /// <returns>A failed logout result</returns>
    public static LogoutResultModel Failure(string errorMessage)
    {
        return new LogoutResultModel
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}