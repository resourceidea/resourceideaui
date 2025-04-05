using EastSeat.ResourceIdea.Application.Features.Employees.Commands;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Web.RequestContext;
using EastSeat.ResourceIdea.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Components;

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

    private async Task HandleValidSubmit()
    {
        Command.JobPositionId = JobPositionId.Create(JobPosition);
        Command.TenantId = ResourceIdeaRequestContext.Tenant;
        ValidationResponse commandValidation = Command.Validate();
        if (!commandValidation.IsValid)
        {
            // TODO: Log command validation failure.
            NotificationService.ShowErrorNotification("ERROR: Invalid data provided.");
            return;
        }

        ResourceIdeaResponse<EmployeeModel> response = await Mediator.Send(Command);
        if (!response.IsSuccess || !response.Content.HasValue)
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
