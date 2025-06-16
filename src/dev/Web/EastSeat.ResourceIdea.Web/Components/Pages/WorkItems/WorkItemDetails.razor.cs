using EastSeat.ResourceIdea.Application.Features.WorkItems.Queries;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.WorkItems.Models;
using EastSeat.ResourceIdea.Domain.WorkItems.ValueObjects;
using EastSeat.ResourceIdea.Web.RequestContext;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace EastSeat.ResourceIdea.Web.Components.Pages.WorkItems;

public partial class WorkItemDetails : ComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = default!;

    [Parameter] public Guid Id { get; set; }

    private WorkItemModel? WorkItem { get; set; }
    private bool IsLoading { get; set; } = true;
    private bool HasError { get; set; }
    private string ErrorMessage { get; set; } = string.Empty;
    private string NavigationSource { get; set; } = string.Empty;
    private string? EngagementId { get; set; }
    private string? ClientId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        // Parse query parameters to determine navigation source
        var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
        var queryParams = QueryHelpers.ParseQuery(uri.Query);
        
        if (queryParams.TryGetValue("from", out var fromValue))
        {
            NavigationSource = fromValue.ToString();
        }
        
        if (queryParams.TryGetValue("engagementId", out var engagementIdValue))
        {
            EngagementId = engagementIdValue.ToString();
        }

        if (queryParams.TryGetValue("clientId", out var clientIdValue))
        {
            ClientId = clientIdValue.ToString();
        }

        await LoadWorkItemAsync();
    }

    protected override async Task OnParametersSetAsync()
    {
        if (Id != Guid.Empty)
        {
            await LoadWorkItemAsync();
        }
    }

    private async Task LoadWorkItemAsync()
    {
        IsLoading = true;
        HasError = false;
        ErrorMessage = string.Empty;

        try
        {
            var query = new GetWorkItemByIdQuery
            {
                WorkItemId = WorkItemId.Create(Id)
            };

            var response = await Mediator.Send(query);

            if (response.IsSuccess && response.Content.HasValue)
            {
                WorkItem = response.Content.Value;
            }
            else
            {
                HasError = true;
                ErrorMessage = GetErrorMessage(response.Error);
            }
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = "An unexpected error occurred. Please try again later.";
            Console.Error.WriteLine($"Unexpected error: {ex}"); // Replace with proper logging
            throw;
        }
        finally
        {
            IsLoading = false;
            StateHasChanged();
        }
    }

    private string GetStatusBadgeClass(WorkItemStatus status)
    {
        return status switch
        {
            WorkItemStatus.NotStarted => "bg-secondary",
            WorkItemStatus.InProgress => "bg-primary",
            WorkItemStatus.OnHold => "bg-warning",
            WorkItemStatus.Completed => "bg-success",
            WorkItemStatus.Canceled => "bg-danger",
            _ => "bg-secondary"
        };
    }

    private string GetPriorityBadgeClass(int priority)
    {
        return priority switch
        {
            1 => "bg-danger",      // Highest priority - red
            2 => "bg-warning",     // High priority - yellow
            3 => "bg-primary",     // Normal priority - blue
            4 => "bg-info",        // Low priority - light blue
            5 => "bg-secondary",   // Lowest priority - gray
            _ => "bg-secondary"
        };
    }

    private string GetPriorityText(int priority)
    {
        return priority switch
        {
            1 => "Critical",
            2 => "High",
            3 => "Medium",
            4 => "Low",
            5 => "Lowest",
            _ => "Medium"
        };
    }

    private string GetBackNavigationUrl()
    {
        return NavigationSource switch
        {
            "engagement" when !string.IsNullOrEmpty(EngagementId) => $"/engagements/{EngagementId}",
            "client" when !string.IsNullOrEmpty(ClientId) => $"/clients/{ClientId}",
            "workitems" => "/workitems",
            _ => "/workitems" // Default fallback
        };
    }

    private string GetBackButtonText()
    {
        return NavigationSource switch
        {
            "engagement" => "Back to engagement details",
            "client" => "Back to client details",
            "workitems" => "Back to work items list",
            _ => "Back to work items list" // Default fallback
        };
    }

    private static string GetErrorMessage(ErrorCode errorCode)
    {
        return errorCode switch
        {
            ErrorCode.NotFound => "Work item not found.",
            ErrorCode.DataStoreQueryFailure => "Failed to query work item details from the data store.",
            ErrorCode.BadRequest => "Invalid request to load work item details.",
            _ => "Failed to load work item details."
        };
    }
}