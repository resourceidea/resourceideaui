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
        // Logout command doesn't require validation - it should always be allowed
        return new ValidationResponse(true, []);
    }
}