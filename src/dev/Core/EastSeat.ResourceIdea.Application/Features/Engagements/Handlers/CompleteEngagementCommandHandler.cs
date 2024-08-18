using AutoMapper;
using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.Engagements.Commands;
using EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;
using EastSeat.ResourceIdea.Application.Features.Engagements.Validators;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Handlers;

public sealed class CompleteEngagementCommandHandler (
    IEngagementRepository engagementRepository,
    IMapper mapper): IRequestHandler<CompleteEngagementCommand, ResourceIdeaResponse<EngagementModel>>
{
    private readonly IEngagementRepository _engagementRepository = engagementRepository;
    private readonly IMapper _mapper = mapper;

    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<EngagementModel>> Handle(
        CompleteEngagementCommand request,
        CancellationToken cancellationToken)
    {
        CompleteEngagementCommandValidator completeEngagementValidator = new();
        var validationResult = completeEngagementValidator.Validate(request);

        if (!validationResult.IsValid || validationResult.Errors.Count > 0)
        {
            return ResourceIdeaResponse<EngagementModel>.Failure(ErrorCode.CompleteEngagementCommandValidationFailure);
        }

        var completedEngagement = await _engagementRepository.CompleteAsync(request.EngagementId, cancellationToken);

        return ResourceIdeaResponse<EngagementModel>
                    .Success(Optional<EngagementModel>.Some(_mapper.Map<EngagementModel>(completedEngagement)));
    }
}
