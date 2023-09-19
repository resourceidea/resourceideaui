using EastSeat.ResourceIdea.Application.Responses;

namespace EastSeat.ResourceIdea.Application.Features.Employee.Commands.CreateEmployee;

/// <summary>
/// Response to the command to create an employee.
/// </summary>
public class CreateEmployeeCommandResponse : BaseResponse
{
    /// <summary>
    /// Instantiates <see cref="CreateEmployeeCommandResponse"/>.
    /// </summary>
    public CreateEmployeeCommandResponse() : base()
    {
    }

    /// <summary>
    /// Instantiates <see cref="CreateEmployeeCommandResponse"/>.
    /// </summary>
    /// <param name="message">Response message.</param>
    public CreateEmployeeCommandResponse(string message) : base(message)
    {
    }

    /// <summary>
    /// Instantiates <see cref="CreateEmployeeCommandResponse"/>.
    /// </summary>
    /// <param name="success">Flag indicating whether execution of the command was a success or not.</param>
    /// <param name="message">Response message.</param>
    public CreateEmployeeCommandResponse(bool success, string message) : base(success, message)
    {
    }

    /// <summary>New application user that has been created.</summary>
    public CreateEmployeeViewModel Employee { get; set; } = default!;
}
