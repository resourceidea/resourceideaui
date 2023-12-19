using AutoMapper;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Features.Client.Commands;
using EastSeat.ResourceIdea.Application.Features.Client.DTO;
using EastSeat.ResourceIdea.Application.Features.Client.Validators;
using EastSeat.ResourceIdea.Application.Responses;

using FluentValidation.Results;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Client.Handlers;

public class UpdateClientCommandHandler(IMapper mapper, IAsyncRepository<Domain.Entities.Client> clientRepository) : IRequestHandler<UpdateClientCommand, BaseResponse<ClientDTO>>
{
    private readonly IMapper mapper = mapper;
    private readonly IAsyncRepository<Domain.Entities.Client> clientRepository = clientRepository;

    public async Task<BaseResponse<ClientDTO>> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<ClientDTO>();

        var validator = new UpdateClientCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (IsFailedValidationResult(validationResult))
        {
            response.Success = false;
            response.ErrorCode = Constants.ErrorCodes.Commands.UpdateClient.ValidationFailure;
            response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
            return response;
        }

        var client = await clientRepository.GetByIdAsync(request.Id);
        if (client is null)
        {
            response.Success = false;
            response.ErrorCode = Constants.ErrorCodes.Commands.UpdateClient.ClientNotFound;
            response.Errors = [
                Constants.ErrorCodes.Commands.UpdateClient.ClientNotFound
                ];
            return response;
        }

        // SubscriptionId is not updated because we don't expect it to be changed by this operation.
        client.Name = request.Name;
        client.Address = request.Address;
        client.ColorCode = request.ColorCode;

        var updatedClient = await clientRepository.UpdateAsync(client);
        response.Content = mapper.Map<ClientDTO>(updatedClient);

        return response;
    }

    private static bool IsFailedValidationResult(ValidationResult validationResult)
    {
        return !validationResult.IsValid || validationResult.Errors.Count > 0;
    }
}
