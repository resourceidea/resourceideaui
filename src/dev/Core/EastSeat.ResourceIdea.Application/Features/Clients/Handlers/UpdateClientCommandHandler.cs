namespace EastSeat.ResourceIdea.Application.Features.Clients.Handlers
{
    using AutoMapper;

    using EastSeat.ResourceIdea.Application.Enums;
    using EastSeat.ResourceIdea.Application.Features.Clients.Commands;
    using EastSeat.ResourceIdea.Application.Features.Clients.Contracts;
    using EastSeat.ResourceIdea.Application.Features.Clients.Validators;
    using EastSeat.ResourceIdea.Application.Types;
    using EastSeat.ResourceIdea.Domain.Clients.Entities;
    using EastSeat.ResourceIdea.Domain.Clients.Models;

    using MediatR;

    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Command handler for updating client.
    /// </summary>
    public sealed class UpdateClientCommandHandler(
        IClientsService clientsService,
        IMapper mapper) : IRequestHandler<UpdateClientCommand, ResourceIdeaResponse<ClientModel>>
    {
        private readonly IClientsService _clientsService = clientsService;
        private readonly IMapper _mapper = mapper;

        public async Task<ResourceIdeaResponse<ClientModel>> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            var commandValidation = await ValidateCommand(request, cancellationToken);
            if (commandValidation.IsFailure)
            {
                return commandValidation;
            }

            Client client = GetClientUpdate(request);
            var response = await _clientsService.UpdateAsync(client, cancellationToken);
            if (response.IsFailure)
            {
                return ResourceIdeaResponse<ClientModel>.Failure(ErrorCode.DataStoreCommandFailure);
            }

            return ResourceIdeaResponse<ClientModel>.Success(_mapper.Map<ClientModel>(response));
        }

        private static Client GetClientUpdate(UpdateClientCommand request)
        {
            return new()
            {
                Id = request.ClientId,
                Name = request.Name,
                Address = request.Address,
                TenantId = request.TenantId.Value
            };
        }

        private static async Task<ResourceIdeaResponse<ClientModel>> ValidateCommand(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            var commandValidationResponse = ResourceIdeaResponse<ClientModel>.Success(Optional<ClientModel>.None);

            UpdateClientCommandValidator validator = new();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsValid is false || validationResult.Errors.Count > 0)
            {
                commandValidationResponse = ResourceIdeaResponse<ClientModel>.Failure(ErrorCode.UpdateClientCommandValidationFailure);
            }

            return commandValidationResponse;
        }
    }
}