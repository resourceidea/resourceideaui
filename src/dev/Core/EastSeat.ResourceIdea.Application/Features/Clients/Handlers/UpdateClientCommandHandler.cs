namespace EastSeat.ResourceIdea.Application.Features.Clients.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using EastSeat.ResourceIdea.Application.Features.Clients.Commands;
    using EastSeat.ResourceIdea.Application.Features.Clients.Validators;
    using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
    using EastSeat.ResourceIdea.Application.Types;
    using EastSeat.ResourceIdea.Domain.Clients.Entities;
    using EastSeat.ResourceIdea.Domain.Clients.Models;
    using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
    using EastSeat.ResourceIdea.Domain.Enums;
    using MediatR;

    /// <summary>
    /// Command handler for updating client.
    /// </summary>
    public sealed class UpdateClientCommandHandler(
        IAsyncRepository<Client>  clientRepository,
        IMapper mapper) : IRequestHandler<UpdateClientCommand, ResourceIdeaResponse<ClientModel>>
    {
        private readonly IAsyncRepository<Client> _clientRepository = clientRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ResourceIdeaResponse<ClientModel>> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            UpdateClientCommandValidator validator = new();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsValid || validationResult.Errors.Count > 0)
            {
                return new ResourceIdeaResponse<ClientModel>
                {
                    Success = false,
                    Message = "Update client command validation failed",
                    ErrorCode = ErrorCode.UpdateClientCommandValidationFailure.ToString(),
                    Content = Optional<ClientModel>.None
                };
            }

            Client client = new()
            {
                Id = request.ClientId,
                Name = request.Name,
                Address = request.Address,
                TenantId = request.TenantId.Value
            };
            var updatedClient = await _clientRepository.UpdateAsync(client, cancellationToken);

            return new ResourceIdeaResponse<ClientModel>
            {
                Success = true,
                Message = "Client updated successfully",
                Content = Optional<ClientModel>.Some(_mapper.Map<ClientModel>(updatedClient))
            };
        }
    }
}