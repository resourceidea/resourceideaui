using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Departments.Entities;
using EastSeat.ResourceIdea.Domain.Departments.Models;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Extensions;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Departments.Commands;

/// <summary>
/// Command to create a department.
/// </summary>
public sealed class CreateDepartmentCommand : BaseRequest<DepartmentModel>
{
    /// <summary>Name of the department to be created.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Converts the command to <see cref="Department"/> entity.
    /// </summary>
    /// <returns><see cref="Department"/> entity.</returns>
    public Department ToEntity()
    {
        return new Department
        {
            Name = Name,
            Id = DepartmentId.Create(Guid.NewGuid())
        };
    }

    /// <summary>
    /// Validates the command.
    /// </summary>
    /// <returns><see cref="ValidationResponse"/></returns>
    public override ValidationResponse Validate()
    {
        var validationFailureMessages = new[]
        {
            Name.ValidateRequired(nameof(Name)),
        }
        .Where(message => !string.IsNullOrWhiteSpace(message));

        return validationFailureMessages.Any()
            ? new ValidationResponse(false, validationFailureMessages)
            : new ValidationResponse(true, []);
    }
}
