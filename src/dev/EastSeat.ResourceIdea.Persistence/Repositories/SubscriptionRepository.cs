using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace EastSeat.ResourceIdea.Persistence.Repositories;

/// <summary>
/// Repository for the subscription records.
/// </summary>
public class SubscriptionRepository : BaseRepository<Subscription>, ISubscriptionRepository
{
    public SubscriptionRepository(ResourceIdeaDbContext dbContext) : base(dbContext)
    {
    }

    /// <inheritdoc/>
    public async Task<bool> IsSubscriberNameAlreadyInUse(string? subscriberName)
    {
        if (string.IsNullOrEmpty(subscriberName))
        {
            return true;
        }

        return await dbContext.Subscriptions.AnyAsync(subscriber => subscriber.SubscriberName == subscriberName);
    }
}
