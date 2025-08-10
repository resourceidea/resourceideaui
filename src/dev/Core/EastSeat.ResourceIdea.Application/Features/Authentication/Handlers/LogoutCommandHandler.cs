// ----------------------------------------------------------------------------------
// File: LogoutCommandHandler.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Authentication\Handlers\LogoutCommandHandler.cs
// Description: Handler for the LogoutCommand.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Authentication.Commands;
using EastSeat.ResourceIdea.Application.Features.Authentication.Contracts;
using EastSeat.ResourceIdea.Application.Features.Authentication.Models;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Authentication.Handlers;

/// <summary>
/// Handler for the LogoutCommand.
/// </summary>
public sealed class LogoutCommandHandler(IAuthenticationService authenticationService)
    : IRequestHandler<LogoutCommand, ResourceIdeaResponse<LogoutResultModel>>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<LogoutResultModel>> Handle(
        LogoutCommand request,
        CancellationToken cancellationToken)
    {
        // Validate the command (though logout should always be allowed)
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
        {
            var errorMessage = string.Join("; ", validationResult.ValidationFailureMessages);
            return ResourceIdeaResponse<LogoutResultModel>.Failure(ErrorCode.CommandValidationFailure);
        }

        // Perform the logout
        var logoutResult = await _authenticationService.LogoutAsync(cancellationToken);

        if (logoutResult.IsFailure)
        {
            return ResourceIdeaResponse<LogoutResultModel>.Failure(logoutResult.Error);
        }

        // Set the redirect URL
        if (logoutResult.Content.HasValue && logoutResult.Content.Value.IsSuccess)
        {
            logoutResult.Content.Value.RedirectUrl = GetSafeReturnUrl(request.ReturnUrl);
        }

        return logoutResult;
    }

    /// <summary>
    /// Gets a safe return URL, ensuring it's a valid local path.
    /// </summary>
    /// <param name="returnUrl">The requested return URL</param>
    /// <returns>A safe return URL</returns>
    private static string GetSafeReturnUrl(string? returnUrl)
    {
        // Only allow app-local absolute paths like "/employees"
        if (string.IsNullOrWhiteSpace(returnUrl))
            return "/";

        if (Uri.TryCreate(returnUrl, UriKind.Relative, out _)
            && returnUrl.StartsWith("/", StringComparison.Ordinal)
            && !returnUrl.StartsWith("//", StringComparison.Ordinal))
        {
            return returnUrl;
        }

        return "/";
    }
}