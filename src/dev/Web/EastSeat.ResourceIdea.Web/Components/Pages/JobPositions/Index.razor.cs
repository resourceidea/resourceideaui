// ----------------------------------------------------------------------------
// File: Index.razor.cs
// Path: src\dev\Web\EastSeat.ResourceIdea.Web\Components\Pages\JobPositions\Index.razor.cs
// Description: Index page for job positions.
// ----------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Queries;
using EastSeat.ResourceIdea.Domain.JobPositions.Models;

using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Pages.JobPositions;

public partial class Index : ComponentBase
{
    public PagedListResponse<TenantJobPositionModel> JobPositions { get; set; } = new();
    public bool IsLoadingModelData;

    protected override async Task OnInitializedAsync() => await LoadJobPositionsAsync();

    protected async Task HandlePageChangeAsync(int page) => await LoadJobPositionsAsync(page);

    private async Task LoadJobPositionsAsync(int page = 1)
    {
        IsLoadingModelData = true;

        var query = new GetAllJobPositionsQuery
        {
            TenantId = ResourceIdeaRequestContext.Tenant,
            PageNumber = page,
            PageSize = 10
        };

        var response = await Mediator.Send(query);
        if (response.IsSuccess && response.Content.HasValue)
        {
            JobPositions = response.Content.Value;
        }
        else
        {
            // TODO: Display message for failure to get job positions
        }

        IsLoadingModelData = false;
    }
}
