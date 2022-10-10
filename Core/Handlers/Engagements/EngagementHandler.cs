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
    public async Task<IList<EngagementViewModel>> GetPaginatedResultAsync(
        string? subscriptionCode, 
        int currentPage, 
        int pageSize = 10, 
        string? search = null)
    {
        ArgumentNullException.ThrowIfNull(subscriptionCode);

        var data = await GetDataAsync(subscriptionCode, search);
        return data.OrderBy(d => d.Name)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public async Task<int> GetCountAsync(string? subscriptionCode, string? search)
    {
        ArgumentNullException.ThrowIfNull(subscriptionCode);

        var data = await GetDataAsync(subscriptionCode, search);
        return data.Count;
    }

    public async Task<EngagementViewModel?> GetEngagementByIdAsync(string? subscriptionCode, string? engagementId)
    {
        ArgumentNullException.ThrowIfNull(subscriptionCode);
        ArgumentNullException.ThrowIfNull(engagementId);

        EngagementViewModel? result = null;

        var engagementQuery = await _dbContext.Projects.SingleOrDefaultAsync(c => c.ClientId == engagementId);
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

        var engagementForUpdate = await _dbContext.Projects
            .SingleOrDefaultAsync(p => p.Client.CompanyCode == subscriptionCode && p.ProjectId == input.ProjectId);

        if (engagementForUpdate is not null)
        {
            engagementForUpdate.Color = input.Color;
            engagementForUpdate.Name = input.Name ?? "N/A";

            await _dbContext.SaveChangesAsync();
        }

        ArgumentNullException.ThrowIfNull(input.ProjectId, nameof(input.ProjectId));
    }

    private async Task<IList<EngagementViewModel>> GetDataAsync(string? subscriptionCode, string? search)
    {
        ArgumentNullException.ThrowIfNull(subscriptionCode);

        var data = _dbContext.Projects
            .Where(p => p.Client.CompanyCode == subscriptionCode);

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