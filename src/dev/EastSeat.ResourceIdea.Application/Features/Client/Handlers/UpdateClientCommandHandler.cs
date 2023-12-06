using AutoMapper;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Features.Client.Commands;
using EastSeat.ResourceIdea.Application.Features.Client.DTO;
using EastSeat.ResourceIdea.Application.Features.Client.Validators;
using EastSeat.ResourceIdea.Application.Responses;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Client.Handlers;

public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, BaseResponse<ClientDTO>>
{
    private readonly IMapper mapper;
    private readonly IAsyncRepository<Domain.Entities.Client> clientRepository;

    public UpdateClientCommandHandler(IMapper mapper, IAsyncRepository<Domain.Entities.Client> clientRepository)
    {
        this.mapper = mapper;
        this.clientRepository = clientRepository;
    }

    public async Task<BaseResponse<ClientDTO>> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<ClientDTO>();

        var validator = new UpdateClientCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid || validationResult.Errors.Count > 0)
        {
            response.Success = false;
            response.Errors = new List<string>();
            foreach (var error in validationResult.Errors)
            {
                response.Errors.Add(error.ErrorMessage);
            }
        }

        if (response.Success)
        {
            var client = new Domain.Entities.Client
            {
                Id = request.Id,
                Name = request.Name,
                Address = request.Address,
                ColorCode = request.ColorCode,
                SubscriptionId = request.SubscriptionId
            };
            client = await clientRepository.UpdateAsync(client);
            response.Content = mapper.Map<ClientDTO>(client);
        }

        return response;
    }
}
