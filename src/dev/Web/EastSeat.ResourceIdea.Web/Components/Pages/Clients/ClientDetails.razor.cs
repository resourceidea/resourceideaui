// ==================================================================================================
// File: ClientDetails.razor.cs
// Path: src\dev\Web\EastSeat.ResourceIdea.Web\Components\Pages\Clients\ClientDetails.razor.cs
// Description: This file contains the code-behind for the ClientDetails component.
// ==================================================================================================

using EastSeat.ResourceIdea.Application.Features.Clients.Queries;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Web.RequestContext;
using EastSeat.ResourceIdea.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Clients;

public partial class ClientDetails : ComponentBase
{
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;
    [Inject] private IMediator Mediator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;

    [Parameter]
    public Guid Id { get; set; }
    
    private bool IsLoading { get; set; } = true;
    private bool HasError { get; set; }
    private string ErrorMessage { get; set; } = string.Empty;
    private ClientModel? Client { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadClientAsync();
    }

    protected override async Task OnParametersSetAsync()
    {
        if (Id != Guid.Empty)
        {
            await LoadClientAsync();
        }
    }

    private async Task LoadClientAsync()
    {
        IsLoading = true;
        HasError = false;
        StateHasChanged();

        var query = new GetClientByIdQuery
        {
            ClientId = ClientId.Create(Id),
            TenantId = ResourceIdeaRequestContext.Tenant
        };

        var response = await Mediator.Send(query);

        if (response is not null && response.IsSuccess && response.Content != null)
        {
            Client = response.Content;
        }
        else
        {
            HasError = true;
            ErrorMessage = "Failed to load client details. Please try again later.";
        }

        IsLoading = false;
        StateHasChanged();
    }

    private void HandleEditClick()
    {
        // This would navigate to the edit page when implemented
        NotificationService.ShowInfoNotification("Edit functionality will be implemented in a future update.");
    }
}