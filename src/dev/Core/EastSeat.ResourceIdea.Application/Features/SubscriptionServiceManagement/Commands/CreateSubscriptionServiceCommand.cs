using EastSeat.ResourceIdea.Domain.Common.Responses;
using EastSeat.ResourceIdea.Domain.SubscriptionServiceManagement.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServiceManagement.Commands;

public sealed class CreateSubscriptionServiceCommand : IRequest<ResourceIdeaResponse<SubscriptionServiceModel>>
{
    /// <summary>Service name</summary>
    public string Name { get; set; } = string.Empty;
}