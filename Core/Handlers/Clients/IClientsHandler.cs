namespace ResourceIdea.Core.Handlers.Clients;

public interface IClientsHandler
{
    Task<IList<ClientViewModel>> GetPaginatedResultAsync(string? subscriptionCode, int currentPage, int pageSize = 10, string? search = null);
    Task<int> GetCountAsync(string? subscriptionCode, string? search);

    Task<ClientViewModel?> GetClientByIdAsync(string?  subscriptionCode, string? clientId);
}