// ----------------------------------------------------------------------------------
// File: LoginCommand.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Authentication\Commands\LoginCommand.cs
// Description: Command to authenticate a user login.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Authentication.Models;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Extensions;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Authentication.Commands;

/// <summary>
/// Represents a command to authenticate a user login.
/// </summary>
public sealed class LoginCommand : BaseRequest<LoginResultModel>
{
    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether to remember the user login.
    /// </summary>
    public bool RememberMe { get; set; }

    /// <summary>
    /// Gets or sets the return URL after successful login.
    /// </summary>
    public string? ReturnUrl { get; set; }

    /// <summary>
    /// Validates the command.
    /// </summary>
    /// <returns><see cref="ValidationResponse"/></returns>
    public override ValidationResponse Validate()
    {
        var validationFailureMessages = new[]
        {
            Email.ValidateRequired(nameof(Email)),
            ValidateEmailFormat(),
            Password.ValidateRequired(nameof(Password))
        }
        .Where(message => !string.IsNullOrWhiteSpace(message));

        return validationFailureMessages.Any()
            ? new ValidationResponse(false, validationFailureMessages)
            : new ValidationResponse(true, []);
    }

    /// <summary>
    /// Validates the email format.
    /// </summary>
    /// <returns>Validation error message or empty string if valid.</returns>
    private string ValidateEmailFormat()
    {
        if (!string.IsNullOrWhiteSpace(Email) && !IsValidEmail(Email))
        {
            return "Email address format is invalid.";
        }
        return string.Empty;
    }

    /// <summary>
    /// Validates email format using simple regex pattern.
    /// </summary>
    /// <param name="email">Email to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    private static bool IsValidEmail(string email)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(email, 
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$", 
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);
    }
}