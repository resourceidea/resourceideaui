// =============================================================================================
// File: AddEmployee.razor.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Web\Components\Pages\Employees\AddEmployee.razor.cs
// Description: This file contains the code-behind for the AddEmployee component.
// =============================================================================================

using EastSeat.ResourceIdea.Application.Features.Employees.Commands;
using EastSeat.ResourceIdea.Domain.Departments.Models;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.Models;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Web.RequestContext;
using EastSeat.ResourceIdea.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Components;
using EastSeat.ResourceIdea.Application.Features.Departments.Queries;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Queries;
using EastSeat.ResourceIdea.Domain.Employees.Models;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Employees;

public partial class AddEmployee : ComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;

    [Parameter, SupplyParameterFromQuery] public Guid JobPosition { get; set; }
    [Parameter, SupplyParameterFromQuery] public string? ReturnView { get; set; }
    [Parameter, SupplyParameterFromQuery] public string? ReturnId { get; set; }

    public AddEmployeeCommand Command { get; set; } = new();
    public List<DepartmentModel> Departments { get; set; } = [];
    public List<TenantJobPositionModel> JobPositions { get; set; } = [];
    public Guid SelectedDepartmentId { get; set; }
    public Guid SelectedJobPositionId { get; set; }
    public bool IsLoadingData { get; set; } = true;
    public bool IsPrePopulated { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsLoadingData = true;

        if (JobPosition != Guid.Empty)
        {
            SelectedJobPositionId = JobPosition;
            IsPrePopulated = true;

            await LoadJobPositionDetailsAsync(JobPositionId.Create(JobPosition));
        }

        await LoadDepartmentsAsync();

        IsLoadingData = false;
    }

    private async Task LoadJobPositionDetailsAsync(JobPositionId jobPositionId)
    {
        try
        {
            var query = new GetJobPositionByIdQuery
            {
                JobPositionId = jobPositionId,
                TenantId = ResourceIdeaRequestContext.Tenant
            };

            var response = await Mediator.Send(query);

            if (response.IsSuccess && response.Content != null)
            {
                var jobPositionDetails = response.Content;
                SelectedDepartmentId = jobPositionDetails.DepartmentId.Value;
            }
        }
        catch (Exception)
        {
            NotificationService.ShowErrorNotification($"ERROR: Failed to load job position details.");
        }
    }
    private async Task LoadDepartmentsAsync()
    {
        var query = new GetAllDepartmentsQuery { TenantId = ResourceIdeaRequestContext.Tenant };
        var response = await Mediator.Send(query);

        if (response.IsSuccess && response.Content != null)
        {
            Departments = [.. response.Content.Items];
            if (Departments.Count > 0)
            {
                if (!IsPrePopulated)
                {
                    var firstDepartmentId = Departments.First().DepartmentId;
                    SelectedDepartmentId = firstDepartmentId.Value;
                }
                await LoadJobPositionsForDepartmentAsync(DepartmentId.Create(SelectedDepartmentId));
            }
        }
        else
        {
            NotificationService.ShowErrorNotification("Failed to load departments");
        }
    }
    private async Task LoadJobPositionsForDepartmentAsync(DepartmentId departmentId)
    {
        var query = new GetJobPositionsByDepartmentIdQuery
        {
            DepartmentId = departmentId,
            TenantId = ResourceIdeaRequestContext.Tenant,
            PageNumber = 1,
            PageSize = 10
        };

        var response = await Mediator.Send(query);
        if (response.IsSuccess && response.Content != null)
        {
            var jobPositionSummaries = response.Content.Items;
            JobPositions = [.. jobPositionSummaries.Select(jp => new TenantJobPositionModel
            {
                Id = jp.JobPositionId,
                Title = jp.Title ?? string.Empty,
                DepartmentId = departmentId
            })];

            if (IsPrePopulated && JobPosition != Guid.Empty)
            {
                SelectedJobPositionId = JobPosition;
                if (JobPositions.All(jp => jp.Id.Value != SelectedJobPositionId))
                {
                    NotificationService.ShowErrorNotification("The specified job position was not found in the selected department");
                }
            }
            else if (JobPositions.Count > 0)
            {
                SelectedJobPositionId = JobPositions.First().Id.Value;
            }
        }
        else
        {
            JobPositions = [];
            NotificationService.ShowErrorNotification("Failed to load job positions");
        }
    }

    private async Task OnDepartmentChanged(ChangeEventArgs e)
    {
        if (Guid.TryParse(e.Value?.ToString(), out var departmentGuid))
        {
            SelectedDepartmentId = departmentGuid;
            await LoadJobPositionsForDepartmentAsync(DepartmentId.Create(departmentGuid));
        }
    }

    private void OnJobPositionChanged(ChangeEventArgs e)
    {
        if (Guid.TryParse(e.Value?.ToString(), out var jobPositionGuid))
        {
            SelectedJobPositionId = jobPositionGuid;
        }
    }

    private async Task HandleValidSubmit()
    {
        Command.JobPositionId = JobPositionId.Create(SelectedJobPositionId);
        Command.TenantId = ResourceIdeaRequestContext.Tenant;
        ValidationResponse commandValidation = Command.Validate();
        if (!commandValidation.IsValid)
        {
            // TODO: Log command validation failure.
            NotificationService.ShowErrorNotification("ERROR: Invalid data provided.");
            return;
        }

        ResourceIdeaResponse<EmployeeModel> response = await Mediator.Send(Command);
        if (!response.IsSuccess || response.Content is null)
        {
            // TODO: Log failure to add new employee.
            NotificationService.ShowErrorNotification("ERROR: Failed to add new employee.");
            return;
        }

        string returnUrl = GetReturnUrl();
        NotificationService.ShowSuccessNotification("Employee added successfully.");
        NavigationManager.NavigateTo(returnUrl);
    }

    private string GetReturnUrl()
    {
        const string defaultUrl = "/employees";

        if (string.IsNullOrEmpty(ReturnView) || string.IsNullOrEmpty(ReturnId))
        {
            return defaultUrl;
        }

        var urlMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "department-details", $"/departments/{ReturnId}" },
            { "jobposition-details", $"/jobpositions/{ReturnId}" },
        };

        return urlMappings.TryGetValue(ReturnView, out var url) ? url : defaultUrl;
    }
}
