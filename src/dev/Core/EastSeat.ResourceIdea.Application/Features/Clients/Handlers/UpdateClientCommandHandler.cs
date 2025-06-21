using EastSeat.ResourceIdea.Application.Features.Clients.Commands;
using EastSeat.ResourceIdea.Application.Features.Clients.Contracts;
using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Handlers;

/// <summary>
/// Handles command to update a client.
/// </summary>
public sealed class UpdateClientCommandHandler(IClientsService clientsService)
    : BaseHandler,
      IRequestHandler<UpdateClientCommand, ResourceIdeaResponse<ClientModel>>
{
    private readonly IClientsService _clientsService = clientsService;

    public async Task<ResourceIdeaResponse<ClientModel>> Handle(UpdateClientCommand command, CancellationToken cancellationToken)
    {
        ValidationResponse commandValidation = command.Validate();
        if (commandValidation.IsValid is false && commandValidation.ValidationFailureMessages.Any())
        {
            // TODO: Log validation failure.
            return ResourceIdeaResponse<ClientModel>.Failure(ErrorCode.CommandValidationFailure);
        }

        Client client = command.ToEntity();
        var updateClientResponse = await _clientsService.UpdateAsync(client, cancellationToken);
        if (updateClientResponse.IsFailure)
        {
            return ResourceIdeaResponse<ClientModel>.Failure(updateClientResponse.Error);
        }

        return updateClientResponse.Content.Value.ToResourceIdeaResponse<Client, ClientModel>();
    }
}