using ResourceIdea.Core.ViewModels;

namespace ResourceIdea.Core.Handlers.Engagements;

public interface IEngagementHandler
{
    /// <summary>
    /// Get paginated list of engagements.
    /// </summary>
    /// <param name="subscriptionCode">Company subscription code.</param>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="search">Search phrase</param>
    /// <returns>List of engagements</returns>
    Task<IList<EngagementViewModel>> GetPaginatedResultAsync(string? subscriptionCode, int currentPage, int pageSize = 10, string? search = null);
    
    /// <summary>
    /// Get a count of engagements for a company subscription code that match a search phrase
    /// </summary>
    /// <param name="subscriptionCode">Company subscription code.</param>
    /// <param name="search">Search phrase.</param>
    /// <returns>Count of engagements</returns>
    Task<int> GetCountAsync(string? subscriptionCode, string? search);

    /// <summary>
    /// Get an engagement by Id.
    /// </summary>
    /// <param name="subscriptionCode">Company subscription code.</param>
    /// <param name="engagementId">Engagement Id</param>
    /// <returns>Engagement</returns>
    Task<EngagementViewModel?> GetEngagementByIdAsync(string? subscriptionCode, string? engagementId);

    /// <summary>
    /// Update engagement details.
    /// </summary>
    /// <param name="subscriptionCode">Company subscription code.</param>
    /// <param name="input">Engagement update details.</param>
    /// <returns></returns>
    Task UpdateAsync(string? subscriptionCode, EngagementViewModel input);
}
