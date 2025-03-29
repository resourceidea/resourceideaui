// ===============================================================================================
//  File: HireEmployeeCommand.cs
//  Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Employees\Commands\HireEmployeeCommand.cs
//  Description: Command to hire an employee. This command is used to create a new employee record in the system.
// ================================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Extensions;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Commands;

/// <summary>
/// Command to hire an employee.
/// </summary>
public class AddEmployeeCommand : BaseRequest<EmployeeModel>
{
    public JobPositionId JobPositionId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string EmployeeNumber => GenerateEmployeeNumber();

    /// <summary>
    /// Maps the command to an Employee entity.
    /// </summary>
    /// <returns>A new Employee entity populated with data from this command.</returns>
    public Employee ToEntity() => new()
    {
        EmployeeId = EmployeeId.NewId(),
        JobPositionId = JobPositionId,
        TenantId = TenantId,
        EmployeeNumber = EmployeeNumber,
        ManagerId = EmployeeId.Empty,
        ApplicationUserId = ApplicationUserId.Create(Guid.NewGuid()),
        FirstName = FirstName,
        LastName = LastName,
        Email = Email,
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
            JobPositionId.ValidateRequired(),
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
    /// Generates a unique employee number.
    /// </summary>
    /// <returns>A unique employee number string.</returns>
    private static string GenerateEmployeeNumber() => $"EMP-{Guid.NewGuid().ToString()[..8].ToUpper()}";
}
