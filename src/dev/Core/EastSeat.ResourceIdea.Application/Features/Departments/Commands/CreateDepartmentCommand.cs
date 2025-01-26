using EastSeat.ResourceIdea.Domain.Departments.Entities;
using EastSeat.ResourceIdea.Domain.Departments.Models;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Departments.Commands;

/// <summary>
/// Command to create a department.
/// </summary>
public sealed class CreateDepartmentCommand : IRequest<ResourceIdeaResponse<DepartmentModel>>
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
}
