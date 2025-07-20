// ===============================================================================================
//  File: ResetEmployeePasswordCommand.cs
//  Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Employees\Commands\ResetEmployeePasswordCommand.cs
//  Description: Command to reset an employee's password.
// ================================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Extensions;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Commands;

/// <summary>
/// Command to reset an employee's password.
/// </summary>
public class ResetEmployeePasswordCommand : BaseRequest<string>
{
    /// <summary>
    /// Gets or sets the employee ID.
    /// </summary>
    public EmployeeId EmployeeId { get; set; }

    /// <summary>
    /// Gets or sets the employee's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Validates the command.
    /// This method checks if the required fields are filled and returns a ValidationResponse.
    /// </summary>
    /// <returns><see cref="ValidationResponse"/> </returns>
    public override ValidationResponse Validate()
    {
        var validationFailureMessages = new[]
        {
            Email.ValidateRequired(nameof(Email)),
        }
        .Where(message => !string.IsNullOrWhiteSpace(message));

        return validationFailureMessages.Any()
            ? new ValidationResponse(false, validationFailureMessages)
            : new ValidationResponse(true, []);
    }
}