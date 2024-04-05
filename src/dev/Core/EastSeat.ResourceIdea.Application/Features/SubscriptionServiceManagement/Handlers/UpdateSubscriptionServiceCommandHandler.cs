using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServiceManagement.Commands;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServiceManagement.Validators;
using EastSeat.ResourceIdea.Domain.Common.Constants;
using EastSeat.ResourceIdea.Domain.Common.Exceptions;
using EastSeat.ResourceIdea.Domain.Common.Responses;
using EastSeat.ResourceIdea.Domain.SubscriptionServiceManagement.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServiceManagement.Models;
using EastSeat.ResourceIdea.Domain.SubscriptionServiceManagement.ValueObjects;
using EastSeat.ResourceIdea.Domain.TenantManagement.Entities;

using MediatR;

using Optional;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServiceManagement.Handlers;

public sealed class UpdateSubscriptionServiceCommandHandler(
    IAsyncRepository<SubscriptionService> subscriptionServiceRepository,
    IMapper mapper) : IRequestHandler<UpdateSubscriptionServiceCommand, ResourceIdeaResponse<SubscriptionServiceModel>>
{
    private readonly IAsyncRepository<SubscriptionService> _subscriptionServiceRepository = subscriptionServiceRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<SubscriptionServiceModel>> Handle(
        UpdateSubscriptionServiceCommand request,
        CancellationToken cancellationToken)
    {
        UpdateSubscriptionServiceCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid || validationResult.Errors.Count > 0)
        {
            return new ResourceIdeaResponse<SubscriptionServiceModel>
            {
                Success = false,
                Message = "Subscription service update command validation failed",
                ErrorCode = ErrorCodes.UpdateSubscriptionServiceCommandValidationFailure.ToString(),
                Content = Option.None<SubscriptionServiceModel>()
            };
        }

        SubscriptionService subscriptionService = new()
        {
            Id = request.Id,
            Name = request.Name
        };

        Option<SubscriptionService> subscriptionServiceUpdateResult = await _subscriptionServiceRepository.UpdateAsync(
            subscriptionService,
            cancellationToken);
        SubscriptionService updatedSubscriptionService = subscriptionServiceUpdateResult.Match(
            some: subscriptionService => subscriptionService,
            none: () => throw new UpdateItemNotFoundException("Update tenant to be updated was not found.")
        );

        return new ResourceIdeaResponse<SubscriptionServiceModel>
        {
            Success = true,
            Message = "Subscription service created successfully",
            Content = Option.Some(_mapper.Map<SubscriptionServiceModel>(newSubscriptionService))
        };
    }
}
