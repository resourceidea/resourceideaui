namespace ResourceIdea.Core.Handlers.Clients;

public class ClientsHandler : IClientsHandler
{
    private readonly ResourceIdeaDBContext _dbContext;

    public ClientsHandler(ResourceIdeaDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<ClientViewModel>> GetPaginatedResultAsync(string subscriptionCode, int currentPage,
        int pageSize = 10)
    {
        var data = await GetDataAsync(subscriptionCode);
        return data.OrderBy(d => d.Name)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public async Task<int> GetCountAsync(string? subscriptionCode)
    {
        ArgumentNullException.ThrowIfNull(subscriptionCode);
        
        var data = await GetDataAsync(subscriptionCode);
        return data.Count;
    }

    private async Task<IList<ClientViewModel>> GetDataAsync(string? subscriptionCode)
    {
        ArgumentNullException.ThrowIfNull(subscriptionCode);
        
        return await _dbContext.Clients
            .Where(c => c.CompanyCode == subscriptionCode)
            .Select(c => new ClientViewModel(c.Name, c.Address, c.Industry))
            .ToListAsync();
    }
}