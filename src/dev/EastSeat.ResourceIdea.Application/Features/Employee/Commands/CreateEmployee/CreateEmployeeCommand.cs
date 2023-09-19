using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Employee.Commands.CreateEmployee;

/// <summary>
/// Command to create an employee record.
/// </summary>
public class CreateEmployeeCommand : IRequest<CreateEmployeeCommandResponse>
{
    /// <summary>UserId of the subscription to which the employee belongs.</summary>
    public Guid SubscriptionId { get; set; } = Guid.Empty;

    /// <summary>Employee's user Id in the application</summary>
    public string UserId { get; set; } = Guid.Empty.ToString();

    /// <summary>Employee's job position id.</summary>
    public Guid JobPositionId { get; set; } = Guid.Empty;
}
