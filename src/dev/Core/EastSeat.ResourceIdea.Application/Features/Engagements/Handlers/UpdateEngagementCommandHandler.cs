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

public sealed class UpdateEngagementCommandHandler (
    IEngagementRepository engagementRepository,
    IMapper mapper) : IRequestHandler<UpdateEngagementCommand, ResourceIdeaResponse<EngagementModel>>
{
    private readonly IEngagementRepository _engagementRepository = engagementRepository;
    private readonly IMapper _mapper = mapper;
    
    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<EngagementModel>> Handle(
        UpdateEngagementCommand request,
        CancellationToken cancellationToken)
    {
        UpdateEngagementCommandValidator updateEngagementValidator = new();
        var validationResult = updateEngagementValidator.Validate(request);
        
        if (!validationResult.IsValid || validationResult.Errors.Count > 0)
        {
            return new ResourceIdeaResponse<EngagementModel>
            {
                Success = false,
                Message = "Invalid update engagement command. Please check the command and try again.",
                ErrorCode = ErrorCodes.UpdateEngagementCommandValidationFailure.ToString(),
                Content = Optional<EngagementModel>.None
            };
        }

        Engagement engagement = new()
        {
            Id = request.EngagementId,
            CommencementDate = request.CommencementDate,
            CompletionDate = request.CompletionDate,
            EngagementStatus = request.Status
        };

        var updatedEngagement = await _engagementRepository.UpdateAsync(engagement, cancellationToken);

        return new ResourceIdeaResponse<EngagementModel>
        {
            Success = true,
            Message = $"Engagement updated successfully.",
            Content = Optional<EngagementModel>.Some(_mapper.Map<EngagementModel>(updatedEngagement))
        };
    }
}
