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
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace EastSeat.ResourceIdea.Web.Components.Pages.WorkItems;

public partial class WorkItems : ComponentBase
{
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;
    [Inject] private IMediator Mediator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    
    private bool IsLoadingPage { get; set; } = true;
    private bool HasError { get; set; }
    private string ErrorMessage { get; set; } = string.Empty;
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
        IsLoadingPage = true;
        HasError = false;
        StateHasChanged();

        try
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
                HasError = true;
                ErrorMessage = "Failed to load work items. Please try again.";
                PagedWorkItemsList = null;
            }
        }
        catch (TaskCanceledException ex)
        {
            HasError = true;
            ErrorMessage = "The operation was canceled. Please try again.";
            Console.Error.WriteLine($"Task canceled: {ex}");
        }
        catch (InvalidOperationException ex)
        {
            HasError = true;
            ErrorMessage = "An invalid operation occurred while loading work items.";
            Console.Error.WriteLine($"Invalid operation: {ex}");
        }
        catch (ArgumentException ex)
        {
            HasError = true;
            ErrorMessage = "An error occurred due to invalid input. Please check your parameters.";
            Console.Error.WriteLine($"Argument error: {ex}");
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = "An unexpected error occurred while loading work items.";
            Console.Error.WriteLine($"Unexpected error: {ex}");
        }
        finally
        {
            IsLoadingPage = false;
            StateHasChanged();
        }
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