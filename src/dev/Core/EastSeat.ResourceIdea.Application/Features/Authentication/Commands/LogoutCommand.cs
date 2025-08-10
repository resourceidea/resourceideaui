// ----------------------------------------------------------------------------------
// File: LogoutCommand.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Authentication\Commands\LogoutCommand.cs
// Description: Command to logout a user.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Authentication.Models;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Authentication.Commands;

/// <summary>
/// Represents a command to logout a user.
/// </summary>
public sealed class LogoutCommand : BaseRequest<LogoutResultModel>
{
    /// <summary>
    /// Gets or sets the return URL after logout.
    /// </summary>
    public string? ReturnUrl { get; set; }

    /// <summary>
    /// Validates the command.
    /// </summary>
    /// <returns><see cref="ValidationResponse"/></returns>
    public override ValidationResponse Validate()
    {
        var validationFailureMessages = new List<string>();

        // Validate return URL if provided
        if (!string.IsNullOrEmpty(ReturnUrl))
        {
            if (!IsValidReturnUrl(ReturnUrl))
            {
                validationFailureMessages.Add("Return URL must be a valid local path starting with '/'.");
            }
        }

        return validationFailureMessages.Any()
            ? new ValidationResponse(false, validationFailureMessages)
            : new ValidationResponse(true, []);
    }

    /// <summary>
    /// Validates that the return URL is a safe local path.
    /// </summary>
    /// <param name="returnUrl">The return URL to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    private static bool IsValidReturnUrl(string returnUrl)
    {
        // Only allow app-local absolute paths like "/employees"
        return Uri.TryCreate(returnUrl, UriKind.Relative, out _)
            && returnUrl.StartsWith("/", StringComparison.Ordinal)
            && !returnUrl.StartsWith("//", StringComparison.Ordinal);
    }
}