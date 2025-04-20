// ===============================================================================================
//  File: UpdateEmployeeCommand.cs
//  Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Employees\Commands\UpdateEmployeeCommand.cs
//  Description: Command to update an employee's details.
// ================================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Extensions;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Commands;

/// <summary>
/// Command to update an employee's details.
/// </summary>
public class UpdateEmployeeCommand : BaseRequest<EmployeeModel>
{
    /// <summary>
    /// Gets or sets the employee ID.
    /// </summary>
    public EmployeeId EmployeeId { get; set; }

    /// <summary>
    /// Gets or sets the first name of the employee.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the last name of the employee.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email of the employee.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the job position ID of the employee.
    /// </summary>
    public JobPositionId JobPositionId { get; set; }

    /// <summary>
    /// Gets or sets the department ID.
    /// </summary>
    public DepartmentId DepartmentId { get; set; }

    /// <summary>
    /// Maps the command to an Employee entity.
    /// </summary>
    /// <returns>An Employee entity populated with data from this command.</returns>
    public Employee ToEntity() => new()
    {
        EmployeeId = EmployeeId,
        FirstName = FirstName,
        LastName = LastName,
        Email = Email,
        TenantId = TenantId,
        JobPositionId = JobPositionId
    };

    /// <summary>
    /// Validates the command.
    /// This method checks if the required fields are filled and returns a ValidationResponse.
    /// </summary>
    /// <returns><see cref="ValidationResponse"/> </returns>
    public override ValidationResponse Validate()
    {
        var validationFailureMessages = new[]
        {
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
