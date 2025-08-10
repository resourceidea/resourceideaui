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
using EastSeat.ResourceIdea.Web.Components.Base;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Clients;

public partial class ClientDetails : ResourceIdeaComponentBase
{
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;
    [Inject] private IMediator Mediator { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;

    [Parameter]
    public Guid Id { get; set; }

    private ClientModel? Client { get; set; }

    // Engagement list properties
    private PagedListResponse<EngagementModel>? PagedEngagementsList { get; set; }
    private bool IsLoadingEngagements { get; set; } = false;
    private string EngagementSearchTerm { get; set; } = string.Empty;
    private string EngagementSortField { get; set; } = string.Empty;
    private string EngagementSortDirection { get; set; } = "asc";
    private int CurrentEngagementsPage { get; set; } = 1;
    private const int EngagementsPageSize = 10;

    protected override async Task OnInitializedAsync()
    {
        await ExecuteAsync(LoadClientAsync, "Loading client details");
    }

    protected override async Task OnParametersSetAsync()
    {
        if (Id != Guid.Empty)
        {
            await ExecuteAsync(LoadClientAsync, "Loading client details");
        }
    }

    private async Task LoadClientAsync()
    {
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
            throw new InvalidOperationException("Failed to load client details.");
        }
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

        try
        {
            var query = new GetEngagementsByClientQuery(CurrentEngagementsPage, EngagementsPageSize)
            {
                ClientId = Client.ClientId,
                SearchTerm = string.IsNullOrWhiteSpace(EngagementSearchTerm) ? null : EngagementSearchTerm,
                SortField = string.IsNullOrWhiteSpace(EngagementSortField) ? null : EngagementSortField,
                SortDirection = string.IsNullOrWhiteSpace(EngagementSortDirection) ? null : EngagementSortDirection,
                TenantId = ResourceIdeaRequestContext.Tenant
            };

            var response = await Mediator.Send(query);

            PagedEngagementsList = (response is not null && response.IsSuccess && response.Content.HasValue)
                ? response.Content.Value
                : null;
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex, "Loading engagements");
        }
        finally
        {
            IsLoadingEngagements = false;
            StateHasChanged();
        }
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