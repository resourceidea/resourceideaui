using EastSeat.ResourceIdea.Application.Features.Employees.Commands;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Web.RequestContext;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Employees;

public partial class AddEmployee : ComponentBase
{
    private string? errorMessage;
    private bool isErrorMessage;
    [Inject] private IMediator Mediator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;

    [Parameter, SupplyParameterFromQuery] public Guid JobPosition { get; set; }
    [Parameter, SupplyParameterFromQuery] public string? ReturnView { get; set; }
    [Parameter, SupplyParameterFromQuery] public string? ReturnId { get; set; }

    public AddEmployeeCommand Command { get; set; } = new();

    private async Task HandleValidSubmit()
    {
        Command.JobPositionId = JobPositionId.Create(JobPosition);
        Command.TenantId = ResourceIdeaRequestContext.Tenant;
        ValidationResponse commandValidation = Command.Validate();
        if (!commandValidation.IsValid)
        {
            isErrorMessage = true;
            errorMessage = string.Join(Environment.NewLine, commandValidation.ValidationFailureMessages);
            return;
        }

        ResourceIdeaResponse<EmployeeModel> response = await Mediator.Send(Command);
        if (!response.IsSuccess || !response.Content.HasValue)
        {
            // TODO: Log failure to create job position.
            errorMessage = "Failed to create job position";
            isErrorMessage = true;
        }

        if (ReturnView == "department-details")
        {
            NavigationManager.NavigateTo($"/departments/{ReturnId}");
        }
        else
        {
            NavigationManager.NavigateTo("/employees");
        }
    }
}
