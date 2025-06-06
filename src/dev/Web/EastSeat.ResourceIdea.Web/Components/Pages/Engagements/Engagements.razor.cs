// ==================================================================================================
// File: Engagements.razor.cs
// Path: src\dev\Web\EastSeat.ResourceIdea.Web\Components\Pages\Engagements\Engagements.razor.cs
// Description: This file contains the code-behind for the Engagements component.
// ==================================================================================================

using EastSeat.ResourceIdea.Application.Features.Engagements.Queries;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Web.RequestContext;
using EastSeat.ResourceIdea.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Engagements;

public partial class Engagements : ComponentBase
{
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;
    [Inject] private IMediator Mediator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;

    private bool IsLoading { get; set; } = true;
    private bool HasError { get; set; }
    private string ErrorMessage { get; set; } = string.Empty;
    
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
        await LoadEngagementsAsync();
        IsLoading = false;
    }

    private async Task LoadEngagementsAsync()
    {
        IsLoadingEngagements = true;
        StateHasChanged();

        try
        {
            var query = new GetAllEngagementsQuery(CurrentEngagementsPage, EngagementsPageSize);
            var response = await Mediator.Send(query);

            PagedEngagementsList = (response is not null && response.IsSuccess && response.Content.HasValue)
                ? response.Content.Value
                : null;

            if (response?.IsFailure == true)
            {
                HasError = true;
                ErrorMessage = GetErrorMessage(response.Error);
            }
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = $"An error occurred while loading engagements: {ex.Message}";
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
        // Note: Search is not currently supported by GetAllEngagementsQuery
        // This is a placeholder for future enhancement
        EngagementSearchTerm = searchTerm;
        // Don't reload data since search isn't supported yet
    }

    private async Task OnEngagementSortChange((string field, string direction) sortInfo)
    {
        // Note: Sorting is not currently supported by GetAllEngagementsQuery  
        // This is a placeholder for future enhancement
        EngagementSortField = sortInfo.field;
        EngagementSortDirection = sortInfo.direction;
        // Don't reload data since sorting isn't supported yet
    }

    private static string GetErrorMessage(ErrorCode errorCode)
    {
        return errorCode switch
        {
            ErrorCode.NotFound => "Engagements not found.",
            ErrorCode.DataStoreQueryFailure => "Failed to query engagements from the data store.",
            ErrorCode.BadRequest => "Invalid request to load engagements.",
            _ => "Failed to load engagements."
        };
    }
}