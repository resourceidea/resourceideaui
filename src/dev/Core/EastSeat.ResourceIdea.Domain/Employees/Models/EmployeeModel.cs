// ----------------------------------------------------------------------------------
// File: EmployeeModel.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Domain\Employees\Models\EmployeeModel.cs
// Description: Defines the model for an employee.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Extensions;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.Employees.Models;

/// <summary>
/// Employee model.
/// </summary>
public class EmployeeModel
{
    /// <summary>
    /// Gets or sets the employee identifier.
    /// </summary>
    public EmployeeId EmployeeId { get; set; }

    /// <summary>
    /// Gets or sets the job position identifier.
    /// </summary>
    public JobPositionId JobPositionId { get; set; }

    /// <summary>
    /// Gets or sets the department identifier.
    /// </summary>
    public DepartmentId DepartmentId { get; set; }

    /// <summary>
    /// Gets or sets the application user identifier.
    /// </summary>
    public ApplicationUserId ApplicationUserId { get; set; }

    /// <summary>
    /// Gets or sets the employee number.
    /// </summary>
    public string EmployeeNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the manager identifier.
    /// </summary>
    public EmployeeId ManagerId { get; set; }

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
    /// Validates the model.
    /// </summary>
    /// <returns><see cref="ValidationResponse"/></returns>
    public ValidationResponse IsValid()
    {
        var validationFailureMessages = new[]
        {
            EmployeeId.ValidateRequired(),
            JobPositionId.ValidateRequired(),
            ApplicationUserId.ValidateRequired(),
            ManagerId.ValidateRequired(),
            EmployeeNumber.ValidateRequired(nameof(EmployeeNumber)),
            FirstName.ValidateRequired(nameof(FirstName)),
            LastName.ValidateRequired(nameof(LastName)),
            Email.ValidateRequired(nameof(Email)),
        }
        .Where(message => !string.IsNullOrWhiteSpace(message));

        return validationFailureMessages.Any()
            ? new ValidationResponse(false, validationFailureMessages)
            : new ValidationResponse(true, []);
    }

    /// <summary>
    /// Maps the <see cref="EmployeeModel"/> to <see cref="Employee"/>.
    /// </summary>
    /// <returns><see cref="Employee"/> entity.</returns>
    public Employee ToEntity()
    {
        return new Employee
        {
            EmployeeId = EmployeeId,
            JobPositionId = JobPositionId,
            ApplicationUserId = ApplicationUserId,
            EmployeeNumber = EmployeeNumber,
            ManagerId = ManagerId
        };
    }

    /// <summary>
    /// Maps the <see cref="EmployeeModel"/> to <see cref="ResourceIdeaResponse{EmployeeModel}"/>.
    /// </summary>
    /// <returns><see cref="ResourceIdeaResponse{EmployeeModel}"/></returns>
    public ResourceIdeaResponse<EmployeeModel> ToResourceIdeaResponse()
    {
        throw new NotImplementedException();
    }
}
