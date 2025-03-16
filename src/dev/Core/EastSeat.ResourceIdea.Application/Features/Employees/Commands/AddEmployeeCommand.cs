// ===============================================================================================
//  File: HireEmployeeCommand.cs
//  Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Employees\Commands\HireEmployeeCommand.cs
//  Description: Command to hire an employee. This command is used to create a new employee record in the system.
// ================================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Extensions;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Commands;

/// <summary>
/// Command to hire an employee.
/// </summary>
public class AddEmployeeCommand : BaseRequest<Employee>
{
    public EmployeeId EmployeeId { get; set; }

    public string EmployeeNumber { get; set; } = string.Empty;

    public JobPositionId JobPositionId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

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
            EmployeeNumber.ValidateRequired(nameof(EmployeeNumber)),
            JobPositionId.ValidateRequired(),
            EmployeeId.ValidateRequired(),
            FirstName.ValidateRequired(nameof(FirstName)),
            LastName.ValidateRequired(nameof(LastName)),
            Email.ValidateRequired(nameof(Email)),
        }
        .Where(message => !string.IsNullOrWhiteSpace(message));

        return validationFailureMessages.Any()
            ? new ValidationResponse(false, validationFailureMessages)
            : new ValidationResponse(true, []);
    }
}
