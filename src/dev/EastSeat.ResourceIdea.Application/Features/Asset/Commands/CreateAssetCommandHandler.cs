using AutoMapper;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Asset.Commands;

public class CreateAssetCommandHandler(IMapper mapper, IAsyncRepository<Domain.Entities.Asset> assetRepository) : IRequestHandler<CreateAssetCommand, CreateAssetCommandResponse>
{
    private readonly IMapper mapper = mapper;
    private readonly IAsyncRepository<Domain.Entities.Asset> assetRepository = assetRepository;

    /// <inheritdoc/>
    public async Task<CreateAssetCommandResponse> Handle(CreateAssetCommand request, CancellationToken cancellationToken)
    {
        var createAssetCommandResponse = new CreateAssetCommandResponse();

        var validator = new CreateAssetCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Count > 0)
        {
            createAssetCommandResponse.Success = false;
            createAssetCommandResponse.Errors = new List<string>();
            foreach (var error in validationResult.Errors)
            {
                createAssetCommandResponse.Errors.Add(error.ErrorMessage);
            }
        }

        if (createAssetCommandResponse.Success)
        {
            var asset = new Domain.Entities.Asset
            {
                Id = request.Id,
                Description = request.Description,
                SubscriptionId = request.SubscriptionId
            };
            asset = await assetRepository.AddAsync(asset);
            createAssetCommandResponse.Content = mapper.Map<CreateAssetDTO>(asset);
        }

        return createAssetCommandResponse;
    }
}
