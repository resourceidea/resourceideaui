using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Contracts;

public interface ISubscriptionsService : IDataStoreService<Subscription>
{
}
