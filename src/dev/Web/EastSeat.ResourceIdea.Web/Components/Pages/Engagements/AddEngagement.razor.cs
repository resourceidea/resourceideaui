// ==================================================================================================
// File: AddEngagement.razor.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Web\Components\Pages\Engagements\AddEngagement.razor.cs
// Description: This file contains the code-behind for the AddEngagement component.
// ==================================================================================================

using EastSeat.ResourceIdea.Application.Features.Engagements.Commands;
using EastSeat.ResourceIdea.Application.Features.Clients.Queries;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Web.RequestContext;
using EastSeat.ResourceIdea.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Engagements;

public partial class AddEngagement : ComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;

    [Parameter]
    [SupplyParameterFromQuery]
    public Guid? ClientId { get; set; }

    public CreateEngagementCommand Command { get; set; } = new();
    private PagedListResponse<TenantClientModel>? Clients { get; set; }
    private bool IsLoading { get; set; } = true;
    private bool HasError { get; set; } = false;
    private string ErrorMessage { get; set; } = string.Empty;
    private string SelectedClientId { get; set; } = string.Empty;
    private bool IsClientPreSelected => ClientId.HasValue && ClientId.Value != Guid.Empty;
    private bool HasAnyClients => Clients != null && Clients.Items.Count > 0;
    private bool CanSubmit => HasAnyClients
        && !string.IsNullOrEmpty(SelectedClientId)
        && !string.IsNullOrWhiteSpace(Command.Title)
        && !string.IsNullOrWhiteSpace(Command.Description);

    protected override async Task OnInitializedAsync()
    {
        await LoadClients();

        if (IsClientPreSelected)
        {
            SelectedClientId = ClientId!.Value.ToString();
        }

        IsLoading = false;
        StateHasChanged();
    }

    private async Task LoadClients()
    {
        try
        {
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
                HasError = true;
                ErrorMessage = "Failed to load clients. Please try again later.";
            }
        }
        catch (InvalidOperationException ex)
        {
            HasError = true;
            ErrorMessage = "An invalid operation occurred while loading clients. Please contact support.";
            Console.Error.WriteLine($"InvalidOperationException: {ex.Message}");
        }
        catch (TaskCanceledException ex)
        {
            HasError = true;
            ErrorMessage = "The operation to load clients was canceled. Please try again.";
            Console.Error.WriteLine($"TaskCanceledException: {ex.Message}");
        }
        catch (HttpRequestException ex)
        {
            HasError = true;
            ErrorMessage = "A network error occurred while loading clients. Please check your connection and try again.";
            Console.Error.WriteLine($"HttpRequestException: {ex.Message}");
        }
        catch (TimeoutException ex)
        {
            HasError = true;
            ErrorMessage = "The request to load clients timed out. Please try again later.";
            Console.Error.WriteLine($"TimeoutException: {ex.Message}");
        }
    }

    private async Task HandleValidSubmit()
    {
        // Check if any clients exist
        if (!HasAnyClients)
        {
            NotificationService.ShowErrorNotification("No clients available. Please add a client first before creating an engagement.");
            return;
        }

        // Set the client ID from the selected value
        if (Guid.TryParse(SelectedClientId, out var clientGuid))
        {
            Command.ClientId = EastSeat.ResourceIdea.Domain.Clients.ValueObjects.ClientId.Create(clientGuid);
        }
        else
        {
            NotificationService.ShowErrorNotification("Please select a valid client.");
            return;
        }

        Command.TenantId = ResourceIdeaRequestContext.Tenant;

        ValidationResponse commandValidation = Command.Validate();
        if (!commandValidation.IsValid && commandValidation.ValidationFailureMessages.Any())
        {
            // TODO: Log validation failure.
            NotificationService.ShowErrorNotification("Validation failed. Please check the form and try again.");
            return;
        }

        var response = await Mediator.Send(Command);
        if (response.IsSuccess)
        {
            NotificationService.ShowSuccessNotification("Engagement added successfully.");

            // Navigate based on whether client ID was provided in the query parameter
            if (IsClientPreSelected)
            {
                NavigationManager.NavigateTo($"/clients/{ClientId!.Value}");
            }
            else
            {
                NavigationManager.NavigateTo("/engagements");
            }
        }
        else
        {
            NotificationService.ShowErrorNotification("Failed to add engagement. Please try again.");
        }
    }
}