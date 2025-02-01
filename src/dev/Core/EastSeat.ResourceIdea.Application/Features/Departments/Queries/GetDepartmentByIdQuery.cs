using EastSeat.ResourceIdea.Domain.Departments.Models;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Departments.Queries;

/// <summary>
/// Query to get a department given its ID.
/// </summary>
public sealed class GetDepartmentByIdQuery : IRequest<ResourceIdeaResponse<DepartmentModel>>
{
    /// <summary>
    /// Department ID.
    /// </summary>
    public DepartmentId DepartmentId { get; set; }
}
