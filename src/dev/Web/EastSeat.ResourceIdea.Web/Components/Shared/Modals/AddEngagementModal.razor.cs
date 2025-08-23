// ==================================================================================================
// File: AddEngagementModal.razor.cs
// Path: src\dev\Web\EastSeat.ResourceIdea.Web\Components\Shared\Modals\AddEngagementModal.razor.cs
// Description: Code-behind for the AddEngagementModal component.
// ==================================================================================================

using EastSeat.ResourceIdea.Application.Features.Engagements.Commands;
using EastSeat.ResourceIdea.Application.Features.Clients.Queries;
using EastSeat.ResourceIdea.Application.Features.WorkItems.Commands;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Web.RequestContext;
using EastSeat.ResourceIdea.Web.Services;
using EastSeat.ResourceIdea.Web.Components.Base;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Linq;
using System.Threading.Tasks;

namespace EastSeat.ResourceIdea.Web.Components.Shared.Modals;

public partial class AddEngagementModal : ResourceIdeaComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = null!;
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;

    [Parameter] public bool IsVisible { get; set; } = false;
    [Parameter] public EventCallback<bool> IsVisibleChanged { get; set; }
    [Parameter] public EventCallback OnEngagementCreated { get; set; }
    [Parameter] public Guid? PreSelectedClientId { get; set; }
    [Parameter] public string? PreSelectedClientName { get; set; }

    public CreateEngagementCommand Command { get; set; } = new();
    private PagedListResponse<TenantClientModel>? Clients { get; set; }
    private string SelectedClientId { get; set; } = string.Empty;
    private List<string> WorkItems { get; set; } = new();
    private string NewWorkItemDescription { get; set; } = string.Empty;

    private bool IsClientPreSelected => PreSelectedClientId.HasValue && PreSelectedClientId.Value != Guid.Empty;
    private bool HasAnyClients => Clients != null && Clients.Items.Count > 0;
    private bool CanSubmit => (IsClientPreSelected || (!string.IsNullOrEmpty(SelectedClientId) && HasAnyClients))
        && !string.IsNullOrWhiteSpace(Command.Title)
        && !string.IsNullOrWhiteSpace(Command.Description);

    protected override async Task OnInitializedAsync()
    {
        await ExecuteAsync(async (cancellationToken) =>
        {
            await LoadClients();
            
            if (IsClientPreSelected)
            {
                SelectedClientId = PreSelectedClientId!.Value.ToString();
            }
        }, "Loading modal data");
    }

    protected override Task OnParametersSetAsync()
    {
        if (IsVisible && IsClientPreSelected && PreSelectedClientId.HasValue)
        {
            SelectedClientId = PreSelectedClientId.Value.ToString();
        }
        return Task.CompletedTask;
    }

    private async Task LoadClients()
    {
        if (IsClientPreSelected)
        {
            return; // No need to load clients if one is pre-selected
        }

        TenantClientsQuery query = new()
        {
            TenantId = ResourceIdeaRequestContext.Tenant,
            PageNumber = 1,
            PageSize = 100, // Get a larger page size to include all clients
        };

        var response = await Mediator.Send(query);
        if (response.IsSuccess && response.Content.HasValue)
        {
            Clients = response.Content.Value;
        }
        else
        {
            throw new InvalidOperationException("Failed to load clients. Please try again later.");
        }
    }

    private async Task HandleValidSubmit()
    {
        await ExecuteAsync(async (cancellationToken) =>
        {
            // Validate client selection
            if (!IsClientPreSelected && (!HasAnyClients || string.IsNullOrEmpty(SelectedClientId)))
            {
                NotificationService.ShowErrorNotification("Please select a valid client.");
                return;
            }

            // Set the client ID from the selected value or pre-selected value
            var clientGuid = IsClientPreSelected ? PreSelectedClientId!.Value : Guid.Parse(SelectedClientId);
            Command.ClientId = EastSeat.ResourceIdea.Domain.Clients.ValueObjects.ClientId.Create(clientGuid);
            Command.TenantId = ResourceIdeaRequestContext.Tenant;

            // Validate command
            ValidationResponse commandValidation = Command.Validate();
            if (!commandValidation.IsValid && commandValidation.ValidationFailureMessages.Any())
            {
                NotificationService.ShowErrorNotification("Validation failed. Please check the form and try again.");
                return;
            }

            // Create engagement
            var engagementResponse = await Mediator.Send(Command, cancellationToken);
            if (!engagementResponse.IsSuccess)
            {
                NotificationService.ShowErrorNotification("Failed to add engagement. Please try again.");
                return;
            }

            // Create work items if any
            if (WorkItems.Any() && engagementResponse.Content.HasValue)
            {
                var engagement = engagementResponse.Content.Value;
                var engagementId = EngagementId.Create(engagement.Id.Value);

                var workItemCommands = WorkItems.Select(workItemDescription => new CreateWorkItemCommand
                {
                    Title = workItemDescription,
                    Description = workItemDescription,
                    EngagementId = engagementId,
                    TenantId = ResourceIdeaRequestContext.Tenant,
                    Priority = Priority.Medium
                });

                // Create work items sequentially to avoid DbContext concurrency issues
                foreach (var workItemCommand in workItemCommands)
                {
                    await Mediator.Send(workItemCommand, cancellationToken);
                }
            }

            NotificationService.ShowSuccessNotification("Engagement added successfully.");
            
            // Notify parent component and close modal
            await OnEngagementCreated.InvokeAsync();
            await CloseModal();
        }, "Creating engagement");
    }

    private async Task CloseModal()
    {
        IsVisible = false;
        await IsVisibleChanged.InvokeAsync(IsVisible);
        ClearForm();
    }

    private void ClearForm()
    {
        Command = new CreateEngagementCommand();
        WorkItems.Clear();
        NewWorkItemDescription = string.Empty;
        
        // Don't clear client selection if pre-selected
        if (!IsClientPreSelected)
        {
            SelectedClientId = string.Empty;
        }
        
        StateHasChanged();
    }

    private void AddWorkItem()
    {
        if (!string.IsNullOrWhiteSpace(NewWorkItemDescription))
        {
            WorkItems.Add(NewWorkItemDescription.Trim());
            NewWorkItemDescription = string.Empty;
            StateHasChanged();
        }
    }

    private void RemoveWorkItem(int index)
    {
        if (index >= 0 && index < WorkItems.Count)
        {
            WorkItems.RemoveAt(index);
            StateHasChanged();
        }
    }

    private void OnWorkItemKeyPress(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            AddWorkItem();
        }
    }
}