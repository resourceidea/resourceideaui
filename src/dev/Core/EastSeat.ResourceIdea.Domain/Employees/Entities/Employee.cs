/// -------------------------------------------------------------------------------
/// File: Employee.cs
/// Path: src\dev\Core\EastSeat.ResourceIdea.Domain\Employees\Entities\Employee.cs
/// Description: Employee entity.
/// -------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.Entities;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.Employees.Entities;

/// <summary>
/// Employee entity.
/// </summary>
public class Employee : BaseEntity
{
    public EmployeeId EmployeeId { get; set; }

    public JobPositionId JobPositionId { get; set; }

    public ApplicationUserId ApplicationUserId { get; set; }

    public string? EmployeeNumber { get; set; }

    public EmployeeId ManagerId { get; set; }

    public JobPosition? JobPosition { get; set; }

    public Employee? Manager { get; set; }

    public IEnumerable<Employee>? Subordinates { get; set; }

    /// <inheritdoc/>
    public override TModel ToModel<TModel>()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override ResourceIdeaResponse<TModel> ToResourceIdeaResponse<TEntity, TModel>()
    {
        throw new NotImplementedException();
    }
}
