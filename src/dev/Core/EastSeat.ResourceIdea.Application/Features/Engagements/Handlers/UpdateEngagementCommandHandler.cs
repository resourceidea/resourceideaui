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
    IEngagementsService engagementsService,
    IMapper mapper) : IRequestHandler<UpdateEngagementCommand, ResourceIdeaResponse<EngagementModel>>
{
    private readonly IEngagementsService _engagementsService = engagementsService;
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

        // TODO: Add mapper for UpdateEngagementCommand to Engagement.
        Engagement engagement = _mapper.Map<Engagement>(request);

        var result = await _engagementsService.UpdateAsync(engagement, cancellationToken);

        // TODO: Add mapper for Engagement to EngagementModel.
        return _mapper.Map<ResourceIdeaResponse<EngagementModel>>(result);
    }
}
