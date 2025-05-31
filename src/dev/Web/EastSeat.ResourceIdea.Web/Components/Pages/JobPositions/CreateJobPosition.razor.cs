// ----------------------------------------------------------------------
// File: CreateJobPosition.razor.cs
// Path: src\dev\Web\EastSeat.ResourceIdea.Web\Components\Pages\JobPositions\CreateJobPosition.razor.cs
// Description: CreateJobPosition component backend file.
// ----------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.JobPositions.Commands;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.Models;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Web.RequestContext;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Pages.JobPositions;

public partial class CreateJobPosition : ComponentBase
{
    [Parameter, SupplyParameterFromQuery]
    public Guid Department { get; set; }

    [Inject] private IMediator Mediator { get; set; } = null!;    
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;
    private string? errorMessage;
    private bool isErrorMessage;
    
    public CreateJobPositionCommand Command { get; set; } = new();

    private async Task HandleValidSubmit()
    {
        Command.DepartmentId = DepartmentId.Create(Department);
        Command.TenantId = ResourceIdeaRequestContext.Tenant;
        ValidationResponse commandValidationResponse = Command.Validate();
        if (!commandValidationResponse.IsValid)
        {
            errorMessage = string.Join(
                Environment.NewLine,
                commandValidationResponse.ValidationFailureMessages.FirstOrDefault());
            isErrorMessage = true;
            return;
        }

        ResourceIdeaResponse<JobPositionModel> response = await Mediator.Send(Command);
        if (!response.IsSuccess || !response.Content != null)
        {
            // TODO: Log failure to create job position.
            errorMessage = "Failed to create job position";
            isErrorMessage = true;
        }

        NavigationManager.NavigateTo($"/departments/{Department}");
    }
}