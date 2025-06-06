// ==================================================================================================
// File: Engagements.razor.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Web\Components\Pages\Engagements\Engagements.razor.cs
// Description: This file contains the code-behind for the Engagements component.
// ==================================================================================================

using EastSeat.ResourceIdea.Application.Features.Engagements.Queries;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Web.RequestContext;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Engagements;

public partial class Engagements : ComponentBase
{
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;
    [Inject] private IMediator Mediator { get; set; } = null!;
    
    private bool IsLoadingPage { get; set; } = true;
    private PagedListResponse<EngagementModel>? TenantEngagements { get; set; }
    private int CurrentPage { get; set; } = 1;
    private const int PageSize = 10;

    protected override async Task OnInitializedAsync()
    {
        await LoadTenantEngagements();
        IsLoadingPage = false;
        StateHasChanged();
    }

    private async Task LoadTenantEngagements()
    {
        IsLoadingPage = true;
        StateHasChanged();

        GetAllEngagementsQuery query = new(CurrentPage, PageSize);

        var response = await Mediator.Send(query);
        if (response.IsSuccess && response.Content.HasValue)
        {
            TenantEngagements = response.Content.Value;
        }
        else
        {
            // Handle error response
            // For now, just set to null
            TenantEngagements = null;
        }

        IsLoadingPage = false;
        StateHasChanged();
    }

    protected async Task HandlePageChangeAsync(int page)
    {
        CurrentPage = page;
        await LoadTenantEngagements();
    }
}