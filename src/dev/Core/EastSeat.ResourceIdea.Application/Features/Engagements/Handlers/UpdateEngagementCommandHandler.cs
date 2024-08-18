using AutoMapper;
using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.Engagements.Commands;
using EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;
using EastSeat.ResourceIdea.Application.Features.Engagements.Validators;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
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
        
        if (validationResult.IsValid is false || validationResult.Errors.Count > 0)
        {
            return ResourceIdeaResponse<EngagementModel>.Failure(ErrorCode.UpdateEngagementCommandValidationFailure);
        }

        Engagement engagement = new()
        {
            Id = request.EngagementId,
            CommencementDate = request.CommencementDate,
            CompletionDate = request.CompletionDate,
            EngagementStatus = request.Status
        };

        var updatedEngagement = await _engagementRepository.UpdateAsync(engagement, cancellationToken);

        return ResourceIdeaResponse<EngagementModel>
                    .Success(Optional<EngagementModel>.Some(_mapper.Map<EngagementModel>(updatedEngagement)));
    }
}
