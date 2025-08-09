// ----------------------------------------------------------------------------------
// File: LoginCommandHandler.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Authentication\Handlers\LoginCommandHandler.cs
// Description: Handler for the LoginCommand.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Authentication.Commands;
using EastSeat.ResourceIdea.Application.Features.Authentication.Contracts;
using EastSeat.ResourceIdea.Application.Features.Authentication.Models;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Authentication.Handlers;

/// <summary>
/// Handler for the LoginCommand.
/// </summary>
public sealed class LoginCommandHandler(IAuthenticationService authenticationService)
    : IRequestHandler<LoginCommand, ResourceIdeaResponse<LoginResultModel>>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<LoginResultModel>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        // Validate the command
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
        {
            var errorMessage = string.Join("; ", validationResult.ValidationFailureMessages);
            return ResourceIdeaResponse<LoginResultModel>.Failure(ErrorCode.LoginCommandValidationFailure);
        }

        // Attempt to authenticate the user
        var loginResult = await _authenticationService.LoginAsync(
            request.Email,
            request.Password,
            request.RememberMe,
            cancellationToken);

        if (loginResult.IsFailure)
        {
            return ResourceIdeaResponse<LoginResultModel>.Failure(loginResult.Error);
        }

        // Set the redirect URL if login was successful
        if (loginResult.Content.HasValue && loginResult.Content.Value.IsSuccess)
        {
            loginResult.Content.Value.RedirectUrl = request.ReturnUrl ?? "/employees";
        }

        return loginResult;
    }
}