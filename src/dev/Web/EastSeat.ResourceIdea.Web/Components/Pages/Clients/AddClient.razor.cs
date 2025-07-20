// ======================================================================================
// File: AddClient.razor.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Web\Components\Pages\Clients\AddClient.razor.cs
// Description: This file contains the code-behind for the AddClient component.
// ======================================================================================

using EastSeat.ResourceIdea.Application.Features.Clients.Commands;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Web.Components.Base;
using EastSeat.ResourceIdea.Web.RequestContext;
using EastSeat.ResourceIdea.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Clients;

public partial class AddClient : ResourceIdeaComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = null!;
    [Inject] private IResourceIdeaRequestContext ResourceIdeaRequestContext { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;

    public AddClientCommand Command { get; set; } = new();

    private async Task HandleValidSubmit()
    {
        await ExecuteAsync(async () =>
        {
            Command.TenantId = ResourceIdeaRequestContext.Tenant;
            ValidationResponse commandValidation = Command.Validate();
            if (!commandValidation.IsValid && commandValidation.ValidationFailureMessages.Any())
            {
                // TODO: Log validation failure.
                NotificationService.ShowErrorNotification("Validation failed.");
                return;
            }
            var response = await Mediator.Send(Command);
            if (response.IsSuccess)
            {
                NotificationService.ShowSuccessNotification("Client added successfully.");
                Navigation.NavigateTo("/clients");
            }
            else
            {
                NotificationService.ShowErrorNotification("Failed to add client.");
            }
        }, "Adding new client");
    }
}
