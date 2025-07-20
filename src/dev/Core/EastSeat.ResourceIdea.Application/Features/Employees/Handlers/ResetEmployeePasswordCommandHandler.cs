// =========================================================================================
// File: ResetEmployeePasswordCommandHandler.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Employees\Handlers\ResetEmployeePasswordCommandHandler.cs
// Description: Handles the command to reset an employee's password.
// =========================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.Employees.Commands;
using EastSeat.ResourceIdea.Application.Features.Employees.Contracts;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Handlers;

public class ResetEmployeePasswordCommandHandler(IApplicationUserService applicationUserService)
    : BaseHandler, IRequestHandler<ResetEmployeePasswordCommand, ResourceIdeaResponse<string>>
{
    private readonly IApplicationUserService _applicationUserService = applicationUserService;

    public async Task<ResourceIdeaResponse<string>> Handle(
        ResetEmployeePasswordCommand command,
        CancellationToken cancellationToken)
    {
        ValidationResponse commandValidation = command.Validate();
        if (!commandValidation.IsValid && commandValidation.ValidationFailureMessages.Any())
        {
            // TODO: Log validation failure.
            return ResourceIdeaResponse<string>.Failure(ErrorCode.CommandValidationFailure);
        }

        var resetPasswordResponse = await _applicationUserService.ResetPasswordAsync(command.Email);
        if (resetPasswordResponse.IsFailure)
        {
            return ResourceIdeaResponse<string>.Failure(resetPasswordResponse.Error);
        }

        // TODO: Send email notification with new password
        // For now, we'll just return the temporary password to display in UI
        
        return resetPasswordResponse;
    }
}