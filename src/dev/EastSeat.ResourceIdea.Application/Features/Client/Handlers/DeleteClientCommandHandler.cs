using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Features.Client.Commands;
using EastSeat.ResourceIdea.Application.Features.Client.Validators;
using EastSeat.ResourceIdea.Application.Responses;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Client.Handlers
{
    public class DeleteClientCommandHandler(IAsyncRepository<Domain.Entities.Client> clientRepository) : IRequestHandler<DeleteClientCommand, BaseResponse<Unit>>
    {
        private readonly IAsyncRepository<Domain.Entities.Client> clientRepository = clientRepository;

        public async Task<BaseResponse<Unit>> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<Unit>();

            var validator = new DeleteClientCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid || validationResult.Errors.Count > 0)
            {
                response.Success = false;
                response.Errors = [];
                foreach (var error in validationResult.Errors)
                {
                    response.Errors.Add(error.ErrorMessage);
                }
            }

            if (response.Success)
            {
                await clientRepository.DeleteAsync(request.Id);
                response.Content = Unit.Value;
            }

            return response;
        }
    }
}
