namespace ResourceIdea.Core.Handlers.Clients;

public interface IClientsHandler
{
    Task<IList<ClientViewModel>> GetPaginatedResultAsync(string subscriptionCode, int currentPage, int pageSize = 10);
    Task<int> GetCountAsync(string subscriptionCode);
}