/* ------------------------------------------------------------------------------
 * File: DepartmentDetail.razor.cs
 * Path: src\dev\Web\EastSeat.ResourceIdea.Web\Components\Pages\Departments\DepartmentDetail.razor.cs
 * Description: Department detail page component backend class.
 * ------------------------------------------------------------------------------- */

using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Departments.Commands;
using EastSeat.ResourceIdea.Application.Features.Departments.Queries;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Queries;
using EastSeat.ResourceIdea.Domain.Departments.Models;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.Models;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Web.Components.Shared;
using EastSeat.ResourceIdea.Web.RequestContext;
using EastSeat.ResourceIdea.Web.Services;
using MediatR;

using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Departments;

public partial class DepartmentDetail : ComponentBase
{
    [Parameter] public Guid Id { get; set; }

    PagedListResponse<JobPositionSummary> JobPositionSummaries { get; set; } = new();

    public UpdateDepartmentCommand Model { get; set; } = new();

    private bool IsLoadingModelData;
    private string? errorMessage;
    private bool isErrorMessage;
    private ParentComponent ParentComponent { get; set; } = new();
    private NotificationMessage? notification;

    [Inject] private IMediator Mediator { get; set; } = null!;
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        IsLoadingModelData = true;

        ParentComponent.View = "department-details";
        ParentComponent.Id = Id.ToString();

        notification = NotificationService.GetMessage();

        await LoadDepartmentDetailsAsync();
        await LoadDepartmentJobPositionsAsync();

        IsLoadingModelData = false;
    }

    protected async Task HandleValidSubmitAsync()
    {
        ValidationResponse validationResponse = Model.Validate();
        if (!validationResponse.IsValid)
        {
            // TODO: Log validation failure.
            errorMessage = string.Join(
                Environment.NewLine,
                validationResponse.ValidationFailureMessages.FirstOrDefault());
            isErrorMessage = true;
            return;
        }
        ResourceIdeaResponse<DepartmentModel> response = await Mediator.Send(Model);
        if (response.IsSuccess && response.Content.HasValue)
        {
            DisplayMessage(message: "Changes saved successfully", isError: false);
        }
        else
        {
            DisplayMessage(message: "Failed to save changes", isError: true);
        }
    }

    protected async Task HandlePageChangeAsync(int page) => await LoadDepartmentJobPositionsAsync(page);

    private async Task LoadDepartmentJobPositionsAsync(int page = 1)
    {
        GetJobPositionsByDepartmentIdQuery query = new()
        {
            DepartmentId = DepartmentId.Create(Id),
            TenantId = ResourceIdeaRequestContext.Tenant,
            PageNumber = page,
            PageSize = 5
        };

        var validationResponse = query.Validate();
        if (!validationResponse.IsValid)
        {
            // TODO: Log validation failure.
            errorMessage = string.Join(
                Environment.NewLine,
                validationResponse.ValidationFailureMessages.FirstOrDefault());
            DisplayMessage(message: errorMessage, isError: true);
        }

        ResourceIdeaResponse<PagedListResponse<JobPositionSummary>> response = await Mediator.Send(query);
        if (response.IsSuccess && response.Content.HasValue)
        {
            JobPositionSummaries = response.Content.Value;
        }
        else
        {
            // TODO: Display and log message for failure.
        }
    }

    private async Task LoadDepartmentDetailsAsync()
    {
        GetDepartmentByIdQuery query = new()
        {
            DepartmentId = DepartmentId.Create(Id),
            TenantId = ResourceIdeaRequestContext.Tenant
        };

        var validationResponse = query.Validate();
        if (!validationResponse.IsValid)
        {
            // TODO: Log validation failure.
            errorMessage = string.Join(
                Environment.NewLine,
                validationResponse.ValidationFailureMessages.FirstOrDefault());
            DisplayMessage(message: errorMessage, isError: true);
        }

        ResourceIdeaResponse<DepartmentModel> response = await Mediator.Send(query);
        if (response.IsSuccess && response.Content.HasValue)
        {
            Model = new UpdateDepartmentCommand
            {
                Name = response.Content.Value.Name,
                DepartmentId = response.Content.Value.DepartmentId,
                TenantId = ResourceIdeaRequestContext.Tenant
            };
        }
        else
        {
            // TODO: Log failure to query for department.                
            DisplayMessage(message: "Failed to query for department.", isError: true);
        }
    }

    private void DisplayMessage(string message, bool isError)
    {
        errorMessage = message;
        isErrorMessage = isError;
    }
}
