namespace ResourceIdea.Web.Core.Handlers.Clients;

/// <summary>
/// Clients handling interface.
/// </summary>
public interface IClientsHandler
{
    /// <summary>
    /// Get paginated list of clients.
    /// </summary>
    /// <param name="subscriptionCode">Subscription code of the company whose clients are to be listed</param>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Pagination size.</param>
    /// <param name="search">Search phrase.</param>
    /// <returns>List of clients.</returns>
    Task<IList<ClientViewModel>> GetPaginatedResultAsync(string? subscriptionCode, int currentPage, int pageSize = 10, string? search = null);

    /// <summary>
    /// Get the count of clients.
    /// </summary>
    /// <param name="subscriptionCode">Company subscription code.</param>
    /// <param name="search">Search phrase.</param>
    /// <returns>Clients count.</returns>
    Task<int> GetCountAsync(string? subscriptionCode, string? search);

    /// <summary>
    /// Get client by Id.
    /// </summary>
    /// <param name="subscriptionCode">Company subscription code.</param>
    /// <param name="clientId">Client Id.</param>
    /// <returns>Client</returns>
    Task<ClientViewModel?> GetClientByIdAsync(string?  subscriptionCode, string? clientId);

    /// <summary>
    /// Update client details.
    /// </summary>
    /// <param name="subscriptionCode">Company subscription code.</param>
    /// <param name="clientViewModel">Client update details.</param>
    /// <returns></returns>
    System.Threading.Tasks.Task UpdateAsync(string? subscriptionCode, ClientViewModel clientViewModel);
    
    /// <summary>
    /// Add client.
    /// </summary>
    /// <param name="subscriptionCode">Company subscription code.</param>
    /// <param name="client">Client details to add.</param>
    /// <returns>Client Id.</returns>
    Task<string> AddAsync(string? subscriptionCode, ClientViewModel client);
}