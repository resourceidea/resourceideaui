// ==================================================================================================
// File: Engagements.razor.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Web\Components\Pages\Engagements\Engagements.razor.cs
// Description: This file contains the code-behind for the Engagements component.
// ==================================================================================================

using EastSeat.ResourceIdea.Application.Features.Engagements.Queries;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Web.RequestContext;
using EastSeat.ResourceIdea.Web.Components.Base;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Engagements;

public partial class Engagements : ResourceIdeaComponentBase
{
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;
    [Inject] private IMediator Mediator { get; set; } = null!;

    private PagedListResponse<EngagementModel>? TenantEngagements { get; set; }
    private int CurrentPage { get; set; } = 1;
    private const int PageSize = 10;
    private bool showAddEngagementModal = false;

    protected override async Task OnInitializedAsync()
    {
        await ExecuteAsync(async (cancellationToken) =>
        {
            await LoadTenantEngagements();
        }, "Loading engagements");
    }

    private async Task LoadTenantEngagements()
    {
        GetAllEngagementsQuery query = new(CurrentPage, PageSize)
        {
            TenantId = ResourceIdeaRequestContext.Tenant
        };

        var response = await Mediator.Send(query);
        // Handle error response: For now, just set to null if the condition fails.
        TenantEngagements = response.IsSuccess && response.Content.HasValue
            ? response.Content.Value
            : null;
    }

    protected async Task HandlePageChangeAsync(int page)
    {
        CurrentPage = page;
        await ExecuteAsync(async (cancellationToken) =>
        {
            await LoadTenantEngagements();
        }, "Loading page");
    }

    private void ShowAddEngagementModal()
    {
        showAddEngagementModal = true;
    }

    private async Task OnEngagementCreated()
    {
        // Refresh the engagements list
        await ExecuteAsync(async (cancellationToken) =>
        {
            await LoadTenantEngagements();
        }, "Refreshing engagements list");
    }
}