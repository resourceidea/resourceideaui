// ======================================================================================
// File: AddClient.razor.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Web\Components\Pages\Clients\AddClient.razor.cs
// Description: This file contains the code-behind for the AddClient component.
// ======================================================================================

using EastSeat.ResourceIdea.Application.Features.Clients.Commands;
using EastSeat.ResourceIdea.Web.RequestContext;
using EastSeat.ResourceIdea.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Clients;

public partial class AddClient : ComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;

    public AddClientCommand Command { get; set; } = new();

    private Task HandleValidSubmit()
    {
        throw new NotImplementedException("HandleValidSubmit method is not implemented yet.");
    }
}
