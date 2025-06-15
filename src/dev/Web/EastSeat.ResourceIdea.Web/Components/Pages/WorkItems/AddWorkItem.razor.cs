// ----------------------------------------------------------------------------------
// File: AddWorkItem.razor.cs
// Path: src\dev\Web\EastSeat.ResourceIdea.Web\Components\Pages\WorkItems\AddWorkItem.razor.cs
// Description: Code-behind for the AddWorkItem page.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Employees.Queries;
using EastSeat.ResourceIdea.Application.Features.Engagements.Queries;
using EastSeat.ResourceIdea.Application.Features.WorkItems.Commands;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Web.Extensions;
using EastSeat.ResourceIdea.Web.Services.Contracts;

using MediatR;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace EastSeat.ResourceIdea.Web.Components.Pages.WorkItems;

public partial class AddWorkItem : ComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;

    [Parameter]
    [SupplyParameterFromQuery]
    public Guid? ClientId { get; set; }

    [Parameter]
    [SupplyParameterFromQuery]
    public Guid? EngagementId { get; set; }

    public CreateWorkItemCommand Command { get; set; } = new();
    private PagedListResponse<EngagementModel>? Engagements { get; set; }
    private PagedListResponse<TenantEmployeeModel>? Employees { get; set; }
    private bool IsLoading { get; set; } = true;
    private bool HasError { get; set; } = false;
    private string ErrorMessage { get; set; } = string.Empty;
    private string SelectedEngagementId { get; set; } = string.Empty;
    private string SelectedAssignedToId { get; set; } = string.Empty;
    private string NavigationSource { get; set; } = string.Empty;

    private bool IsEngagementPreSelected => EngagementId.HasValue;
    private bool HasAnyEngagements => Engagements?.Items.Any() == true;
    private bool HasAnyEmployees => Employees?.Items.Any() == true;
    private bool CanSubmit => !string.IsNullOrWhiteSpace(Command.Title) && 
                             !string.IsNullOrWhiteSpace(SelectedEngagementId) && 
                             HasAnyEngagements;

    protected override async Task OnInitializedAsync()
    {
        // Parse query parameters to determine navigation source and context
        var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
        var queryParams = QueryHelpers.ParseQuery(uri.Query);
        
        if (queryParams.TryGetValue("from", out var fromValue))
        {
            NavigationSource = fromValue.ToString();
        }
        else
        {
            // Determine navigation source based on query parameters
            if (ClientId.HasValue && EngagementId.HasValue)
            {
                NavigationSource = "workitems";
            }
            else if (ClientId.HasValue)
            {
                NavigationSource = "client";
            }
            else if (EngagementId.HasValue)
            {
                NavigationSource = "engagement";
            }
            else
            {
                NavigationSource = "workitems";
            }
        }

        await LoadEngagements();
        await LoadEmployees();
        
        if (IsEngagementPreSelected)
        {
            SelectedEngagementId = EngagementId!.Value.ToString();
        }
        
        IsLoading = false;
        StateHasChanged();
    }

    private async Task LoadEngagements()
    {
        try
        {
            var query = new GetAllEngagementsQuery(1, 100); // Get a larger page size to include all engagements
            query.TenantId = ResourceIdeaRequestContext.Tenant;

            var response = await Mediator.Send(query);
            if (response.IsSuccess && response.Content.HasValue)
            {
                Engagements = response.Content.Value;
            }
            else
            {
                HasError = true;
                ErrorMessage = "Failed to load engagements. Please try again later.";
            }
        }
        catch (InvalidOperationException ex)
        {
            HasError = true;
            ErrorMessage = "An error occurred while loading engagements. Please try again later.";
            Console.Error.WriteLine($"InvalidOperationException in LoadEngagements: {ex}");
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = "An unexpected error occurred. Please try again later.";
            Console.Error.WriteLine($"Unexpected error in LoadEngagements: {ex}");
        }
    }

    private async Task LoadEmployees()
    {
        try
        {
            TenantEmployeesQuery query = new()
            {
                TenantId = ResourceIdeaRequestContext.Tenant,
                PageNumber = 1,
                PageSize = 100, // Get a larger page size to include all employees
            };

            var response = await Mediator.Send(query);
            if (response.IsSuccess && response.Content.HasValue)
            {
                Employees = response.Content.Value;
            }
            else
            {
                // Note: It's OK if employees loading fails, as assignment is optional
                Console.WriteLine("Failed to load employees, but continuing as assignment is optional.");
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.Error.WriteLine($"InvalidOperationException in LoadEmployees: {ex}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error in LoadEmployees: {ex}");
        }
    }

    private async Task HandleValidSubmit()
    {
        // Check if any engagements exist
        if (!HasAnyEngagements)
        {
            NotificationService.ShowErrorNotification("No engagements available. Please add an engagement first before creating a work item.");
            return;
        }

        // Set the engagement ID from the selected value
        if (Guid.TryParse(SelectedEngagementId, out var engagementGuid))
        {
            Command.EngagementId = EngagementId.Create(engagementGuid);
        }
        else
        {
            NotificationService.ShowErrorNotification("Please select a valid engagement.");
            return;
        }

        // Set the assigned employee ID if selected
        if (!string.IsNullOrWhiteSpace(SelectedAssignedToId) && Guid.TryParse(SelectedAssignedToId, out var assignedToGuid))
        {
            Command.AssignedToId = EmployeeId.Create(assignedToGuid);
        }
        else
        {
            Command.AssignedToId = null; // Optional assignment
        }

        Command.TenantId = ResourceIdeaRequestContext.Tenant;
        
        ValidationResponse commandValidation = Command.Validate();
        if (!commandValidation.IsValid && commandValidation.ValidationFailureMessages.Any())
        {
            // TODO: Log validation failure.
            NotificationService.ShowErrorNotification("Validation failed. Please check the form and try again.");
            return;
        }

        try
        {
            var response = await Mediator.Send(Command);
            if (response.IsSuccess)
            {
                NotificationService.ShowSuccessNotification("Work item created successfully!");
                NavigateBack();
            }
            else
            {
                NotificationService.ShowErrorNotification("Failed to create work item. Please try again.");
            }
        }
        catch (Exception ex)
        {
            NotificationService.ShowErrorNotification("An unexpected error occurred. Please try again.");
            Console.Error.WriteLine($"Error creating work item: {ex}");
        }
    }

    private void NavigateBack()
    {
        var backUrl = GetBackNavigationUrl();
        NavigationManager.NavigateTo(backUrl);
    }

    private string GetBackNavigationUrl()
    {
        return NavigationSource switch
        {
            "client" when ClientId.HasValue => $"/clients/{ClientId.Value}",
            "engagement" when EngagementId.HasValue => $"/engagements/{EngagementId.Value}",
            "workitems" when ClientId.HasValue && EngagementId.HasValue => $"/workitems?clientid={ClientId.Value}&engagementid={EngagementId.Value}",
            _ => "/workitems" // Default fallback
        };
    }

    private string GetBackButtonText()
    {
        return NavigationSource switch
        {
            "client" => "Back to client details",
            "engagement" => "Back to engagement details", 
            "workitems" => "Back to work items list",
            _ => "Back to work items list" // Default fallback
        };
    }
}