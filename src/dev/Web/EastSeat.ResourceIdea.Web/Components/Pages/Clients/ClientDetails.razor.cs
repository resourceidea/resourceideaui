// ==================================================================================================
// File: ClientDetails.razor.cs
// Path: src\dev\Web\EastSeat.ResourceIdea.Web\Components\Pages\Clients\ClientDetails.razor.cs
// Description: This file contains the code-behind for the ClientDetails component.
// ==================================================================================================

using EastSeat.ResourceIdea.Application.Features.Clients.Queries;
using EastSeat.ResourceIdea.Application.Features.Engagements.Queries;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
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
    
    // Engagement list properties
    private PagedListResponse<EngagementModel>? PagedEngagementsList { get; set; }
    private bool IsLoadingEngagements { get; set; } = true;
    private string EngagementSearchTerm { get; set; } = string.Empty;
    private string EngagementSortField { get; set; } = string.Empty;
    private string EngagementSortDirection { get; set; } = "asc";
    private int CurrentEngagementsPage { get; set; } = 1;
    private const int EngagementsPageSize = 10;

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

        if (response is not null && response.IsSuccess && response.Content.HasValue)
        {
            Client = response.Content.Value;
            // Load engagements after client is loaded
            await LoadEngagementsAsync();
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

    private async Task LoadEngagementsAsync()
    {
        if (Client == null) return;

        IsLoadingEngagements = true;
        StateHasChanged();

        var query = new GetEngagementsByClientQuery(CurrentEngagementsPage, EngagementsPageSize)
        {
            ClientId = Client.ClientId,
            SearchTerm = string.IsNullOrWhiteSpace(EngagementSearchTerm) ? null : EngagementSearchTerm,
            SortField = string.IsNullOrWhiteSpace(EngagementSortField) ? null : EngagementSortField,
            SortDirection = string.IsNullOrWhiteSpace(EngagementSortDirection) ? null : EngagementSortDirection
        };

        var response = await Mediator.Send(query);

        if (response is not null && response.IsSuccess && response.Content.HasValue)
        {
            PagedEngagementsList = response.Content.Value;
        }
        else
        {
            PagedEngagementsList = null;
        }

        IsLoadingEngagements = false;
        StateHasChanged();
    }

    private async Task OnEngagementsPageChange(int page)
    {
        CurrentEngagementsPage = page;
        await LoadEngagementsAsync();
    }

    private async Task OnEngagementSearchTermChange(string searchTerm)
    {
        EngagementSearchTerm = searchTerm;
        CurrentEngagementsPage = 1; // Reset to first page when searching
        await LoadEngagementsAsync();
    }

    private async Task OnEngagementSortChange((string field, string direction) sortInfo)
    {
        EngagementSortField = sortInfo.field;
        EngagementSortDirection = sortInfo.direction;
        CurrentEngagementsPage = 1; // Reset to first page when sorting
        await LoadEngagementsAsync();
    }
}