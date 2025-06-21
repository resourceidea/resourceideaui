// ==================================================================================================
// File: WorkItems.razor.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Web\Components\Pages\WorkItems\WorkItems.razor.cs
// Description: This file contains the code-behind for the WorkItems component.
// ==================================================================================================

using EastSeat.ResourceIdea.Application.Features.WorkItems.Queries;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.WorkItems.Models;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Web.RequestContext;
using EastSeat.ResourceIdea.Web.Components.Base;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace EastSeat.ResourceIdea.Web.Components.Pages.WorkItems;

public partial class WorkItems : ResourceIdeaComponentBase
{
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;
    [Inject] private IMediator Mediator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    private PagedListResponse<WorkItemModel>? PagedWorkItemsList { get; set; }

    private int CurrentPage { get; set; } = 1;
    private const int PageSize = 10;
    private string SearchTerm { get; set; } = string.Empty;
    private string SortField { get; set; } = string.Empty;
    private string SortDirection { get; set; } = "asc";

    // Navigation context
    private Guid? EngagementIdParam { get; set; }
    private Guid? ClientIdParam { get; set; }
    private string NavigationSource { get; set; } = string.Empty;
    private bool ShowBackButton => !string.IsNullOrEmpty(NavigationSource);

    protected override async Task OnInitializedAsync()
    {
        ParseQueryParameters();
        await LoadWorkItems();
        IsLoadingPage = false;
        StateHasChanged();
    }

    private void ParseQueryParameters()
    {
        var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
        var queryParams = QueryHelpers.ParseQuery(uri.Query);

        if (queryParams.TryGetValue("engagementid", out var engagementIdValue) &&
            Guid.TryParse(engagementIdValue.ToString(), out var engagementId))
        {
            EngagementIdParam = engagementId;
            NavigationSource = "engagement";
        }

        if (queryParams.TryGetValue("clientid", out var clientIdValue) &&
            Guid.TryParse(clientIdValue.ToString(), out var clientId))
        {
            ClientIdParam = clientId;
            NavigationSource = "client";
        }
    }

    private async Task LoadWorkItems()
    {
        await ExecuteAsync(async () =>
        {
            ResourceIdeaResponse<PagedListResponse<WorkItemModel>>? response = null;

            if (EngagementIdParam.HasValue)
            {
                var query = new GetWorkItemsByEngagementQuery(
                    EngagementId.Create(EngagementIdParam.Value),
                    CurrentPage,
                    PageSize,
                    SearchTerm)
                {
                    TenantId = ResourceIdeaRequestContext.Tenant
                };
                response = await Mediator.Send(query);
            }
            else if (ClientIdParam.HasValue)
            {
                var query = new GetWorkItemsByClientQuery(
                    ClientId.Create(ClientIdParam.Value),
                    CurrentPage,
                    PageSize,
                    SearchTerm)
                {
                    TenantId = ResourceIdeaRequestContext.Tenant
                };
                response = await Mediator.Send(query);
            }
            else
            {
                var query = new GetAllWorkItemsQuery(CurrentPage, PageSize, SearchTerm)
                {
                    TenantId = ResourceIdeaRequestContext.Tenant
                };
                response = await Mediator.Send(query);
            }

            if (response?.IsSuccess == true && response.Content.HasValue)
            {
                PagedWorkItemsList = response.Content.Value;
            }
            else
            {
                PagedWorkItemsList = null;
                throw new InvalidOperationException("Failed to load work items. Please try again.");
            }
        }, "Loading work items");
    }

    protected async Task HandlePageChangeAsync(int page)
    {
        CurrentPage = page;
        await LoadWorkItems();
    }

    protected async Task HandleSearchTermChangeAsync(string searchTerm)
    {
        SearchTerm = searchTerm;
        CurrentPage = 1; // Reset to first page when searching
        await LoadWorkItems();
    }

    protected async Task HandleSortChangeAsync((string field, string direction) sortInfo)
    {
        SortField = sortInfo.field;
        SortDirection = sortInfo.direction;
        CurrentPage = 1; // Reset to first page when sorting
        await LoadWorkItems();
    }

    private string GetPageTitle()
    {
        return NavigationSource switch
        {
            "engagement" => "Work Items - Engagement",
            "client" => "Work Items - Client",
            _ => "Work Items"
        };
    }

    private string GetBackButtonText()
    {
        return NavigationSource switch
        {
            "engagement" => "Back to engagement details",
            "client" => "Back to client details",
            _ => "Back"
        };
    }

    private string GetBackNavigationUrl()
    {
        return NavigationSource switch
        {
            "engagement" when EngagementIdParam.HasValue => $"/engagements/{EngagementIdParam.Value}",
            "client" when ClientIdParam.HasValue => $"/clients/{ClientIdParam.Value}",
            _ => "/"
        };
    }

    private string GetAddWorkItemUrl()
    {
        if (EngagementIdParam.HasValue && ClientIdParam.HasValue)
        {
            return $"/workitems/add?clientid={ClientIdParam.Value}&engagementid={EngagementIdParam.Value}";
        }
        else if (ClientIdParam.HasValue)
        {
            return $"/workitems/add?clientid={ClientIdParam.Value}";
        }
        else if (EngagementIdParam.HasValue)
        {
            return $"/workitems/add?engagementid={EngagementIdParam.Value}";
        }
        else
        {
            return "/workitems/add";
        }
    }
}