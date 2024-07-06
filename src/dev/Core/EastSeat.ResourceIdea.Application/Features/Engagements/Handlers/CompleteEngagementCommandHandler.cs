using AutoMapper;
using EastSeat.ResourceIdea.Application.Features.Engagements.Commands;
using EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;
using EastSeat.ResourceIdea.Application.Features.Engagements.Validators;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Enums;
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

        if (validationResult.IsValid || validationResult.Errors.Count > 0)
        {
            return new ResourceIdeaResponse<EngagementModel>
            {
                Success = false,
                Message = "Invalid complete engagement command. Please check the command and try again.",
                ErrorCode = ErrorCodes.CompleteEngagementCommandValidationFailure.ToString(),
                Content = Optional<EngagementModel>.None
            };
        }

        Engagement engagement = new()
        {
            Id = request.EngagementId
        };

        var completedEngagement = await _engagementRepository.CompleteAsync(engagement, cancellationToken);

        return new ResourceIdeaResponse<EngagementModel>
        {
            Success = true,
            Message = $"Engagement completed successfully.",
            Content = Optional<EngagementModel>.Some(_mapper.Map<EngagementModel>(completedEngagement))
        };
    }
}
