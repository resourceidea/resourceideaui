using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Subscriptions.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Commands;

/// <summary>
/// Command to create a subscription.
/// </summary>
public sealed class CreateSubscriptionCommand : IRequest<ResourceIdeaResponse<SubscriptionModel>> { }