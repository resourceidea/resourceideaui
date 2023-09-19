namespace EastSeat.ResourceIdea.Application.Features.Employee.Commands.CreateEmployee;

/// <summary>
/// Hold data for the employee to be created.
/// </summary>
public class CreateEmployeeViewModel
{
    /// <summary>User Id in the application.</summary>
    public string UserId { get; set; } = Guid.Empty.ToString();

    /// <summary>Id of the employee's job position.</summary>
    public Guid JobPositionId { get; set; } = Guid.Empty;

    /// <summary>Id of the employee's subscription.</summary>
    public Guid SubscriptionId { get; set; } = Guid.Empty;
}
