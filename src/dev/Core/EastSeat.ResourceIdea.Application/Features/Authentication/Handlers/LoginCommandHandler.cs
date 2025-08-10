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
using Microsoft.Extensions.Logging;

namespace EastSeat.ResourceIdea.Application.Features.Authentication.Handlers;

/// <summary>
/// Handler for the LoginCommand.
/// </summary>
public sealed class LoginCommandHandler(IAuthenticationService authenticationService, ILogger<LoginCommandHandler> logger)
    : IRequestHandler<LoginCommand, ResourceIdeaResponse<LoginResultModel>>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    private readonly ILogger<LoginCommandHandler> _logger = logger;

    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<LoginResultModel>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        // Validate the command
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
        {
            var sanitizedErrorMessage = GetSanitizedValidationErrors(validationResult.ValidationFailureMessages);
            _logger.LogWarning("Login command validation failed at {Timestamp}: {ErrorCount} validation errors", 
                DateTime.UtcNow, validationResult.ValidationFailureMessages.Count());
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
            loginResult.Content.Value.RedirectUrl = GetSafeReturnUrl(request.ReturnUrl);
        }

        return loginResult;
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
            return "/employees";

        if (Uri.TryCreate(returnUrl, UriKind.Relative, out _)
            && returnUrl.StartsWith("/", StringComparison.Ordinal)
            && !returnUrl.StartsWith("//", StringComparison.Ordinal))
        {
            return returnUrl;
        }

        return "/employees";
    }

    /// <summary>
    /// Gets sanitized validation error messages without exposing sensitive information.
    /// </summary>
    /// <param name="validationMessages">The original validation messages</param>
    /// <returns>Sanitized error message</returns>
    private static string GetSanitizedValidationErrors(IEnumerable<string> validationMessages)
    {
        // Return a generic message without exposing the specific validation details
        // to prevent information disclosure
        return "Validation failed for the login request.";
    }
}