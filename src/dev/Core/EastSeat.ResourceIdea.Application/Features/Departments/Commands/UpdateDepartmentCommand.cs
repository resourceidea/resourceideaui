using MediatR;
using EastSeat.ResourceIdea.Domain.Departments.Entities;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.Departments.Models;

namespace EastSeat.ResourceIdea.Application.Features.Departments.Commands;

public class UpdateDepartmentCommand : IRequest<ResourceIdeaResponse<DepartmentModel>>
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
}