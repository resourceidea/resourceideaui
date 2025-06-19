using EastSeat.ResourceIdea.Application.Features.Departments.Commands;
using EastSeat.ResourceIdea.Web.RequestContext;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Departments;

public partial class CreateDepartment : ComponentBase
{
    public CreateDepartmentCommand Command { get; set; } = new();
    private string? errorMessage;
    [Inject] private IMediator Mediator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;

    private async Task HandleValidSubmit()
    {
        Command.TenantId = ResourceIdeaRequestContext.Tenant;
        var validationResponse = Command.Validate();
        if (!validationResponse.IsValid)
        {
            errorMessage = string.Join(", ", validationResponse.ValidationFailureMessages);
            return;
        }

        var result = await Mediator.Send(Command);
        if (result != null && result.IsSuccess)
        {
            NavigationManager.NavigateTo("/departments");
        }
        else
        {
            errorMessage = "Failed to create department. Please try again.";
        }
    }
}