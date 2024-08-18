namespace EastSeat.ResourceIdea.Application.Features.Clients.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using EastSeat.ResourceIdea.Application.Enums;
    using EastSeat.ResourceIdea.Application.Features.Clients.Commands;
    using EastSeat.ResourceIdea.Application.Features.Clients.Validators;
    using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
    using EastSeat.ResourceIdea.Application.Types;
    using EastSeat.ResourceIdea.Domain.Clients.Entities;
    using EastSeat.ResourceIdea.Domain.Clients.Models;
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
            if (validationResult.IsValid is false || validationResult.Errors.Count > 0)
            {
                return ResourceIdeaResponse<ClientModel>.Failure(ErrorCode.UpdateClientCommandValidationFailure);
            }

            Client client = new()
            {
                Id = request.ClientId,
                Name = request.Name,
                Address = request.Address,
                TenantId = request.TenantId.Value
            };
            var updatedClient = await _clientRepository.UpdateAsync(client, cancellationToken);

            return ResourceIdeaResponse<ClientModel>.Success(Optional<ClientModel>.Some(_mapper.Map<ClientModel>(updatedClient)));
        }
    }
}