using MediatR;
using EastSeat.ResourceIdea.Domain.Departments.Entities;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.Departments.Models;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Extensions;

namespace EastSeat.ResourceIdea.Application.Features.Departments.Commands;

public class UpdateDepartmentCommand : BaseRequest<DepartmentModel>
{
    /// <summary>
    /// Department ID.
    /// </summary>
    public DepartmentId DepartmentId { get; set; }

    /// <summary>
    /// Name of the department.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Map the command to a <see cref="Department"/> entity.
    /// </summary>
    /// <returns><see cref="Department"/> entity.</returns>
    public Department ToEntity()
    {
        return new Department
        {
            Id = DepartmentId,
            Name = Name
        };
    }

    /// <summary>
    /// Validates the command.
    /// </summary>
    /// <returns><see cref="ValidationResponse"/></returns>
    public ValidationResponse Validate()
    {
        var validationFailureMessages = new[]
        {
            Name.ValidateRequired(nameof(Name)),
            DepartmentId.ValidateRequired(),
            TenantId.ValidateRequired(),
        }
        .Where(message => !string.IsNullOrWhiteSpace(message));

        return validationFailureMessages.Any()
            ? new ValidationResponse(false, validationFailureMessages)
            : new ValidationResponse(true, []);
    }
}