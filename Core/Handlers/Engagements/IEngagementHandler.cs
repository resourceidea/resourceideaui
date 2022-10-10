using ResourceIdea.Core.ViewModels;

namespace ResourceIdea.Core.Handlers.Engagements;

public interface IEngagementHandler
{
    Task<IList<EngagementViewModel>> GetPaginatedResultAsync(string? subscriptionCode, int currentPage, int pageSize = 10, string? search = null);
    Task<int> GetCountAsync(string? subscriptionCode, string? search);
    Task<EngagementViewModel?> GetEngagementByIdAsync(string? subscriptionCode, string? engagementId);
    Task UpdateAsync(string? subscriptionCode, EngagementViewModel input);
}
