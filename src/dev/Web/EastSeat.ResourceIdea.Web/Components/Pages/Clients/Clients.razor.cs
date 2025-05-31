// ==================================================================================================
// File: Clients.razor.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Web\Components\Pages\Clients\Clients.razor.cs
// Description: This file contains the code-behind for the Clients component.
// ==================================================================================================

using EastSeat.ResourceIdea.Application.Features.Clients.Queries;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Web.RequestContext;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Clients;

public partial class Clients : ComponentBase
{
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;
    [Inject] private IMediator Mediator { get; set; } = null!;
    private bool IsLoadingPage { get; set; } = true;
    private PagedListResponse<TenantClientModel>? TenantClients { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsLoadingPage = false;
        await LoadTenantClients();
        IsLoadingPage = false;
        StateHasChanged();
    }

    private async Task LoadTenantClients()
    {
        TenantClientsQuery query = new()
        {
            TenantId = ResourceIdeaRequestContext.Tenant,
            PageNumber = 1,
            PageSize = 10,
        };

        var response = await Mediator.Send(query);
        if (response.IsSuccess && response.Content != null)
        {
            TenantClients = response.Content;
        }
        else
        {
            // Handle error response
            // For example, show a notification or log the error
        }
    }

    protected Task HandlePageChangeAsync(int page)
    {
        // Handle page change logic here
        return Task.CompletedTask;
    }
}
