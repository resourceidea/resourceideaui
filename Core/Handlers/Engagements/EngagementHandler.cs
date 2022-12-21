using Microsoft.EntityFrameworkCore;

using ResourceIdea.Core.ViewModels;

namespace ResourceIdea.Core.Handlers.Engagements;

public class EngagementHandler : IEngagementHandler
{
    private readonly ResourceIdeaDBContext _dbContext;

    public EngagementHandler(ResourceIdeaDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<IList<EngagementViewModel>> GetPaginatedResultAsync(
        string? subscriptionCode,
        string? clientId,
        int currentPage, 
        int pageSize = 10, 
        string? search = null)
    {
        ArgumentNullException.ThrowIfNull(subscriptionCode);
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
        ArgumentNullException.ThrowIfNull(subscriptionCode);
        ArgumentNullException.ThrowIfNull(clientId);

        var data = await GetDataAsync(subscriptionCode, clientId, search);
        return data.Count;
    }

    /// <inheritdoc />
    public async Task<EngagementViewModel?> GetEngagementByIdAsync(string? subscriptionCode, string? clientId, string? engagementId)
    {
        ArgumentNullException.ThrowIfNull(subscriptionCode);
        ArgumentNullException.ThrowIfNull(clientId);
        ArgumentNullException.ThrowIfNull(engagementId);

        EngagementViewModel? result = null;

        var engagementQuery = await _dbContext.Projects.SingleOrDefaultAsync(c => c.ClientId == engagementId && c.ClientId == clientId);
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

    public async Task UpdateAsync(string? subscriptionCode, EngagementViewModel input)
    {
        ArgumentNullException.ThrowIfNull(subscriptionCode, nameof(subscriptionCode));
        ArgumentNullException.ThrowIfNull(input.ProjectId, nameof(input.ProjectId));

        var engagementForUpdate = await _dbContext.Projects
            .SingleOrDefaultAsync(p => p.Client.CompanyCode == subscriptionCode
                                       && p.ProjectId == input.ProjectId
                                       && p.ClientId == input.ClientId);

        if (engagementForUpdate is not null)
        {
            engagementForUpdate.Color = input.Color;
            engagementForUpdate.Name = input.Name ?? "N/A";

            await _dbContext.SaveChangesAsync();
        }
    }

    private async Task<IList<EngagementViewModel>> GetDataAsync(string? subscriptionCode, string? clientId, string? search)
    {
        ArgumentNullException.ThrowIfNull(subscriptionCode);
        ArgumentNullException.ThrowIfNull(clientId);

        var data = _dbContext.Projects
            .Where(p => p.Client.CompanyCode == subscriptionCode
                            && p.ClientId == clientId);

        if (search != null)
        {
            data = data.Where(d => d.Name.Contains(search));
        }

        return await data.Select(p => new EngagementViewModel(
                p.ClientId,
                p.Name,
                p.ClientId,
                p.Color))
            .ToListAsync();
    }
}