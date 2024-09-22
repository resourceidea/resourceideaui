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
    IEngagementsService engagementsService,
    IMapper mapper): IRequestHandler<CompleteEngagementCommand, ResourceIdeaResponse<EngagementModel>>
{
    private readonly IEngagementsService _engagementsService = engagementsService;
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
            return ResourceIdeaResponse<EngagementModel>.Failure(ErrorCode.DataStoreCommandFailure);
        }

        var completedEngagement = await _engagementsService.CompleteAsync(
            request.EngagementId,
            request.CompletionDate,
            cancellationToken);

        return ResourceIdeaResponse<EngagementModel>.Success(_mapper.Map<EngagementModel>(completedEngagement));
    }
}
