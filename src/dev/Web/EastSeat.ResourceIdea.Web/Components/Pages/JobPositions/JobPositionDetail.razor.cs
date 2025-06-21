// ------------------------------------------------------------------------------
// File: JobPositionDetail.razor.cs
// Path: src/dev/Web/EastSeat.ResourceIdea.Web/Components/Pages/JobPositions/JobPositionDetail.razor.cs
// Description: JobPositionDetail component backend file.
// ------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.JobPositions.Commands;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Queries;
using EastSeat.ResourceIdea.Domain.JobPositions.Models;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Web.RequestContext;
using EastSeat.ResourceIdea.Web.Components.Base;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Pages.JobPositions;

public partial class JobPositionDetail : ResourceIdeaComponentBase
{
    [Parameter]
    public Guid Id { get; set; }

    [Inject] private IMediator Mediator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;

    private string? message;
    private bool isErrorMessage;
    private bool IsLoadingModelData = true;

    public JobPositionModel Model { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadJobPositionData();
    }

    private async Task LoadJobPositionData()
    {
        IsLoadingModelData = true;
        await ExecuteAsync(async () =>
        {
            var jobPositionId = JobPositionId.Create(Id);
            var query = new GetJobPositionByIdQuery
            {
                JobPositionId = jobPositionId,
                TenantId = ResourceIdeaRequestContext.Tenant
            };

            var response = await Mediator.Send(query);

            if (!response.IsSuccess || !response.Content.HasValue)
            {
                message = "Failed to load job position details";
                isErrorMessage = true;
                return;
            }

            Model = response.Content.Value;
        }, "Loading job position data", manageLoadingState: false);

        IsLoadingModelData = false;
    }

    private async Task HandleValidSubmit()
    {
        await ExecuteAsync(async () =>
        {
            var command = new UpdateJobPositionCommand
            {
                // Using the correct Create method for Guid
                Id = JobPositionId.Create(Id),
                Title = Model.Title,
                Description = Model.Description,
                DepartmentId = Model.DepartmentId,
                TenantId = ResourceIdeaRequestContext.Tenant
            };

            var validationResponse = command.Validate();
            if (!validationResponse.IsValid)
            {
                message = validationResponse.ValidationFailureMessages.Any()
                    ? string.Join(Environment.NewLine, validationResponse.ValidationFailureMessages.FirstOrDefault())
                    : "Validation failure.";
                isErrorMessage = true;
                return;
            }

            var response = await Mediator.Send(command);
            if (response.IsSuccess && response.Content.HasValue)
            {
                // Update successful, reload data
                await LoadJobPositionData();
                message = "Job position updated successfully";
                isErrorMessage = false;
            }
            else
            {
                message = "Failed to update job position";
                isErrorMessage = true;
            }
        }, "Updating job position", manageLoadingState: false);
    }
}