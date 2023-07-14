namespace ResourceIdea.Web.Core.Handlers.Engagements;

public class EngagementService : IEngagementService
{
    private readonly ResourceIdeaDBContext dbContext;

    public EngagementService(ResourceIdeaDBContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<IList<EngagementViewModel>> GetPaginatedResultAsync(
        string? subscriptionCode,
        string? clientId,
        int currentPage,
        int pageSize = 10,
        string? search = null)
    {
        if (subscriptionCode is null)
        {
            throw new MissingSubscriptionCodeException();
        }

        ArgumentNullException.ThrowIfNull(clientId);

        var data = await GetDataAsync(subscriptionCode, clientId, search);
        return data.OrderBy(d => d.Name)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    /// <inheritdoc />
    public async Task<int> GetCountAsync(string? subscriptionCode, string? clientId, string? search)
    {
        if (subscriptionCode is null)
        {
            throw new MissingSubscriptionCodeException();
        }

        ArgumentNullException.ThrowIfNull(clientId);

        var data = await GetDataAsync(subscriptionCode, clientId, search);
        return data.Count;
    }

    /// <inheritdoc />
    public async Task<EngagementViewModel?> GetEngagementByIdAsync(string? subscriptionCode, string? engagementId)
    {
        if (subscriptionCode is null)
        {
            throw new MissingSubscriptionCodeException();
        }

        ArgumentNullException.ThrowIfNull(engagementId);
        EngagementViewModel? engagementView = null;
        var engagementQuery = await dbContext.Engagements
                                             .Include(engagement => engagement.Client)
                                             .FirstOrDefaultAsync(engagement => engagement.EngagementId == engagementId
                                                                                          && engagement.Client.CompanyCode == subscriptionCode);
        if (engagementQuery is not null)
        {
            engagementView = new EngagementViewModel
            {
                EngagementId = engagementQuery.EngagementId,
                Name = engagementQuery.Name,
                ClientId = engagementQuery.ClientId,
                Color = engagementQuery.Color,
                Client = engagementQuery.Client.Name
            };
        }

        return engagementView;
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(string? subscriptionCode, EngagementViewModel input)
    {
        if (subscriptionCode is null)
        {
            throw new MissingSubscriptionCodeException();
        }

        ArgumentNullException.ThrowIfNull(input.EngagementId, nameof(input.EngagementId));
        var engagementForUpdate = await dbContext.Engagements
            .SingleOrDefaultAsync(engagement => engagement.Client.CompanyCode == subscriptionCode
                                             && engagement.EngagementId == input.EngagementId
                                             && engagement.ClientId == input.ClientId);

        if (engagementForUpdate is not null)
        {
            engagementForUpdate.Color = input.Color;
            engagementForUpdate.Name = input.Name ?? "NA";

            await dbContext.SaveChangesAsync();
        }
    }

    private async Task<IList<EngagementViewModel>> GetDataAsync(string? subscriptionCode, string? clientId, string? search)
    {
        if (subscriptionCode is null)
        {
            throw new MissingSubscriptionCodeException();
        }

        ArgumentNullException.ThrowIfNull(clientId);

        var data = dbContext.Engagements
            .Where(p => p.Client.CompanyCode == subscriptionCode &&
                        p.ClientId == clientId);

        if (search != null)
        {
            data = data.Where(d => d.Name.Contains(search));
        }

        return await data.Select(engagement => new EngagementViewModel
        {
            EngagementId = engagement.EngagementId,
            Name = engagement.Name,
            ClientId = engagement.ClientId,
            Color = engagement.Color,
            Client = engagement.Client.Name
        }).ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<string> AddAsync(string? subscriptionCode, EngagementViewModel engagement)
    {
        if (subscriptionCode is null)
        {
            throw new MissingSubscriptionCodeException();
        }

        ArgumentNullException.ThrowIfNull(engagement);

        var result = await dbContext.Engagements
            .AddAsync(new Engagement
            {
                EngagementId = engagement.EngagementId ?? Guid.NewGuid().ToString(),
                Name = engagement.Name ?? "NA",
                ClientId = engagement.ClientId ?? "NA",
            });
        await dbContext.SaveChangesAsync();

        return result.Entity.EngagementId;
    }
}