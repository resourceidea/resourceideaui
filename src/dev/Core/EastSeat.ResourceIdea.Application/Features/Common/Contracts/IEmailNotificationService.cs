// =============================================================================================
// File: IEmailNotificationService.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Common\Contracts\IEmailNotificationService.cs
// Description: Interface for email notification service for password reset and other notifications.
// =============================================================================================

using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Common.Contracts;

/// <summary>
/// Interface for email notification service.
/// </summary>
public interface IEmailNotificationService
{
    /// <summary>
    /// Send password reset notification email.
    /// </summary>
    /// <param name="email">User's email address</param>
    /// <param name="temporaryPassword">The temporary password</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="ResourceIdeaResponse{string}"/> indicating success or failure with message</returns>
    Task<ResourceIdeaResponse<string>> SendPasswordResetNotificationAsync(
        string email, 
        string temporaryPassword, 
        CancellationToken cancellationToken = default);
}