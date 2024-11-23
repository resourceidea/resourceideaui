using AutoMapper;
using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.Engagements.Commands;
using EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;
using EastSeat.ResourceIdea.Application.Features.Engagements.Validators;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Handlers;

public sealed class StartEngagementCommandHandler (
    IEngagementsService engagementsService,
    IMapper mapper) : IRequestHandler<StartEngagementCommand, ResourceIdeaResponse<EngagementModel>>
{
    private readonly IEngagementsService _engagementsService = engagementsService;
    private readonly IMapper _mapper = mapper;
    
    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<EngagementModel>> Handle(
        StartEngagementCommand request,
        CancellationToken cancellationToken)
    {
        StartEngagementCommandValidator startEngagementValidator = new();
        var validationResult = startEngagementValidator.Validate(request);
        
        if (validationResult.IsValid is false || validationResult.Errors.Count > 0)
        {
            return ResourceIdeaResponse<EngagementModel>.Failure(ErrorCode.DataStoreCommandFailure);
        }

        var startedEngagement = await _engagementsService.StartAsync(
            request.EngagementId,
            request.CommencementDate,
            cancellationToken);

        return ResourceIdeaResponse<EngagementModel>
                    .Success(Optional<EngagementModel>.Some(_mapper.Map<EngagementModel>(startedEngagement)));
    }
}
