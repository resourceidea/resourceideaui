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

public sealed class StartEngagementCommandHandler (
    IEngagementRepository engagementRepository,
    IMapper mapper) : IRequestHandler<StartEngagementCommand, ResourceIdeaResponse<EngagementModel>>
{
    private readonly IEngagementRepository _engagementRepository = engagementRepository;
    private readonly IMapper _mapper = mapper;
    
    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<EngagementModel>> Handle(
        StartEngagementCommand request,
        CancellationToken cancellationToken)
    {
        StartEngagementCommandValidator startEngagementValidator = new();
        var validationResult = startEngagementValidator.Validate(request);
        
        if (!validationResult.IsValid || validationResult.Errors.Count > 0)
        {
            return new ResourceIdeaResponse<EngagementModel>
            {
                Success = false,
                Message = "Invalid start engagement command. Please check the command and try again.",
                ErrorCode = ErrorCodes.StartEngagementCommandValidationFailure.ToString(),
                Content = Optional<EngagementModel>.None
            };
        }

        Engagement engagement = new()
        {
            Id = request.EngagementId,
            CommencementDate = request.CommencementDate,
            EngagementStatus = EngagementStatus.InProgress
        };

        var startedEngagement = await _engagementRepository.StartAsync(engagement, cancellationToken);

        return new ResourceIdeaResponse<EngagementModel>
        {
            Success = true,
            Message = $"Engagement started successfully.",
            Content = Optional<EngagementModel>.Some(_mapper.Map<EngagementModel>(startedEngagement))
        };
    }
}
