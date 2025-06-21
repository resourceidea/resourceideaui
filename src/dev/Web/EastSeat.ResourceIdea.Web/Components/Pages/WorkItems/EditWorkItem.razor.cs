// ----------------------------------------------------------------------------------
// File: EditWorkItem.razor.cs
// Path: src\dev\Web\EastSeat.ResourceIdea.Web\Components\Pages\WorkItems\EditWorkItem.razor.cs
// Description: Code-behind for the EditWorkItem page.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.WorkItems.Commands;
using EastSeat.ResourceIdea.Application.Features.WorkItems.Queries;
using EastSeat.ResourceIdea.Application.Features.Engagements.Queries;
using EastSeat.ResourceIdea.Application.Features.Clients.Queries;
using EastSeat.ResourceIdea.Domain.WorkItems.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Web.Services;
using EastSeat.ResourceIdea.Web.RequestContext;
using EastSeat.ResourceIdea.Web.Components.Base;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace EastSeat.ResourceIdea.Web.Components.Pages.WorkItems;

public partial class EditWorkItem : ResourceIdeaComponentBase
{
    [Parameter]
    public Guid Id { get; set; }

    [Inject] public IMediator Mediator { get; set; } = default!;
    [Inject] public NavigationManager Navigation { get; set; } = default!;
    [Inject] public IResourceIdeaRequestContext RequestContext { get; set; } = default!;
    [Inject] public NotificationService NotificationService { get; set; } = default!;

    private UpdateWorkItemCommand? Command;
    private string EngagementName = string.Empty;
    private string ClientName = string.Empty;
    private string ClientId = string.Empty;
    private string EngagementId = string.Empty;
    private DateTimeOffset? StartDate;
    private DateTimeOffset? CompletedDate;
    private bool IsSubmitting = false;
    private bool IsWorkItemCompleted = false;

    // Business rule properties
    private bool CannotEditStartDate => Command?.Status != WorkItemStatus.NotStarted;
    private bool CannotEditEndDate => Command?.Status != WorkItemStatus.NotStarted && 
                                      Command?.Status != WorkItemStatus.InProgress && 
                                      Command?.Status != WorkItemStatus.OnHold;

    protected override async Task OnInitializedAsync()
    {
        await ExecuteAsync(async () =>
        {
            // Load work item details
            GetWorkItemByIdQuery workItemQuery = new()
            {
                WorkItemId = WorkItemId.Create(Id),
                TenantId = RequestContext.Tenant
            };

            var workItemResponse = await Mediator.Send(workItemQuery);
            if (workItemResponse.IsFailure || !workItemResponse.Content.HasValue)
            {
                throw new InvalidOperationException("Failed to load work item details.");
            }

            var workItem = workItemResponse.Content.Value;

            // Check if work item is completed
            if (workItem.Status == WorkItemStatus.Completed)
            {
                IsWorkItemCompleted = true;
                return;
            }

            // Load engagement details to get engagement name and client id
            GetEngagementByIdQuery engagementQuery = new()
            {
                EngagementId = workItem.EngagementId
            };

            var engagementResponse = await Mediator.Send(engagementQuery);
            if (engagementResponse.IsFailure || !engagementResponse.Content.HasValue)
            {
                throw new InvalidOperationException("Failed to load engagement details.");
            }

            var engagement = engagementResponse.Content.Value;
            EngagementName = engagement.Description;
            EngagementId = engagement.Id.Value.ToString();

            // Load client details to get client name
            GetClientByIdQuery clientQuery = new()
            {
                ClientId = engagement.ClientId,
                TenantId = RequestContext.Tenant
            };

            var clientResponse = await Mediator.Send(clientQuery);
            if (clientResponse.IsFailure || !clientResponse.Content.HasValue)
            {
                throw new InvalidOperationException("Failed to load client details.");
            }

            var client = clientResponse.Content.Value;
            ClientName = client.Name;
            ClientId = client.ClientId.Value.ToString();

            // Initialize command
            Command = new UpdateWorkItemCommand
            {
                WorkItemId = workItem.Id,
                EngagementId = workItem.EngagementId,
                Title = workItem.Title,
                Description = workItem.Description,
                Status = workItem.Status,
                Priority = workItem.Priority,
                AssignedToId = workItem.AssignedToId
            };
            Command.TenantId = workItem.TenantId;

            StartDate = workItem.StartDate;
            CompletedDate = workItem.CompletedDate;
        }, "Loading work item details for editing");
    }

    private async Task HandleValidSubmit()
    {
        if (Command is null)
        {
            return;
        }

        IsSubmitting = true;
        StateHasChanged();

        var success = await ExecuteAsync(async () =>
        {
            // Update command with date values
            Command.StartDate = StartDate;
            Command.CompletedDate = CompletedDate;

            var result = await Mediator.Send(Command);
            if (result.IsSuccess)
            {
                NotificationService.ShowSuccessNotification("Work item details updated successfully.");
                // Navigate back to work item details
                Navigation.NavigateTo(GetBackNavigationUrl());
            }
            else
            {
                throw new InvalidOperationException("Failed to update work item details. Please try again.");
            }
        }, "Updating work item details", manageLoadingState: false);

        if (!success)
        {
            NotificationService.ShowErrorNotification(ErrorMessage ?? "Failed to update work item details. Please try again.");
        }

        IsSubmitting = false;
        StateHasChanged();
    }

    private string GetBackNavigationUrl()
    {
        // Parse query parameters to determine navigation context
        var uri = Navigation.ToAbsoluteUri(Navigation.Uri);
        var queryParams = QueryHelpers.ParseQuery(uri.Query);
        
        var backUrl = $"/workitems/{Id}";
        
        // Add context parameters if available
        var queryString = new List<string>();
        
        if (!string.IsNullOrEmpty(EngagementId))
        {
            queryString.Add($"engagementId={EngagementId}");
        }
        
        if (!string.IsNullOrEmpty(ClientId))
        {
            queryString.Add($"clientId={ClientId}");
        }
        
        if (queryString.Any())
        {
            backUrl += "?" + string.Join("&", queryString);
        }
        
        return backUrl;
    }
}