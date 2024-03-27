using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Domain.Entities;
using EastSeat.ResourceIdea.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;

namespace EastSeat.ResourceIdea.Persistence.Repositories;

/// <summary>
/// Repository for the subscription records.
/// </summary>
public class SubscriptionRepository(ResourceIdeaDbContext dbContext) : ISubscriptionRepository
{
    public Task<Subscription> AddAsync(Subscription model)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Subscription?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<PagedList<Subscription>> GetPagedListAsync(int page, int size, Expression<Func<Subscription, bool>>? filter = null)
    {
        throw new NotImplementedException();
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

    public Task<IReadOnlyList<Subscription>> ListAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Subscription> UpdateAsync(Subscription model)
    {
        throw new NotImplementedException();
    }
}
