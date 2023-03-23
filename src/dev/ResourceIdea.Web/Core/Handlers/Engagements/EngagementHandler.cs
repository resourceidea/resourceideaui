namespace ResourceIdea.Web.Core.Handlers.Engagements;

public class EngagementHandler : IEngagementHandler
{
    private readonly ResourceIdeaDBContext dbContext;

    public EngagementHandler(ResourceIdeaDBContext dbContext)
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
    public async Task<EngagementViewModel?> GetEngagementByIdAsync(string? subscriptionCode, string? clientId, string? engagementId)
    {
        if (subscriptionCode is null)
        {
            throw new MissingSubscriptionCodeException();
        }

        ArgumentNullException.ThrowIfNull(clientId);
        ArgumentNullException.ThrowIfNull(engagementId);

        EngagementViewModel? result = null;

        var engagementQuery = await dbContext.Projects.SingleOrDefaultAsync(c => c.ProjectId == engagementId && c.ClientId == clientId);
        if (engagementQuery is not null)
        {
            result = new EngagementViewModel(
                engagementQuery.ProjectId,
                engagementQuery.Name,
                engagementQuery.ClientId,
                engagementQuery.Color
            );
        }

        return result;
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(string? subscriptionCode, EngagementViewModel input)
    {
        if (subscriptionCode is null)
        {
            throw new MissingSubscriptionCodeException();
        }

        ArgumentNullException.ThrowIfNull(input.ProjectId, nameof(input.ProjectId));

        var engagementForUpdate = await dbContext.Projects
            .SingleOrDefaultAsync(p => p.Client.CompanyCode == subscriptionCode
                                       && p.ProjectId == input.ProjectId
                                       && p.ClientId == input.ClientId);

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

        var data = dbContext.Projects
            .Where(p => p.Client.CompanyCode == subscriptionCode
                            && p.ClientId == clientId);

        if (search != null)
        {
            data = data.Where(d => d.Name.Contains(search));
        }

        return await data.Select(p => new EngagementViewModel(
                p.ProjectId,
                p.Name,
                p.ClientId,
                p.Color))
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<string> AddAsync(string? subscriptionCode, EngagementViewModel engagement)
    {
        if (subscriptionCode is null)
        {
            throw new MissingSubscriptionCodeException();
        }

        ArgumentNullException.ThrowIfNull(engagement);

        var result = await dbContext.Projects
            .AddAsync(new Project
            {
                ProjectId = engagement.ProjectId ?? Guid.NewGuid().ToString(),
                Name = engagement.Name ?? "NA",
                ClientId = engagement.ClientId ?? "NA"
            });
        await dbContext.SaveChangesAsync();

        return result.Entity.ProjectId;
    }
}