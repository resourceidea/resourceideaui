// ----------------------------------------------------------------------------------
// File: LogoutCommandHandler.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Authentication\Handlers\LogoutCommandHandler.cs
// Description: Handler for the LogoutCommand.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Authentication.Commands;
using EastSeat.ResourceIdea.Application.Features.Authentication.Contracts;
using EastSeat.ResourceIdea.Application.Features.Authentication.Models;
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
            var failureResult = LogoutResultModel.Failure(errorMessage);
            return ResourceIdeaResponse<LogoutResultModel>.Success(failureResult);
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
            logoutResult.Content.Value.RedirectUrl = request.ReturnUrl ?? "/";
        }

        return logoutResult;
    }
}