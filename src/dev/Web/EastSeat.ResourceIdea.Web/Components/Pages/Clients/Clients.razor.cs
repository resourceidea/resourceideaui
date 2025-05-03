// ==================================================================================================
// File: Clients.razor.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Web\Components\Pages\Clients\Clients.razor.cs
// Description: This file contains the code-behind for the Clients component.
// ==================================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Clients;

public partial class Clients : ComponentBase
{
    private bool IsLoadingPage { get; set; } = true;
    private PagedListResponse<TenantClientModel>? TenantClients { get; set; }

    protected override Task OnInitializedAsync()
    {
        // TODO: Handle initialization logic here.
        IsLoadingPage = false;
        return Task.CompletedTask;
    }

    protected Task HandlePageChangeAsync(int page)
    {
        // Handle page change logic here
        return Task.CompletedTask;
    }
}
