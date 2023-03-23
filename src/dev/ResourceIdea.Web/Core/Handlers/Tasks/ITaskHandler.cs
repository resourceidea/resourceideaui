namespace ResourceIdea.Web.Core.Handlers.Tasks;

public interface ITaskHandler
{
    /// <summary>
    /// Get paginated list of Tasks.
    /// </summary>
    /// <param name="subscriptionCode">Company subscription code.</param>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="filters">Filters for the list</param>
    /// <param name="search">Search phrase</param>
    /// <returns>List of engagements</returns>
    Task<IList<TaskViewModel>> GetPaginatedListAsync(
        string? subscriptionCode,
        int currentPage,
        int pageSize = 10,
        Dictionary<string, string>? filters = null,
        string? search = null);

    /// <summary>
    /// Get a count of engagements for a company subscription code that match a search phrase
    /// </summary>
    /// <param name="subscriptionCode">Company subscription code.</param>
    /// <param name="filters">Filters</param>
    /// <param name="search">Search phrase.</param>
    /// <returns>Count of engagements</returns>
    Task<int> GetCountAsync(string? subscriptionCode, Dictionary<string, string>? filters, string? search);

    /// <summary>
    /// Get an engagement by Id.
    /// </summary>
    /// <param name="subscriptionCode">Company subscription code.</param>
    /// <param name="taskId">Task Id</param>
    /// <returns>Engagement</returns>
    Task<TaskViewModel?> GetEngagementByIdAsync(string? subscriptionCode, string? taskId);

    /// <summary>
    /// Update engagement details.
    /// </summary>
    /// <param name="subscriptionCode">Company subscription code.</param>
    /// <param name="input">Engagement update details.</param>
    /// <returns></returns>
    Task UpdateAsync(string? subscriptionCode, TaskViewModel input);

    /// <summary>
    /// Add engagement to the store.
    /// </summary>
    /// <param name="subscriptionCode">Subscription code.</param>
    /// <param name="engagement">Engagement to be stored.</param>
    /// <returns>Engagement ID.</returns>
    Task<string> AddAsync(string? subscriptionCode, TaskViewModel engagement);
}
