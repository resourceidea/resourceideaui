// ----------------------------------------------------------------------------------
// File: UserValidationResult.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Authentication\Models\UserValidationResult.cs
// Description: Model representing the result of user validation.
// ----------------------------------------------------------------------------------

namespace EastSeat.ResourceIdea.Application.Features.Authentication.Models;

/// <summary>
/// Represents the result of user validation.
/// </summary>
public class UserValidationResult
{
    /// <summary>
    /// Gets or sets whether the user validation was successful.
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// Gets or sets any error message if validation failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Creates a successful validation result.
    /// </summary>
    /// <returns>A successful validation result</returns>
    public static UserValidationResult Success()
    {
        return new UserValidationResult
        {
            IsValid = true
        };
    }

    /// <summary>
    /// Creates a failed validation result.
    /// </summary>
    /// <param name="errorMessage">The error message</param>
    /// <returns>A failed validation result</returns>
    public static UserValidationResult Failure(string errorMessage)
    {
        return new UserValidationResult
        {
            IsValid = false,
            ErrorMessage = errorMessage
        };
    }
}